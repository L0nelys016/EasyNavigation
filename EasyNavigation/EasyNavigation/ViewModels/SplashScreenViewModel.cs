using EasyNavigation.ViewModels.Base;
using Navigation.Abstractions;

namespace EasyNavigation.ViewModels
{
    public class SplashScreenViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public SplashScreenViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void NanigationToMain() => _navigationService.DestroyAndNavigate<MainViewModel>();
    }
}
