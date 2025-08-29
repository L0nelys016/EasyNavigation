using Navigation.Abstractions.Base;
using System;

namespace EasyNavigation.ViewModels;

public class ViewModelBase : TemplateViewModel
{
    protected bool IsDisposed { get; set; } = false;

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        GC.SuppressFinalize(this);
        IsDisposed = true;
    }

    public override void RefreshPage() { }

    protected override void InitializeParams<T>(T _params) { }
}
