using ContainersDesktop.Core.Models;

namespace ContainersDesktop.Core.Contracts.Services;
public interface IMovimientosServicio
{
    Task<List<Movim>> ObtenerMovimientos(int idObjeto);
    Task<bool> SincronizarMovimientos(string dbDescarga);
}
