using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class SincronizacionServicio
{
    //private readonly string _dbFile;
    //private readonly string _dbFullPath;
    //private readonly ILogger<SincronizacionServicio> _logger;

    //public SincronizacionServicio(IOptions<Settings> settings, ILogger<SincronizacionServicio> logger)
    //{
    //    _dbFile = Path.Combine(settings.Value.DBFolder, settings.Value.DBName);
    //    _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFile}";
    //    _logger = logger;
    //}

    //public async Task<bool> CrearSincronizacion(Sincronizacion sincronizacion)
    //{
    //    using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath};Pooling=false"))
    //    {            
    //        await db.OpenAsync();

    //        SqliteCommand insertCommand = new SqliteCommand();
    //        insertCommand.Connection = db;
    //        try
    //        {
    //            insertCommand.CommandText = "INSERT INTO SINCRONIZACIONES(SINCRONIZACIONES_FECHA_HORA_INICIO, SINCRONIZACIONES_FECHA_HORA_FIN, SINCRONIZACIONES_DISPOSITIVO_ORIGEN, SINCRONIZACIONES_RESULTADO)" +
    //                "VALUES(@SINCRONIZACIONES_FECHA_HORA_INICIO, @SINCRONIZACIONES_FECHA_HORA_FIN, @SINCRONIZACIONES_DISPOSITIVO_ORIGEN, @SINCRONIZACIONES_RESULTADO)";

    //            insertCommand.Parameters.AddWithValue("@SINCRONIZACIONES_FECHA_HORA_INICIO", sincronizacion.SINCRONIZACIONES_FECHA_HORA_INICIO);
    //            insertCommand.Parameters.AddWithValue("@SINCRONIZACIONES_FECHA_HORA_FIN", sincronizacion.SINCRONIZACIONES_FECHA_HORA_FIN);
    //            insertCommand.Parameters.AddWithValue("@SINCRONIZACIONES_DISPOSITIVO_ORIGEN", sincronizacion.SINCRONIZACIONES_DISPOSITIVO_ORIGEN);
    //            insertCommand.Parameters.AddWithValue("@SINCRONIZACIONES_RESULTADO", sincronizacion.SINCRONIZACIONES_RESULTADO);

    //            await insertCommand.ExecuteReaderAsync();

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError("Error", ex.Message);
    //            throw;
    //        }
    //        finally
    //        {
    //            await db.CloseAsync();
    //            await insertCommand.DisposeAsync();
    //            await db.DisposeAsync();

    //            SqliteConnection.ClearAllPools();
    //        }
    //    }
    //}

    //public async Task<List<Sincronizacion>> ObtenerSincronizaciones()
    //{
    //    List<Sincronizacion> sincronizacionList = new();

    //    using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //    {
    //        await db.OpenAsync();

    //        SqliteCommand selectCommand = new SqliteCommand
    //            ("SELECT SINCRONIZACIONES_ID_REG, SINCRONIZACIONES_FECHA_HORA_INICIO, SINCRONIZACIONES_FECHA_HORA_FIN, SINCRONIZACIONES_DISPOSITIVO_ORIGEN, SINCRONIZACIONES_RESULTADO " +
    //            "FROM SINCRONIZACIONES", db);

    //        SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

    //        while (query.Read())
    //        {
    //            //if (query.GetString(1) == "A")
    //            //{
    //            var sincronizacionObjeto = new Sincronizacion()
    //            {
    //                ID = query.GetInt32(0),
    //                SINCRONIZACIONES_FECHA_HORA_INICIO = FormatoFecha.ConvertirAFechaHora(query.GetString(1)),
    //                SINCRONIZACIONES_FECHA_HORA_FIN = FormatoFecha.ConvertirAFechaHora(query.GetString(2)),
    //                SINCRONIZACIONES_DISPOSITIVO_ORIGEN = query.GetInt32(3),
    //                SINCRONIZACIONES_RESULTADO = query.GetString(4),
    //            };
    //            sincronizacionList.Add(sincronizacionObjeto);
    //            //}
    //        }
            
    //        await db.CloseAsync();
    //    }

    //    return sincronizacionList;
    //}
}
