using CoreDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Infraestructura.Persistencia.Contracts;
public interface IConfigRepository<T> where T : ConfigBaseEntity
{
    Task<List<T>> LeerTodas();
    Task Guardar(T entity);
    Task<T> Leer(string clave);
    Task Borrar(T entity);
}
