using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyNavigation.ViewModels.Base;
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
        private void NanigationToMain() =>
            _navigationService.DestroyAndNavigate<MainViewModel>();
    }
}
