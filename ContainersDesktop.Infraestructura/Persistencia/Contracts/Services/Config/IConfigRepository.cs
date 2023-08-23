using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Infraestructura.Contracts.Services.Config;
public interface IConfigRepository<T> where T : ConfigBaseEntity
{    
    Task<List<T>> LeerTodas();
    Task Guardar(T entity);
    Task<T> Leer(string clave);
    Task Borrar(T entity);   
}
