using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Core.Persistencia;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class ListasServicio
{
    //private readonly string _dbFile = string.Empty;
    //private readonly string _dbFullPath = string.Empty;
    //private readonly ILogger<ListasServicio> _logger;

    //public ListasServicio(IOptions<Settings> settings, ILogger<ListasServicio> logger)
    //{
    //    _dbFile = Path.Combine(settings.Value.DBFolder, settings.Value.DBName);
    //    _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFile}";
    //    _logger = logger;
    //}

    //public async Task<int> CrearLista(Lista lista)
    //{
    //    using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //    {
    //        await db.OpenAsync();

    //        SqliteCommand insertCommand = new SqliteCommand();
    //        insertCommand.Connection = db;

    //        try
    //        {
    //            // Use parameterized query to prevent SQL injection attacks
    //            insertCommand.CommandText = "INSERT INTO LISTAS (LISTAS_ID_ESTADO_REG, LISTAS_ID_LISTA, LISTAS_ID_LISTA_ORDEN, LISTAS_ID_LISTA_DESCRIP, LISTAS_FECHA_ACTUALIZACION)" +
    //                "VALUES (@LISTAS_ID_ESTADO_REG, @LISTAS_ID_LISTA, @LISTAS_ID_LISTA_ORDEN, @LISTAS_ID_LISTA_DESCRIP, @LISTAS_FECHA_ACTUALIZACION);";
    //            insertCommand.Parameters.AddWithValue("@LISTAS_ID_ESTADO_REG", lista.LISTAS_ID_ESTADO_REG);
    //            insertCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA", lista.LISTAS_ID_LISTA);
    //            insertCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA_ORDEN", lista.LISTAS_ID_LISTA_ORDEN);
    //            insertCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA_DESCRIP", lista.LISTAS_ID_LISTA_DESCRIP);
    //            insertCommand.Parameters.AddWithValue("@LISTAS_FECHA_ACTUALIZACION", lista.LISTAS_FECHA_ACTUALIZACION);

    //            await insertCommand.ExecuteReaderAsync();

    //            var identity = await OperacionesComunes.GetIdentity(db);

    //            return identity;
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError("Error", ex.Message);
    //            throw;
    //        }
    //        finally
    //        {
    //            db.CloseAsync();
    //        }
    //    }        
    //}

    //public async Task<List<Lista>> ObtenerListas()
    //{
    //    List<Lista> listas = new List<Lista>();
        
    //    using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //    {
    //        try
    //        {
    //            await db.OpenAsync();

    //            SqliteCommand selectCommand = new SqliteCommand
    //                ("SELECT LISTAS_ID_REG, LISTAS_ID_ESTADO_REG, LISTAS_ID_LISTA, LISTAS_ID_LISTA_ORDEN, LISTAS_ID_LISTA_DESCRIP, LISTAS_FECHA_ACTUALIZACION FROM LISTAS", db);

    //            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

    //            while (query.Read())
    //            {
    //                //if (query.GetString(1) == "A")
    //                //{
    //                var nuevaLista = new Lista()
    //                {
    //                    ID = query.GetInt32(0),
    //                    LISTAS_ID_ESTADO_REG = query.GetString(1),
    //                    LISTAS_ID_LISTA = query.GetInt32(2),
    //                    LISTAS_ID_LISTA_ORDEN = query.GetInt32(3),
    //                    LISTAS_ID_LISTA_DESCRIP = query.GetString(4),
    //                    LISTAS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(query.GetString(5)),
    //                };
    //                listas.Add(nuevaLista);
    //                //}
    //            }

    //            return listas;
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError("Error", ex.Message);
    //            throw;
    //        }
    //        finally
    //        {
    //            await db.CloseAsync();
    //        }            
    //    }
        
        
    //}

    //public async Task<bool> ActualizarLista(Lista lista)
    //{
    //    try
    //    {
    //        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //        {
    //            db.Open();

    //            SqliteCommand updateCommand = new SqliteCommand();
    //            updateCommand.Connection = db;

    //            // Use parameterized query to prevent SQL injection attacks
    //            updateCommand.CommandText = "UPDATE LISTAS SET LISTAS_ID_LISTA = @LISTAS_ID_LISTA, LISTAS_ID_LISTA_ORDEN = @LISTAS_ID_LISTA_ORDEN, " +
    //                "LISTAS_ID_LISTA_DESCRIP = @LISTAS_ID_LISTA_DESCRIP, LISTAS_FECHA_ACTUALIZACION = @LISTAS_FECHA_ACTUALIZACION " +
    //                "WHERE LISTAS_ID_REG = @LISTAS_ID_REG";

    //            updateCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA", lista.LISTAS_ID_LISTA);
    //            updateCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA_ORDEN", lista.LISTAS_ID_LISTA_ORDEN);
    //            updateCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA_DESCRIP", lista.LISTAS_ID_LISTA_DESCRIP);
    //            updateCommand.Parameters.AddWithValue("@LISTAS_FECHA_ACTUALIZACION", lista.LISTAS_FECHA_ACTUALIZACION);
    //            updateCommand.Parameters.AddWithValue("@LISTAS_ID_REG", lista.ID);

    //            await updateCommand.ExecuteReaderAsync();

    //            return true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}

    //public async Task<bool> BorrarRecuperarLista(Lista lista)
    //{
    //    try
    //    {
    //        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
    //        {
    //            db.Open();

    //            SqliteCommand deleteCommand = new SqliteCommand
    //                ("UPDATE LISTAS SET LISTAS_ID_ESTADO_REG=@LISTAS_ID_ESTADO_REG, LISTAS_FECHA_ACTUALIZACION = @LISTAS_FECHA_ACTUALIZACION WHERE LISTAS_ID_REG = @LISTAS_ID_REG", db);

    //            deleteCommand.Parameters.AddWithValue("@LISTAS_ID_REG", lista.ID);
    //            deleteCommand.Parameters.AddWithValue("@LISTAS_FECHA_ACTUALIZACION", lista.LISTAS_FECHA_ACTUALIZACION);
    //            deleteCommand.Parameters.AddWithValue("@LISTAS_ID_ESTADO_REG", lista.LISTAS_ID_ESTADO_REG);

    //            SqliteDataReader query = await deleteCommand.ExecuteReaderAsync();

    //        }

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}
}
