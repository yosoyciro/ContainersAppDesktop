using ContainersDesktop.Core.Models;

namespace ContainersDesktop.Core.Contracts.Services;
public interface IListasServicio
{
    Task<List<Listas>> ObtenerListas(bool verTodos);
    Task<bool> CrearLista(Listas lista);
    Task<bool> ActualizarLista(Listas lista);
    Task<bool> BorrarLista(int id);
}
