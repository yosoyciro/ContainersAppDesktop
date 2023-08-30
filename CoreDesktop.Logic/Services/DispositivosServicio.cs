using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Core.Persistencia;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class DispositivosServicio
{
    //private readonly string _dbFile;
    //private readonly string _dbFullPath;
    //private readonly ILogger<DispositivosServicio> _logger;

    //public DispositivosServicio(IOptions<Settings> settings, ILogger<DispositivosServicio> logger)
    //{
    //    _dbFile = Path.Combine(settings.Value.DBFolder, settings.Value.DBName);
    //    _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFile}";
    //    _logger = logger;
    //}

    //public async Task<bool> ActualizarDispositivo(Dispositivo dispositivo)
    //{
    //    using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //    {
    //        try
    //        {
    //            await db.OpenAsync();

    //            SqliteCommand updateCommand = new SqliteCommand();
    //            updateCommand.Connection = db;

    //            // Use parameterized query to prevent SQL injection attacks
    //            updateCommand.CommandText = "UPDATE DISPOSITIVOS SET DISPOSITIVOS_DESCRIP = @DISPOSITIVOS_DESCRIP,  DISPOSITIVOS_CONTAINER = @DISPOSITIVOS_CONTAINER, " +
    //                "DISPOSITIVOS_FECHA_ACTUALIZACION = @DISPOSITIVOS_FECHA_ACTUALIZACION " +
    //                "WHERE DISPOSITIVOS_ID_REG = @DISPOSITIVOS_ID_REG";

    //            updateCommand.Parameters.AddWithValue("@DISPOSITIVOS_DESCRIP", dispositivo.DISPOSITIVOS_DESCRIP);
    //            updateCommand.Parameters.AddWithValue("@DISPOSITIVOS_CONTAINER", dispositivo.DISPOSITIVOS_CONTAINER);
    //            updateCommand.Parameters.AddWithValue("@DISPOSITIVOS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));
    //            updateCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_REG", dispositivo.ID);


    //            await updateCommand.ExecuteReaderAsync();

    //            return true;
    //        }

    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex.Message);
    //            throw;
    //        }
    //        finally
    //        {
    //            await db.CloseAsync();
    //        }
    //    }
    //}

    //public async Task<bool> BorrarRecuperarDispositivo(int id, string accion)
    //{
    //    using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //    {
    //        try
    //        {
    //            await db.OpenAsync();

    //            SqliteCommand deleteCommand = new SqliteCommand
    //                ("UPDATE DISPOSITIVOS SET DISPOSITIVOS_ID_ESTADO_REG=@DISPOSITIVOS_ID_ESTADO_REG, DISPOSITIVOS_FECHA_ACTUALIZACION = @DISPOSITIVOS_FECHA_ACTUALIZACION " +
    //                "WHERE DISPOSITIVOS_ID_REG = @DISPOSITIVOS_ID_REG", db);

    //            deleteCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_REG", id);
    //            deleteCommand.Parameters.AddWithValue("@DISPOSITIVOS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));
    //            deleteCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_ESTADO_REG", accion);

    //            SqliteDataReader query = await deleteCommand.ExecuteReaderAsync();

    //            return true;
    //        }

    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex.Message);
    //            throw;
    //        }
    //        finally
    //        {
    //            await db.CloseAsync();
    //        }
    //    }
    //}

    //public async Task<int> CrearDispositivo(Dispositivo dispositivo)
    //{
    //    using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //    {
    //        try
    //        {
    //            await db.OpenAsync();

    //            SqliteCommand insertCommand = new SqliteCommand();
    //            insertCommand.Connection = db;

    //            // Use parameterized query to prevent SQL injection attacks
    //            insertCommand.CommandText = "INSERT INTO DISPOSITIVOS(DISPOSITIVOS_ID_ESTADO_REG, DISPOSITIVOS_DESCRIP, DISPOSITIVOS_CONTAINER, DISPOSITIVOS_FECHA_ACTUALIZACION)" +
    //                "VALUES(@DISPOSITIVOS_ID_ESTADO_REG, @DISPOSITIVOS_DESCRIP, @DISPOSITIVOS_CONTAINER, @DISPOSITIVOS_FECHA_ACTUALIZACION)";

    //            insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_ESTADO_REG", dispositivo.DISPOSITIVOS_ID_ESTADO_REG);
    //            insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_DESCRIP", dispositivo.DISPOSITIVOS_DESCRIP);
    //            insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_CONTAINER", dispositivo.DISPOSITIVOS_CONTAINER);
    //            insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));

    //            await insertCommand.ExecuteReaderAsync();
    //            var identity = await OperacionesComunes.GetIdentity(db);

    //            return identity;
    //        }

    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex.Message);
    //            throw;
    //        }
    //        finally
    //        {
    //            await db.CloseAsync();
    //        }
    //    }
    //}

    //public async Task<List<Dispositivo>> ObtenerDispositivos()
    //{
    //    List<Dispositivo> dispositivosList = new();


    //    using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //    {
    //        SqliteCommand selectCommand = new SqliteCommand
    //            ("SELECT DISPOSITIVOS_ID_REG, DISPOSITIVOS_ID_ESTADO_REG, DISPOSITIVOS_DESCRIP, DISPOSITIVOS_CONTAINER, DISPOSITIVOS_FECHA_ACTUALIZACION " +
    //            "FROM DISPOSITIVOS", db);

    //        try
    //        {
    //            await db.OpenAsync();

    //            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

    //            while (query.Read())
    //            {
    //                //if (query.GetString(1) == "A")
    //                //{
    //                var dispositivoObjeto = new Dispositivo()
    //                {
    //                    ID = query.GetInt32(0),
    //                    DISPOSITIVOS_ID_ESTADO_REG = query.GetString(1),
    //                    DISPOSITIVOS_DESCRIP = query.GetString(2),
    //                    DISPOSITIVOS_CONTAINER = query.GetString(3),
    //                    DISPOSITIVOS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(query.GetString(4)),
    //                };
    //                dispositivosList.Add(dispositivoObjeto);
    //                //}
    //            }

    //            return dispositivosList;
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError("Error en ObtenerDispositivos", ex);
    //            throw;
    //        }
    //        finally
    //        {
    //            await db.CloseAsync();
    //            await selectCommand.DisposeAsync();
    //            //query.Close();
    //            //await query.DisposeAsync();
    //            await db.DisposeAsync();
    //            SqliteConnection.ClearAllPools();

    //            GC.Collect();
    //        }
    //    }
    //}

    //public async Task<bool> ExisteContainer(string cloudContainer)
    //{
    //    Dispositivo dispositivo = new();
    //    bool existe;

    //    using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //    {
    //        await db.OpenAsync();

    //        SqliteCommand selectCommand = new SqliteCommand
    //            ("SELECT DISPOSITIVOS_ID_REG FROM DISPOSITIVOS WHERE DISPOSITIVOS_CONTAINER=@DISPOSITIVOS_CONTAINER", db);

    //        selectCommand.Parameters.AddWithValue("@DISPOSITIVOS_CONTAINER", cloudContainer);
    //        SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

    //        existe = query.HasRows;

    //        selectCommand.Dispose();
    //        query.Close();
    //        await query.DisposeAsync();
    //        db.Close();
    //        db.Dispose();
    //        SqliteConnection.ClearAllPools();

    //        GC.Collect();
    //    }

    //    return existe;
    //}
}
