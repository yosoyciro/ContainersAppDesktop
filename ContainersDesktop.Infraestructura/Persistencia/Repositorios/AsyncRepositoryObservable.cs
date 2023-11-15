using ContainersDesktop.Dominio.Models.Base;
using ContainersDesktop.Infraestructura.Contracts;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Infraestructura.Specification;
using Microsoft.EntityFrameworkCore;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class AsyncRepositoryObservable<T> : IAsyncRepositoryObservable<T> where T : BaseEntityObservable
{
    private readonly ContainersDbContext _context;
    private readonly DbSet<T> _dbSet;
    public AsyncRepositoryObservable(ContainersDbContext context)
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

            return entity.Id;
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

    //public async Task<IReadOnlyCollection<T?>> GetAllWithSpecsAsync(ISpecification<T> spec, bool disableTracking = true)
    //{
    //    if (disableTracking)
    //        return await ApplySpecification(spec).AsNoTracking().ToListAsync();

    //    return await ApplySpecification(spec).ToListAsync();
    //}

    //public async Task<T?> GetOneWithSpecsAsync(ISpecification<T> spec)
    //{
    //    var entity = await ApplySpecification(spec).FirstOrDefaultAsync();        

    //    return entity;
    //}

    //public async Task<int?> CountAsync(ISpecification<T> spec)
    //{
    //    return await ApplySpecification(spec).CountAsync();
    //}

    //private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    //{
    //    return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    //}

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
