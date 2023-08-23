using System.Collections.Generic;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Infraestructura.Contracts.Services;
public interface IClaListServicio
{
    Task<List<ClaList>> ObtenerClaListas();
    Task<bool> CrearClaLista(ClaList claList);
    Task<bool> ActualizarClaLista(ClaList claList);
    Task<bool> BorrarClaLista(int id);
}
