using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using CoreDesktop.Logic.Services;

namespace ContainersDesktop.Logica.Services;
public class SincronizarServicio
{
    private readonly IAsyncRepository<Dispositivo> _dispositivosRepo;
    private readonly IAsyncRepository<Sincronizacion> _sincronizacionRepo;
    private readonly AzureStorageManagement _azureStorageManagement;
    private readonly MensajesServicio _mensajesServicio;

    public SincronizarServicio(AzureStorageManagement azureStorageManagement, IAsyncRepository<Dispositivo> dispositivosRepo, IAsyncRepository<Sincronizacion> sincronizacionRepo, MensajesServicio mensajesServicio)
    {
        _azureStorageManagement = azureStorageManagement;
        _dispositivosRepo = dispositivosRepo;
        _sincronizacionRepo = sincronizacionRepo;
        _mensajesServicio = mensajesServicio;
    }
    public async Task Sincronizar()
    {
        var dbDescarga = string.Empty;
        var fechaHoraInicio = DateTime.Now;
        var fechaHoraFin = DateTime.Now;
        var idDispositivo = 0;
        //Subo Base
        try
        {
            var dispositivos = await _dispositivosRepo.GetAsync();

            foreach (var item in dispositivos.Where(x => x.DISPOSITIVOS_ID_ESTADO_REG == "A" && !string.IsNullOrEmpty(x.DISPOSITIVOS_CONTAINER)))
            {
                idDispositivo = item.ID;
                fechaHoraInicio = DateTime.Now;

                ////Bajo del container
                //dbDescarga = await _azureStorageManagement.DownloadFile(item.DISPOSITIVOS_CONTAINER);

                //if (dbDescarga != string.Empty)
                //{
                //    //TODO - Proceso e incorporo los movimientos
                //    await _movimientosServicio.SincronizarMovimientos(dbDescarga, item.DISPOSITIVOS_ID_REG);
                //    await _tareasProgramadasServicio.Sincronizar(dbDescarga, item.DISPOSITIVOS_ID_REG);

                //    if (File.Exists(dbDescarga))
                //    {
                //        File.Delete(dbDescarga);
                //    }
                //}
                await _mensajesServicio.ProcesarPendientes();

                //Subo al contenedor
                await _azureStorageManagement.UploadFile(item.DISPOSITIVOS_CONTAINER);
                
                fechaHoraFin = DateTime.Now;

                //Grabo sincronizacion
                var sincronizacion = new Sincronizacion()
                {
                    SINCRONIZACIONES_FECHA_HORA_INICIO = FormatoFecha.FechaEstandar(fechaHoraInicio),
                    SINCRONIZACIONES_FECHA_HORA_FIN = FormatoFecha.FechaEstandar(fechaHoraFin),
                    SINCRONIZACIONES_DISPOSITIVO_ORIGEN = item.ID,
                    SINCRONIZACIONES_RESULTADO = "Ok",
                };
                await _sincronizacionRepo.AddAsync(sincronizacion);
            }
        }
        catch (SystemException ex)
        {
            //Grabo sincronizacion
            var sincronizacion = new Sincronizacion()
            {
                SINCRONIZACIONES_FECHA_HORA_INICIO = FormatoFecha.FechaEstandar(fechaHoraInicio),
                SINCRONIZACIONES_FECHA_HORA_FIN = FormatoFecha.FechaEstandar(fechaHoraFin),
                SINCRONIZACIONES_DISPOSITIVO_ORIGEN = idDispositivo,
                SINCRONIZACIONES_RESULTADO = "Error " + ex.Message,
            };
            await _sincronizacionRepo.AddAsync(sincronizacion);

            throw;
        }
    }
}
