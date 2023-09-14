using System.Text;
using ContainersDesktop.Dominio.Models.Mensajeria;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace ContainersDesktop.Logica.Mensajeria.Services;
public class AzureServiceBus
{
    private readonly ServiceBusClient _client;
    private readonly ILogger<AzureServiceBus> _logger;

    public AzureServiceBus(IConfiguration config, ILogger<AzureServiceBus> logger)
    {
        _client = new ServiceBusClient(config.GetConnectionString("ServiceBusKey"));
        _logger = logger;
    }

    public async Task<ServiceBusClient> GetInstance()
    {
        return _client;
    }


    public async Task EnviarMensaje<T>(T @message) where T : Message
    {        
        try
        {
            const string queueName = "datos-a-mobiles";
            var mensaje = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(mensaje);

            ServiceBusSender sender = _client.CreateSender(queueName);
            ServiceBusMessage messageBus = new ServiceBusMessage(body);

            await sender.SendMessageAsync(messageBus);
            _logger.LogInformation("Message sent to the queue successfully.");

            await sender.DisposeAsync();        
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }          
    }
}
