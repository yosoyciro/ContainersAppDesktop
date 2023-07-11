using ContainersDesktop.Activation;
using ContainersDesktop.Contracts.Services;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models.Storage;
using ContainersDesktop.Core.Persistencia;
using ContainersDesktop.Core.Services;
using ContainersDesktop.Helpers;
using ContainersDesktop.Models;
using ContainersDesktop.Services;
using ContainersDesktop.ViewModels;
using ContainersDesktop.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace ContainersDesktop;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        //Inicio Database
        InicializarDB.InicializarBase();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();
            services.AddTransient<IObjetosServicio, ObjetosServicio>();
            services.AddTransient<IListasServicio, ListasServicio>();
            services.AddTransient<IClaListServicio, ClaListServicio>();
            services.AddTransient<IDispositivosServicio, DispositivosServicio>();
            services.AddTransient<IMovimientosServicio, MovimientosServicio>();
            services.AddTransient<ISincronizacionServicio, SincronizacionServicio>();
            services.AddTransient<AzureStorageManagement>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();            
            services.AddTransient<ContainersGridViewModel>();
            services.AddTransient<ContainersGridPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<DispositivosViewModel>();
            services.AddTransient<DispositivosPage>();
            services.AddTransient<TiposListaDetailsViewModel>();
            services.AddTransient<TiposListaDetailsPage>();
            services.AddTransient<ListaPorTipoViewModel>();
            services.AddTransient<ListaPorTipoPage>();
            services.AddTransient<MovimientosViewModel>();
            services.AddTransient<MovimientosPage>();
            services.AddTransient<MovimientosContainerPage>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginPage>();
            services.AddTransient<SincronizacionesViewModel>();
            services.AddTransient<SincronizacionesPage>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            services.Configure<Settings>(options => context.Configuration.GetSection("Settings").Bind(options));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
