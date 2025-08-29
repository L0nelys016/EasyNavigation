using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Navigation.Abstractions;
using Navigation.Abstractions.Base;
using Navigation.Options;

namespace Navigation.NavigationServices
{
    public class NavigationService : INavigationService
    {
        private readonly INavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        private Stack<TemplateViewModel> _historyViewModel = new();
        private readonly int _maxHistoryViewModel;

        public NavigationService(
            INavigationStore navStore,
            IServiceProvider serProvider,
            ILogger<NavigationService> logger,
            IOptions<NavigateHistory> options
        )
        {
            _navigationStore = navStore;
            _serviceProvider = serProvider;
            _maxHistoryViewModel = options.Value.MaxNavigateHistory;
            _logger = logger;
        }

        public bool HistoryIsNotEmpty => _historyViewModel.Count > 0;

        private Action<TemplateViewModel?>? OverlayAction { get; set; }

        public void CloseOverlay()
        {
            if (!HistoryIsNotEmpty)
            {
                _logger.LogError("Попытка закрытия оверлей окна при пустой истории");
                return;
            }

            TemplateViewModel viewModel = _historyViewModel.Pop();

            viewModel?.Dispose();

            OverlayAction?.Invoke(null);

            OverlayAction = null;

            _logger.LogInformation("Закрытие оверлей окна");
        }

        public void DestroyAndNavigate<TViewModel>()
            where TViewModel : TemplateViewModel
        {
            TemplateViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            DisposeAndSetViewModel(viewModel);
            _logger.LogInformation($"Переход к {viewModel.GetType().Name} без сохранения истории");
        }

        public void DestroyAndNavigate<TViewModel, TParams>(TParams _params)
            where TViewModel : TemplateViewModel
        {
            TemplateViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            DisposeAndSetViewModel(viewModel);
            viewModel.Initialize(_params);
            _logger.LogInformation(
                $"Переход к {viewModel.GetType().Name} без сохранения истории c передачей параметров: {_params}"
            );
        }

        public void Navigate<TViewModel>()
            where TViewModel : TemplateViewModel
        {
            TemplateViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            AddToHistoryAndSetViewModel(viewModel);
            _logger.LogInformation($"Переход к {viewModel.GetType().Name}");
        }

        public void Navigate<TViewModel, TParams>(TParams _params)
            where TViewModel : TemplateViewModel
        {
            TemplateViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();
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
            TemplateViewModel viewModel = _historyViewModel.Pop();
            _navigationStore.CurrentViewModel = viewModel;
            _logger.LogInformation($"Возврат к {viewModel.GetType().Name}");
        }

        public void NavigateOverlay<TViewModel>(
            Action<TemplateViewModel?>? overlayAction = null,
            Action? onClose = null
        )
            where TViewModel : TemplateViewModel
        {
            TemplateViewModel? viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            overlayAction?.Invoke(viewModel);

            OverlayAction = vm =>
            {
                overlayAction?.Invoke(null);
                onClose?.Invoke();
            };

            _historyViewModel.Push(viewModel);

            _logger.LogInformation($"Оверлей навигация на {viewModel.GetType().Name}");
        }

        public void NavigateOverlay<TViewModel, TParam>(
            TParam _params,
            Action<TemplateViewModel?>? overlayAction = null,
            Action? onClose = null
        )
            where TViewModel : TemplateViewModel
        {
            TemplateViewModel viewModel = _serviceProvider.GetRequiredService<TViewModel>();

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
            where TViewModel : TemplateViewModel
        {
            DestroyAndNavigate<TViewModel>();
            _historyViewModel.Clear();
            _logger.LogInformation($"История была очищена");
        }

        public void ResetAndNavigate<TViewModel, TParams>(TParams _params)
            where TViewModel : TemplateViewModel
        {
            DestroyAndNavigate<TViewModel, TParams>(_params);
            _historyViewModel.Clear();
            _logger.LogInformation($"История была очищена");
        }

        private void AddToHistoryAndSetViewModel(TemplateViewModel viewModel)
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

        private void DisposeAndSetViewModel(TemplateViewModel viewModel)
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
            TemplateViewModel viewModel = _historyViewModel.Pop();
            viewModel.Dispose();
            _historyViewModel = new(_historyViewModel);
            return viewModel.GetType().Name;
        }
    }
}
