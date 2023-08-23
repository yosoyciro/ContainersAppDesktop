using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Contracts.Services;
using ContainersDesktop.Logica.Services;

namespace ContainersDesktop.Logica.Services;
public class SincronizarServicio
{
    private readonly IDispositivosServicio _dispositivosServicio;
    private readonly IMovimientosServicio _movimientosServicio;
    private readonly ITareasProgramadasServicio _tareasProgramadasServicio;
    private readonly ISincronizacionServicio _sincronizacionServicio;
    private readonly AzureStorageManagement _azureStorageManagement;

    public SincronizarServicio(IMovimientosServicio movimientosServicio, ITareasProgramadasServicio tareasProgramadasServicio, ISincronizacionServicio sincronizacionServicio, AzureStorageManagement azureStorageManagement, IDispositivosServicio dispositivosServicio)
    {
        _movimientosServicio = movimientosServicio;
        _tareasProgramadasServicio = tareasProgramadasServicio;
        _sincronizacionServicio = sincronizacionServicio;
        _azureStorageManagement = azureStorageManagement;
        _dispositivosServicio = dispositivosServicio;
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
            var dispositivos = await _dispositivosServicio.ObtenerDispositivos();

            foreach (var item in dispositivos.Where(x => x.DISPOSITIVOS_ID_ESTADO_REG == "A" && !string.IsNullOrEmpty(x.DISPOSITIVOS_CONTAINER)))
            {
                idDispositivo = item.DISPOSITIVOS_ID_REG;
                fechaHoraInicio = DateTime.Now;

                //Bajo del container
                dbDescarga = await _azureStorageManagement.DownloadFile(item.DISPOSITIVOS_CONTAINER);

                if (dbDescarga != string.Empty)
                {
                    //TODO - Proceso e incorporo los movimientos
                    await _movimientosServicio.SincronizarMovimientos(dbDescarga, item.DISPOSITIVOS_ID_REG);
                    await _tareasProgramadasServicio.Sincronizar(dbDescarga, item.DISPOSITIVOS_ID_REG);

                    if (File.Exists(dbDescarga))
                    {
                        File.Delete(dbDescarga);
                    }
                }

                //Subo al contenedor
                await _azureStorageManagement.UploadFile(item.DISPOSITIVOS_CONTAINER);
                
                fechaHoraFin = DateTime.Now;

                //Grabo sincronizacion
                var sincronizacion = new Sincronizaciones()
                {
                    SINCRONIZACIONES_FECHA_HORA_INICIO = FormatoFecha.FechaEstandar(fechaHoraInicio),
                    SINCRONIZACIONES_FECHA_HORA_FIN = FormatoFecha.FechaEstandar(fechaHoraFin),
                    SINCRONIZACIONES_DISPOSITIVO_ORIGEN = item.DISPOSITIVOS_ID_REG,
                    SINCRONIZACIONES_RESULTADO = "Ok",
                };
                await _sincronizacionServicio.CrearSincronizacion(sincronizacion);
            }
        }
        catch (SystemException ex)
        {
            //Grabo sincronizacion
            var sincronizacion = new Sincronizaciones()
            {
                SINCRONIZACIONES_FECHA_HORA_INICIO = FormatoFecha.FechaEstandar(fechaHoraInicio),
                SINCRONIZACIONES_FECHA_HORA_FIN = FormatoFecha.FechaEstandar(fechaHoraFin),
                SINCRONIZACIONES_DISPOSITIVO_ORIGEN = idDispositivo,
                SINCRONIZACIONES_RESULTADO = "Error " + ex.Message,
            };
            await _sincronizacionServicio.CrearSincronizacion(sincronizacion);

            throw;
        }
    }
}
