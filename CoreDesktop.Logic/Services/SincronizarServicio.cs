using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Storage;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using CoreDesktop.Logica.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Logica.Services;
public class SincronizarServicio
{
    private readonly IAsyncRepository<Dispositivo> _dispositivosRepo;
    private readonly IAsyncRepository<Sincronizacion> _sincronizacionRepo;
    private readonly AzureStorageManagement _azureStorageManagement;
    private readonly MensajesServicio _mensajesServicio;
    private readonly ILogger<SincronizarServicio> _logger;
    private readonly string _dbName = string.Empty;
    private readonly string _dbNameSubida = string.Empty;
    private readonly string _dbFolder = string.Empty;
    private readonly string _dbFullPath = string.Empty;
    

    public SincronizarServicio(AzureStorageManagement azureStorageManagement, IAsyncRepository<Dispositivo> dispositivosRepo, IAsyncRepository<Sincronizacion> sincronizacionRepo, MensajesServicio mensajesServicio, ILogger<SincronizarServicio> logger, IOptions<Settings> settings)
    {
        _azureStorageManagement = azureStorageManagement;
        _dispositivosRepo = dispositivosRepo;
        _sincronizacionRepo = sincronizacionRepo;
        _mensajesServicio = mensajesServicio;
        _dbFolder = settings.Value.DBFolder;
        _dbName = settings.Value.DBName;
        _dbNameSubida = settings.Value.DBNameSubida;
        _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFolder}\\{_dbName}";
        _logger = logger;
    }
    public async Task Sincronizar()
    {
        var dbDescarga = string.Empty;
        var fechaHoraInicio = DateTime.Now;
        var fechaHoraFin = DateTime.Now;
        var idDispositivo = 0;
        var dbSubidaFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFolder}\\{_dbNameSubida}";

        //Subo Base
        try
        {
            //Preparo la base a subir            
            File.Copy(_dbFullPath, dbSubidaFullPath);

            var dispositivos = await _dispositivosRepo.GetAsync();

            foreach (var item in dispositivos.Where(x => x.Estado == "A" && !string.IsNullOrEmpty(x.DISPOSITIVOS_CONTAINER)))
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
                await _azureStorageManagement.UploadFile(item.DISPOSITIVOS_CONTAINER!, _dbNameSubida, dbSubidaFullPath);
                
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

            // Log
            _logger.LogError(ex.Message);

            throw;
        }
        finally
        {
            File.Delete(dbSubidaFullPath);
        }
    }
}
