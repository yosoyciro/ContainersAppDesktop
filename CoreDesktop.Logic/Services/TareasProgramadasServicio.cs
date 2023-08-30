using Microsoft.Extensions.Logging;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using CoreDesktop.Logic.Contracts;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class TareasProgramadasServicio : IServiciosRepositorios<TareaProgramada>
{
    private readonly ILogger<TareasProgramadasServicio> _logger;
    private readonly IAsyncRepository<TareaProgramada> _tareasProgramadasRepo;

    public TareasProgramadasServicio(ILogger<TareasProgramadasServicio> logger, IAsyncRepository<TareaProgramada> tareasProgramadasRepo)
    {
        _logger = logger;
        _tareasProgramadasRepo = tareasProgramadasRepo;
    }

    public async Task<List<TareaProgramada>> ObtenerPorObjeto(int idObjeto)
    {
        return null;
    }

    public async Task<List<TareaProgramada>> GetAsync()
    {
        return await _tareasProgramadasRepo.GetAsync();
    }

    public async Task<TareaProgramada?> GetById(int id)
    {
        return await _tareasProgramadasRepo.GetByIdAsync(id);
    }

    public async Task<int> AddAsync(TareaProgramada tareaProgramada)
    {
        try
        {
            return await _tareasProgramadasRepo.AddAsync(tareaProgramada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(TareaProgramada tareaProgramada)
    {
        try
        {
            await _tareasProgramadasRepo.UpdateAsync(tareaProgramada);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<bool> DeleteRecover(TareaProgramada tareaProgramada)
    {
        try
        {
            await _tareasProgramadasRepo.UpdateAsync(tareaProgramada);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}