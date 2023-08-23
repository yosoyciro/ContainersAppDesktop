using System.Collections.Generic;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Infraestructura.Contracts.Services;
public interface IObjetosServicio
{
    Task<List<Objetos>> ObtenerObjetos();
    Task<Objetos> ObtenerObjetoPorId(int id);
    Task<int> CrearObjeto(Objetos objeto);
    Task<bool> ActualizarObjeto(Objetos objeto);
    Task<bool> BorrarRecuperarRegistro(Objetos objeto);
}
