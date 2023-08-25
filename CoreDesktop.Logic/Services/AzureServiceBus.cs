using System.Text;
using CoreDesktop.Dominio.Models.Mensajeria;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CoreDesktop.Logica.Mensajeria.Services;
public class AzureServiceBus
{
    private readonly ServiceBusClient _client;
    private string MensajeRecibido = string.Empty;

    public AzureServiceBus(IConfiguration config)
    {
        _client = new ServiceBusClient(config.GetConnectionString("ServiceBusKey"));
    }

    public async Task EnviarMensaje<T>(T @event) where T : Command
    {
        const string queueName = "desktopamobiles";
        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

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

    public async Task<string> RecibirMensajes()
    {
        var queueName = "mobiletodesktop";
        ServiceBusReceiver receiver = _client.CreateReceiver(queueName);

        try
        {
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
            MensajeRecibido = receivedMessage?.Body?.ToString();
            if (string.IsNullOrEmpty(MensajeRecibido))
            {
                return string.Empty;
            }

            await receiver.CompleteMessageAsync(receivedMessage);
            return MensajeRecibido;

        }
        catch (Exception)
        {
            throw;
        }
    }
}
