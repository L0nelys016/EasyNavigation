using CommunityToolkit.Mvvm.Input;
using Navigation.Abstractions;

namespace EasyNavigation.ViewModels
{
    public partial class AuthorizationViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public AuthorizationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private void NavigationToMain() =>
            _navigationService.Navigate<MainViewModel>();


    }
}
