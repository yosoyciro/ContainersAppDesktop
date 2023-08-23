using System.Collections.Generic;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Infraestructura.Contracts.Services;
public interface IDispositivosServicio
{
    Task<List<Dispositivos>> ObtenerDispositivos();
    Task<int> CrearDispositivo(Dispositivos dispositivo);
    Task<bool> ActualizarDispositivo(Dispositivos dispositivo);
    Task<bool> BorrarRecuperarDispositivo(int id, string accion);
    Task<bool> ExisteContainer(string cloudContainer);
}
