using Navigation.Abstractions;
using Navigation.Abstractions.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNavigation.ViewModels.Base
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationStore _navigationStore;

        public TemplateViewModel? currentViewModel => _navigationStore.CurrentViewModel;


        public MainWindowViewModel(INavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PropertyChanged += OnViewModelChanged;
        }

        private void OnViewModelChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_navigationStore.CurrentViewModel))
                OnPropertyChanged(nameof(currentViewModel));
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            _navigationStore.PropertyChanged -= OnViewModelChanged;
            currentViewModel?.Dispose();
            base.Dispose();
        }
    }
}
