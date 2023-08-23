using System.Collections.Generic;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Infraestructura.Contracts.Services;
public interface ITareasProgramadasServicio
{
    Task<List<TareaProgramada>> ObtenerPorObjeto(int idObjeto);
    Task<List<TareaProgramada>> ObtenerTodos();
    Task<bool> Sincronizar(string dbDescarga, int idDispositivo);
    Task<int> Agregar(TareaProgramada tareaProgramada);
    Task<bool> Modificar(TareaProgramada tareaProgramada);
    Task<bool> BorrarRecuperarRegistro(TareaProgramada tareaProgramada);
}
