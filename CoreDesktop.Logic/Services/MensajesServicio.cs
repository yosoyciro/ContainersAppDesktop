using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using Azure.Messaging.ServiceBus;
using System.Text;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Mensajeria;

namespace ContainersDesktop.Logica.Services;
public class MensajesServicio
{
    private readonly IMensajeRepository<Mensaje> _repository;
    private readonly MensajesServicioProcesar _mensajesServicioProcesar;

    public MensajesServicio(IMensajeRepository<Mensaje> repository, MensajesServicioProcesar mensajesServicioProcesar)
    {
        _repository = repository;
        _mensajesServicioProcesar = mensajesServicioProcesar;
    }

    public async Task Guardar(ServiceBusReceivedMessage message)
    {
        try
        {
            var body = message.Body;
            var text = Encoding.UTF8.GetString(body);            

            var mensaje = new Mensaje(text, string.Empty, FormatoFecha.FechaEstandar(DateTime.Now), "Pendiente");
            await _repository.AddAsync(mensaje);            
        }
        catch (Exception)
        {
            throw;
        }        
    }

    public async Task ProcesarPendientes()
    {
        try
        {
            var mensajes = await _repository.GetAll();

            foreach (var mensaje in mensajes)
            {
                if (await _mensajesServicioProcesar.ProcesarMensaje(mensaje))
                {
                    await _repository.DeleteAsync(mensaje);
                }
            }

            var mensajesSinProcesar = await ConsultarSinProcesar();
                           
            //_mensajesNotificaciones.SetMensajesNoLeidos(mensajesSinProcesar);        
        }
        catch (Exception)
        {

            throw;
        }
        
    }

    public async Task<int> ConsultarSinProcesar()
    {
        try
        {
            var mensajes = await _repository.GetAll();

            return mensajes.Count;
        }
        catch (Exception)
        {

            throw;
        }

    }
}
