using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models.Base;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using CoreDesktop.Dominio.Models;
using Microsoft.EntityFrameworkCore;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class MensajeRepository<T> : IMensajeRepository<T> where T : BaseEntity
{
    private readonly ContainersDbContext _context;

    public MensajeRepository(ContainersDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return await _context.SaveChangesAsync();
    }

    public Task<List<T>> GetAll()
    {
        return _context.Set<T>().ToListAsync();
    }
}
