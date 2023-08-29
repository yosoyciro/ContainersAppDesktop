using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Infraestructura.Persistencia.Contracts.Repositories;
public interface IMensajeRepository<T> where T : BaseEntity
{
    Task<int> AddAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<List<T>> GetAll();
}
