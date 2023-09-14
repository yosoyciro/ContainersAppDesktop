using System;
using System.Reflection;
using AutoMapper;
using ContainersDesktop.Activation;
using ContainersDesktop.Contracts.Services;
using ContainersDesktop.Core.Services;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Storage;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Persistencia;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Infraestructura.Persistencia.Repositorios;
using ContainersDesktop.Logica.Contracts;
using ContainersDesktop.Logica.Contracts.Services;
using ContainersDesktop.Logica.Services;
using ContainersDesktop.Services;
using ContainersDesktop.ViewModels;
using ContainersDesktop.Views;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Logica.Mapping;
using ContainersDesktop.Logica.Services;
using ContainersDesktop.Logica.Workers;
using ContainersDesktop.Logica.Mensajeria.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using ContainersDesktop.Logica.Mensajeria.Messages;
using ContainersDesktop.Logica.Mensajeria.MessageHandlers;

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
    public Window Window => MainWindow;

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();            

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<AzureStorageManagement>();
            services.AddSingleton<PlayFabServicio>();
            services.AddTransient<SincronizarServicio>();
            services.AddSingleton<AzureServiceBus>();
            services.AddSingleton<MensajesServicioProcesar>(sp =>
            {
                var scoopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new MensajesServicioProcesar(scoopeFactory);
            });
            services.AddSingleton<MensajesServicio>();
            

            services.AddTransient<IMensajeRepository<Mensaje>, MensajeRepository<Mensaje>>();

            //Message handlers
            services.AddScoped<IMessageHandler<ContainerModificado>, ContainerModificadoHandler>();
            services.AddScoped<ContainerModificadoHandler>();
            services.AddScoped<IMessageHandler<TareaProgramadaModificada>, TareaProgramadaModificadaHandler>();
            services.AddScoped<TareaProgramadaModificadaHandler>();
            services.AddScoped<IMessageHandler<MovimCreado>, MovimCreadoHandler>();
            services.AddScoped<MovimCreadoHandler>();
            services.AddScoped<IMessageHandler<TareaProgramadaArchivoCreado>, TareaProgramadaArchivoCreadoHandler>();
            services.AddScoped<TareaProgramadaArchivoCreadoHandler>();

            //services.AddHostedService<ServiceBusWorker>();

            //Config services
            services.AddTransient<IConfigRepository<UI_Config>, ConfigRepository<UI_Config>>();
            services.AddTransient<IConfigRepository<UI_Default>, ConfigRepository<UI_Default>>();
            //services.AddTransient(typeof(IConfigRepository<>), typeof(IConfigRepository<>));

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
            services.AddTransient<TareasProgramadasViewModel>();
            services.AddTransient<TareasProgramadasPage>();
            services.AddTransient<Data2MovieViewModel>();
            services.AddTransient<Data2MoviePage>();    

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            services.Configure<Settings>(options => context.Configuration.GetSection("Settings").Bind(options));
            services.Configure<AzureStorageConfig>(options => context.Configuration.GetSection("AzureStorageConfig").Bind(options));

            // DB Context
            services.AddDbContext<ContainersDbContext>(opt =>
                opt.UseSqlite($"Data Source={context.Configuration.GetConnectionString("DefaultConnection")}")
            );
            
            //HTTP client
            //services.AddHttpClient();

            //Logging            
            services.AddLogging(builder =>
            {
                // Only Application Insights is registered as a logger provider
                builder.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) => config.ConnectionString = "InstrumentationKey=49354931-44b9-4d92-906a-4967741d22d3;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/",
                    configureApplicationInsightsLoggerOptions: (options) => { }
                );
            });

            //Telemetría de la app
            services.AddApplicationInsightsTelemetryWorkerService();

            //Automapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }).
        Build();

        //Workers
        //Host.RunAsync();
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

        MainWindow.Maximize();
    }
}
