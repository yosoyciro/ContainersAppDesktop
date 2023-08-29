using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Infraestructura.Persistencia.Contracts.Repositories;
public interface IAsyncRepository<T> where T : BaseEntity
{
    Task<int> AddAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<List<T>> GetAsync();
    Task<T?> GetByIdAsync(int id);
}
