using ContainersDesktop.Core.Models;

namespace ContainersDesktop.Core.Contracts.Services;
public interface IDispositivosServicio
{
    Task<List<Dispositivos>> ObtenerDispositivos();
    Task<int> CrearDispositivo(Dispositivos dispositivo);
    Task<bool> ActualizarDispositivo(Dispositivos dispositivo);
    Task<bool> BorrarDispositivo(int id);
}
