using System.Reflection.Metadata.Ecma335;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Dominio.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class ConfigRepository<T> : IConfigRepository<T> where T : ConfigBaseEntity
{
    private readonly ContainersDbContext _context;

    public ConfigRepository(ContainersDbContext context)
    {
        _context = context;
    }
    public async Task Borrar(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }
    public async Task Guardar(T entity)
    {
        try
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
        
    }
    public async Task<T> Leer(string clave)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(x => x.Clave == clave);
        
    }

    public async Task<List<T>> LeerTodas()
    {
        try
        {
            return await _context.Set<T>().ToListAsync();
        }
        catch (Exception)
        {

            throw;
        }
        

    }
}
