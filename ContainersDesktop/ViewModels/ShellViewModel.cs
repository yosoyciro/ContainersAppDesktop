using Azure.Messaging.ServiceBus;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.Services;
using ContainersDesktop.Views;
using CoreDesktop.Logica.Services;
using CoreDesktop.Logica.Mensajeria.Services;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Navigation;

namespace ContainersDesktop.ViewModels;

public partial class ShellViewModel : ObservableRecipient, IDisposable
{
    //private readonly ServiceBusClient _serviceBusClient;
    private readonly ILogger<ShellViewModel> _logger;
    private readonly MensajesServicio _mensajesServicio;
    private readonly AzureServiceBus _azureServiceBus;
    private ServiceBusClient _serviceBusClient;

    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }
    public LoginViewModel _loginViewModel;

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService, LoginViewModel loginViewModel, ILogger<ShellViewModel> logger, MensajesServicio mensajesServicio, AzureServiceBus azureServiceBus)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        _loginViewModel = loginViewModel;

        //_serviceBusClient = new ServiceBusClient("Endpoint=sb://labservicebusunicom.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XmfSa2X6mF5uJ9BB5Wf+aFZ110sxQY1xM+ASbBsYScI=");
        _logger = logger;
        _mensajesServicio = mensajesServicio;
        _azureServiceBus = azureServiceBus;

        IniciarServiceBusProcesor().Wait();

        ProcesarMensajesGuardados().Wait();        
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }

    public async Task IniciarServiceBusProcesor()
    {
        _serviceBusClient = await _azureServiceBus.GetInstance();
        var processor = _serviceBusClient.CreateProcessor("mobile-a-desktop");
        //var processor = client.CreateProcessor(topicName, subscriptionName);
        processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
        processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
        await processor.StartProcessingAsync();
    }

    private async Task Processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        //Console.WriteLine("Received Processor Message: " + message.Body);        
        var result = await _mensajesServicio.Guardar(message);
        if (result)
        {
            await arg.CompleteMessageAsync(message);
        }
    }

    private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception.ToString());
        return Task.CompletedTask;
    }

    private async Task ProcesarMensajesGuardados()
    {
        await _mensajesServicio.ProcesarPendientes();
    }

    public void Dispose()
    {
        _serviceBusClient.DisposeAsync();
    }
}
