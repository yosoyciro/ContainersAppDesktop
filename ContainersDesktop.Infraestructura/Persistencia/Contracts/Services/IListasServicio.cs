using System.Collections.Generic;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Infraestructura.Contracts.Services;
public interface IListasServicio
{
    Task<List<Listas>> ObtenerListas();
    Task<int> CrearLista(Listas lista);
    Task<bool> ActualizarLista(Listas lista);
    Task<bool> BorrarRecuperarLista(Listas lista);
}
