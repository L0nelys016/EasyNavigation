using System.Data;
using System.Net.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Navigation.Abstractions;
using Navigation.Abstractions.Base;

namespace Navigation.NavigationService
{
    public class NavigationService : INavigationService
    {
        private readonly INavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        private Stack<BaseViewModel> _historyViewModel = new();
        private readonly int _maxHistoryViewModel;

        public NavigationService(
            INavigationStore navStore,
            IServiceProvider serProvider,
            ILogger logger,
            IOptions<BaseViewModel> options
        )
        {
            _navigationStore = navStore;
            _serviceProvider = serProvider;
            _logger = logger;
        }

        public bool HistoryIsNotEmpty => _historyViewModel.Count > 0;

        private Action<BaseViewModel?>? OverlayAction { get; set; }

        public void CloseOverlay()
        {
            if (!HistoryIsNotEmpty)
            {
                _logger.LogError("Попытка закрытия оверлей окна при пустой истории");
                return;
            }

            BaseViewModel viewModel = _historyViewModel.Pop();

            viewModel?.Dispose();

            OverlayAction?.Invoke(null);

            OverlayAction = null;

            _logger.LogInformation("Закрытие оверлей окна");
        }

        public void DestroyAndNavigate<TViewModel>()
            where TViewModel : BaseViewModel
        {
            BaseViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            DisposeAndSetViewModel(viewModel);
            _logger.LogInformation($"Переход к {viewModel.GetType().Name} без сохранения истории");
        }

        public void DestroyAndNavigate<TViewModel, TParams>(TParams _params)
            where TViewModel : BaseViewModel
        {
            BaseViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            DisposeAndSetViewModel(viewModel);
            viewModel.Initialize(_params);
            _logger.LogInformation(
                $"Переход к {viewModel.GetType().Name} без сохранения истории c передачей параметров: {_params}"
            );
        }

        public void Navigate<TViewModel>()
            where TViewModel : BaseViewModel
        {
            BaseViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            AddToHistoryAndSetViewModel(viewModel);
            _logger.LogInformation($"Переход к {viewModel.GetType().Name}");
        }

        public void Navigate<TViewModel, TParams>(TParams _params)
            where TViewModel : BaseViewModel
        {
            BaseViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            viewModel.Initialize(_params);
            AddToHistoryAndSetViewModel(viewModel);
            _logger.LogInformation(
                $"Переход к {viewModel.GetType().Name}, c передачей параметров: {_params}"
            );
        }

        public void NavigateBack()
        {
            if (!HistoryIsNotEmpty)
            {
                _logger.LogError($"Попытка перехода назад при пустой истории");
                return;
            }
            _navigationStore.CurrentViewModel?.Dispose();
            BaseViewModel viewModel = _historyViewModel.Pop();
            _navigationStore.CurrentViewModel = viewModel;
            _logger.LogInformation($"Возврат к {viewModel.GetType().Name}");
        }
        public void NavigateOverlay<TViewModel>(
            Action<BaseViewModel?>? overlayAction = null,
            Action? onClose = null
        )
            where TViewModel : BaseViewModel
        {
            BaseViewModel? viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            overlayAction?.Invoke( viewModel );

            OverlayAction = vm =>
            {
                overlayAction?.Invoke(null);
                onClose?.Invoke();
            };

            _historyViewModel.Push( viewModel );

            _logger.LogInformation($"Оверлей навигация на {viewModel.GetType().Name}");
        }

        public void NavigateOverlay<TViewModel, TParam>(
            TParam _params,
            Action<BaseViewModel?>? overlayAction = null,
            Action? onClose = null
        )
            where TViewModel : BaseViewModel
        {
            BaseViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();

            viewModel.Initialize(_params);

            overlayAction?.Invoke(viewModel);

            OverlayAction = vm =>
            {
                overlayAction?.Invoke(null);
                onClose?.Invoke();
            };

            _historyViewModel.Push(viewModel);

            _logger.LogInformation(
                $"Оверлей навигация на {viewModel.GetType().Name} с передачей параметров: {_params}"
            );
        }

        public void ResetAndNavigate<TViewModel>()
            where TViewModel : BaseViewModel
        {
            DestroyAndNavigate<TViewModel>();
            _historyViewModel.Clear();
            _logger.LogInformation($"История была очищена");
        }

        public void ResetAndNavigate<TViewModel, TParams>(TParams _params)
            where TViewModel : BaseViewModel
        {
            DestroyAndNavigate<TViewModel, TParams>(_params);
            _historyViewModel.Clear();
            _logger.LogInformation($"История была очищена");
        }

        private void AddToHistoryAndSetViewModel(BaseViewModel viewModel)
        {
            if (_navigationStore.CurrentViewModel != null)
            {
                if (_maxHistoryViewModel <= _historyViewModel.Count)
                {
                    string type = RemodeLastVm();
                    _logger.LogWarning(
                        $"Превышен лимит истории, будет удалена самая старая запись - {type}"
                    );
                }
                _historyViewModel.Push(_navigationStore.CurrentViewModel);
            }
            _navigationStore.CurrentViewModel = viewModel;
        }

        private void DisposeAndSetViewModel(BaseViewModel viewModel)
        {
            if (_navigationStore.CurrentViewModel != null)
            {
                _navigationStore.CurrentViewModel.Dispose();
            }
            _navigationStore.CurrentViewModel = viewModel;
        }

        private string RemodeLastVm()
        {
            _historyViewModel = new(_historyViewModel);
            BaseViewModel viewModel = _historyViewModel.Pop();
            viewModel.Dispose();
            _historyViewModel = new(_historyViewModel);
            return viewModel.GetType().Name;
        }
    }
}
