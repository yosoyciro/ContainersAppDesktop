using ContainersDesktop.Dominio.Models.Base;

namespace CoreDesktop.Logic.Contracts;
public interface IServiciosRepositorios<T> where T : BaseEntity
{
    Task<List<T>> GetAsync();
    Task<T?> GetById(int id);
    Task<int> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteRecover(T entity);
}
