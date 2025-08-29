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
            where TViewModel : TemplateViewModel;

        void Navigate<TViewModel, TParams>(TParams _params)
            where TViewModel : TemplateViewModel;

        public void DestroyAndNavigate<TViewModel>()
           where TViewModel : TemplateViewModel;

        public void DestroyAndNavigate<TViewModel, TParams>(TParams _params)
            where TViewModel : TemplateViewModel;

        public void ResetAndNavigate<TViewModel>()
            where TViewModel : TemplateViewModel;

        public void ResetAndNavigate<TViewModel, TParams>(TParams _params)
            where TViewModel : TemplateViewModel;

        public void NavigateOverlay<TViewModel>(
             Action<TemplateViewModel?>? overlayAction = null,
             Action? onClose = null
         )
             where TViewModel : TemplateViewModel;

        public void NavigateOverlay<TViewModel, TParam>(
           TParam _params,
           Action<TemplateViewModel?>? overlayAction = null,
           Action? onClose = null
       )
           where TViewModel : TemplateViewModel;
            
        public void CloseOverlay();

        void NavigateBack();
    }
}
