using System.ComponentModel;
using System.Runtime.CompilerServices;
using Navigation.Abstractions;
using Navigation.Abstractions.Base;

namespace Navigation.NavigationStores
{
    public class NavigationStore : INavigationStore
    {
        private TemplateViewModel? _currentViewModel;

        public TemplateViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
