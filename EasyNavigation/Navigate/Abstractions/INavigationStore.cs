using Navigation.Abstractions.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Abstractions
{
    public interface INavigationStore : INotifyPropertyChanged
    {
        TemplateViewModel? CurrentViewModel { get; set; }
    }
}
