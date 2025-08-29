using CommunityToolkit.Mvvm.ComponentModel;
using Navigation.Exceptions;

namespace Navigation.Abstractions.Base
{
    public abstract class TemplateViewModel : ObservableObject, IDisposable
    {
        public void Initialize<T>(T _params)
        {
            InitializeParams(_params);
        }

        public abstract void RefreshPage();

        protected abstract void InitializeParams<T>(T _params);

        protected static T GetAs<T>(object? _params)
        {
            if (_params is null && default(T) is null)
                return default!;

            if (_params is T t)
                return t;

            throw new TypeExceptions(typeof(T), _params?.GetType());
        }

        public abstract void Dispose();
    }
}
