using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EasyNavigation.ViewModels;
using EasyNavigation.ViewModels.Base;
using EasyNavigation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Navigation.Abstractions;
using Navigation.RegistService;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace EasyNavigation;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

    public IServiceProvider ServiceProvider
    {
        get => _serviceProvider ?? throw new InvalidOperationException("Сервис не инициализирован");
        set => _serviceProvider = value;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        InitializeServices(); 

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
        }

        INavigationService _navigationService =
            ServiceProvider.GetRequiredService<INavigationService>();

        _navigationService.Navigate<SplashScreenViewModel>();

        base.OnFrameworkInitializationCompleted();
    }

    private void InitializeServices()
    {
        ServiceCollection services = new ServiceCollection();

        ConfigureServices(services);

        services.UseMicrosoftDependencyResolver();

        ServiceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        ConfigureViewModelServices(services);
        ConfigureNavigationServices(services);
        ConfigureLoggerService(services);
    }

    private void ConfigureViewModelServices(IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<SplashScreenViewModel>();
        services.AddTransient<AuthorizationViewModel>();
        services.AddTransient<MainViewModel>();
    }

    private void ConfigureNavigationServices(IServiceCollection services)
    {
        NavigateServiceRegister.CreateServiceCollections(services);
    }

    private void ConfigureLoggerService(IServiceCollection services)
    {
        services
            .AddLogging(config =>
            {
                config.AddDebug();
                config.SetMinimumLevel(LogLevel.Information);
            })
            .UseMicrosoftDependencyResolver();
    }
}
