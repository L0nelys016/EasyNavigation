using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Navigation.Abstractions;

namespace EasyNavigation.ViewModels
{
    public partial class SplashScreenViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public SplashScreenViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        [RelayCommand]
        private void NavigationToAuth() =>
            _navigationService.DestroyAndNavigate<AuthorizationViewModel>();
    }
}
