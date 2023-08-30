using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Base;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using CoreDesktop.Logic.Contracts;
using Microsoft.Extensions.Logging;

namespace CoreDesktop.Logic.Implementaciones;
public class ServiciosRepositorios<T> : IServiciosRepositorios<T> where T : BaseEntity
{
    private readonly ILogger<T> _logger;
    private readonly IAsyncRepository<T> _repo;

    public ServiciosRepositorios(ILogger<T> logger, IAsyncRepository<T> repo)
    {
        _logger = logger;
        _repo = repo;
    }

    //public async Task<List<TareaProgramada>> ObtenerPorObjeto(int idObjeto)
    //{
    //    return null;
    //}

    public async Task<List<T>> GetAsync()
    {
        return await _repo.GetAsync();
    }

    public async Task<T?> GetById(int id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<int> AddAsync(T entity)
    {
        try
        {
            return await _repo.AddAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            await _repo.UpdateAsync(entity);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<bool> DeleteRecover(T entity)
    {
        try
        {
            await _repo.UpdateAsync(entity);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}
