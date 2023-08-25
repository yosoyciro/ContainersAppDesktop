using CoreDesktop.Logica.Mensajeria.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreDesktop.Logic.Workers;
public class ServiceBusWorker : BackgroundService
{
    private readonly ILogger<ServiceBusWorker> _logger;
    private readonly AzureServiceBus _azureServiceBus;
    
    public ServiceBusWorker(ILogger<ServiceBusWorker> logger, AzureServiceBus azureServiceBus)
    {
        _logger = logger;
        _azureServiceBus = azureServiceBus;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _azureServiceBus.RecibirMensajes();
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(60000, stoppingToken);
        }
    }
}
