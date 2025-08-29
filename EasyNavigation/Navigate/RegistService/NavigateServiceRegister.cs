using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Navigation.Abstractions;
using Navigation.NavigationServices;
using Navigation.NavigationStores;
using Navigation.Options;

namespace Navigation.RegistService
{
    public static class NavigateServiceRegister
    {
        public static void CreateServiceCollections(IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.SetMinimumLevel(LogLevel.Information);
            });

            _ = services.AddSingleton<INavigationStore, NavigationStore>();

            services.Configure<NavigateHistory>(opt => { });

            services.AddSingleton<INavigationService, NavigationService>();
        }
    }
}
