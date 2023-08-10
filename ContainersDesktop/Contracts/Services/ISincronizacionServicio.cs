using ContainersDesktop.Core.Models;

namespace ContainersDesktop.Core.Contracts.Services;
public interface ISincronizacionServicio
{
    Task<List<Sincronizaciones>> ObtenerSincronizaciones();
    Task<bool> CrearSincronizacion(Sincronizaciones sincronizacion);    
}
