using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models.Base;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Contracts.Services.Config;
using CoreDesktop.Dominio.Models.Base;
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
            var entidad = _context.Set<T>().FirstOrDefault(x => x.Clave == entity.Clave);

            if (entidad == null)
            {
                _context.Set<T>().Add(entity);
            }
            else
            {
                entidad.Valor = entity.Valor;
                _context.Set<T>().Update(entidad);
            }
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
        return await _context.Set<T>().ToListAsync();

    }
}
