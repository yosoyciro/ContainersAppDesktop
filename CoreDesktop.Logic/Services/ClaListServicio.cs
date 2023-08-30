using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;

namespace ContainersDesktop.Logica.Servicios;
public class ClaListServicio
{
    //private readonly IAsyncRepository<ClaList> _claListRepo;

    //public ClaListServicio(IAsyncRepository<ClaList> claListRepo)
    //{
    //    _claListRepo = claListRepo;
    //}
    //public async Task<bool> ActualizarClaLista(ClaList claList)
    //{
    //    try
    //    {
    //        await _claListRepo.UpdateAsync(claList);            

    //        return true;
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    //public async Task<bool> CrearClaLista(ClaList claList)
    //{
    //    try
    //    {
    //        await _claListRepo.AddAsync(claList);

    //        return true;
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    //public async Task<List<ClaList>> ObtenerClaListas()
    //{
    //    return await _claListRepo.GetAsync();
    //}

    //public async Task<bool> BorrarClaLista(int id)
    //{
    //    try
    //    {
    //        var claList = await _claListRepo.GetByIdAsync(id);

    //        claList!.CLALIST_ID_ESTADO_REG = "B";
    //        claList!.CLALIST_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}
}
