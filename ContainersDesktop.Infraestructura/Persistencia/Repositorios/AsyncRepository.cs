using System.Collections.Generic;
using ContainersDesktop.Dominio.Models.Base;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using Microsoft.EntityFrameworkCore;
using Windows.Data.Xml.Dom;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
{
    private readonly ContainersDbContext _context;
    private readonly DbSet<T> _dbSet;
    public AsyncRepository(ContainersDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<int> AddAsync(T entity)
    {
        try
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ID;
        }
        catch (Exception)
        {
            throw;
        }        
    }

    public async Task<int> DeleteAsync(T entity)
    {
        try
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<List<T>> GetAsync()
    {
        try
        {
            return _context.Set<T>().AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            throw;
        }        
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<int> UpdateAsync(T entity)
    {
        try
        {
            //_context.Set<T>().Update(entity);
            var keys = GetPrimaryKeys(_context, entity);

            bool tracked = _context.Entry(entity).State != EntityState.Detached;

            if (tracked)
                return 0;

            if (keys != null)
            {

                var oldValues = _dbSet.Find(keys);

                _context.Entry(oldValues).CurrentValues.SetValues(entity);
            }
            else
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }

            return await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static object[] GetPrimaryKeys<T>(DbContext context, T value)
    {
        var keyNames = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
               .Select(x => x.Name).ToArray();
        var result = new object[keyNames.Length];
        for (int i = 0; i < keyNames.Length; i++)
        {
            result[i] = typeof(T).GetProperty(keyNames[i])?.GetValue(value);
        }
        return result;
    }
}
