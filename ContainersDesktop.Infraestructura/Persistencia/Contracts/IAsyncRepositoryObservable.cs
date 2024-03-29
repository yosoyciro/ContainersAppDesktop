﻿using ContainersDesktop.Dominio.Models.Base;
using ContainersDesktop.Infraestructura.Contracts;

namespace ContainersDesktop.Infraestructura.Persistencia.Contracts;
public interface IAsyncRepositoryObservable<T> where T : BaseEntityObservable
{
    Task<int> AddAsync(T entity);

    Task<int> UpdateAsync(T entity);

    Task<int> DeleteAsync(T entity);

    Task<List<T>> GetAsync();

    Task<T?> GetByIdAsync(int id);

    //Task<IReadOnlyCollection<T?>> GetAllWithSpecsAsync(ISpecification<T> spec, bool disableTracking = true);

    //Task<T?> GetOneWithSpecsAsync(ISpecification<T> spec);

    //Task<int?> CountAsync(ISpecification<T> spec);
}
