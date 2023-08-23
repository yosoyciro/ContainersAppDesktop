using System.Collections.Generic;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Infraestructura.Contracts.Services;
public interface ISincronizacionServicio
{
    Task<List<Sincronizaciones>> ObtenerSincronizaciones();
    Task<bool> CrearSincronizacion(Sincronizaciones sincronizacion);    
}
