using System.Collections.Generic;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Infraestructura.Contracts.Services;
public interface IMovimientosServicio
{
    Task<List<Movim>> ObtenerMovimientosObjeto(int idObjeto);
    Task<List<Movim>> ObtenerMovimientosTodos();
    Task<bool> SincronizarMovimientos(string dbDescarga, int idDispositivo);
    Task<int> CrearMovimiento(Movim movim);
    Task<bool> ActualizarMovimiento(Movim movim);
    Task<bool> BorrarRecuperarRegistro(Movim movim);
}
