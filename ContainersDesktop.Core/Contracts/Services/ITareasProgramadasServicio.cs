using ContainersDesktop.Core.Models;

namespace ContainersDesktop.Core.Contracts.Services;
public interface ITareasProgramadasServicio
{
    Task<List<TareaProgramada>> ObtenerPorObjeto(int idObjeto);
    Task<List<TareaProgramada>> ObtenerTodos();
    Task<bool> Sincronizar(string dbDescarga, int idDispositivo);
    Task<int> Agregar(TareaProgramada tareaProgramada);
    Task<bool> Modificar(TareaProgramada tareaProgramada);
    Task<bool> Borrar(int id);
}
