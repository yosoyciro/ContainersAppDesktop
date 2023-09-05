using ContainersDesktop.Logica.Contracts;
using CoreDesktop.Dominio.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreDesktop.Logica.Services;
public class MensajesServicioProcesar
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MensajesServicioProcesar(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<bool> ProcesarMensaje(Mensaje mensaje)
    {        
        using (var scoope = _scopeFactory.CreateScope())
        {
            try
            {
                dynamic data = JObject.Parse(mensaje.MENSAJES_BODY!);
                var tipoMensaje = data.TipoMensaje;
                //result.
                Type tipoMensajeType = Type.GetType($"ContainersDesktop.Logica.Mensajeria.Messages.{tipoMensaje}, ContainersDesktop.Logica");
                var mensajeObject = JsonConvert.DeserializeObject(mensaje.MENSAJES_BODY, tipoMensajeType);

                //var text = Encoding.UTF8.GetString(body);
                Type handlerType = Type.GetType($"ContainersDesktop.Logica.Mensajeria.MessageHandlers.{tipoMensaje}Handler, ContainersDesktop.Logica");
                var handler = scoope.ServiceProvider.GetService(handlerType);

                var concreteType = typeof(IMessageHandler<>).MakeGenericType(tipoMensajeType);

                await (Task)concreteType!.GetMethod("Handle").Invoke(handler!, new object[] { mensajeObject });

                return true;
            }
            catch (Exception)
            {
                throw;
            }            
        }
    }
}
