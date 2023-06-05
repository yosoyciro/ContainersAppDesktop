using ContainersDesktop.Core.Models;

namespace ContainersDesktop.Core.Contracts.Services;
public interface IClaListServicio
{
    Task<List<ClaList>> ObtenerClaListas();
    Task<bool> CrearClaLista(ClaList claList);
    Task<bool> ActualizarClaLista(ClaList claList);
    Task<bool> BorrarClaLista(int id);
}
