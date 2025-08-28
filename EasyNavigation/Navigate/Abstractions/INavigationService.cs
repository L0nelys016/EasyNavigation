using Navigation.Abstractions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Abstractions
{
    public interface INavigationService
    {
        void Navigate<TViewModel>()
            where TViewModel : BaseViewModel;

        void Navigate<TViewModel, TParams>(TParams _params)
            where TViewModel : BaseViewModel;

        public void DestroyAndNavigate<TViewModel>()
           where TViewModel : BaseViewModel;

        public void DestroyAndNavigate<TViewModel, TParams>(TParams _params)
            where TViewModel : BaseViewModel;

        public void ResetAndNavigate<TViewModel>()
            where TViewModel : BaseViewModel;

        public void ResetAndNavigate<TViewModel, TParams>(TParams _params)
            where TViewModel : BaseViewModel;

        public void NavigateOverlay<TViewModel>()
            where TViewModel : BaseViewModel;

        public void NavigateOverlay<TViewModel, TParam>(
           TParam _params,
           Action<BaseViewModel?>? overlayAction = null,
           Action? onClose = null
       )
           where TViewModel : BaseViewModel;
            
        public void CloseOverlay();

        void NavigateBack();
    }
}
