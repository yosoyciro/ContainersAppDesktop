using System.Reflection.Metadata.Ecma335;
using ContainersDesktop.Dominio.Models.Base;
using ContainersDesktop.Infraestructura.Persistencia.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class AsyncRepository<T> : IAsyncRepository<T> where T : BaseEntity
{
    private readonly ContainersDbContext _context;
    public AsyncRepository(ContainersDbContext context)
    {
        _context = context;
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
        return _context.Set<T>().ToListAsync();
    }

    public Task<T?> GetByIdAsync(int id)
    {
        return _context.Set<T>().FirstOrDefaultAsync(x => x.ID == id);
    }

    public async Task<int> UpdateAsync(T entity)
    {
        try
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
