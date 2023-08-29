using System.Text;
using CoreDesktop.Dominio.Models.Mensajeria;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CoreDesktop.Logica.Mensajeria.Services;
public class AzureServiceBus
{
    private readonly ServiceBusClient _client;

    public AzureServiceBus(IConfiguration config)
    {
        _client = new ServiceBusClient(config.GetConnectionString("ServiceBusKey"));
    }

    public async Task<ServiceBusClient> GetInstance()
    {
        return _client;
    }


    public async Task EnviarMensaje<T>(T @message) where T : Message
    {
        const string queueName = "desktopamobiles";
        var mensaje = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(mensaje);

        ServiceBusSender sender = _client.CreateSender(queueName);
        ServiceBusMessage messageBus = new ServiceBusMessage(body);
        try
        {
            await sender.SendMessageAsync(messageBus);
            Console.WriteLine("Message sent to the queue successfully.");
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }
}
