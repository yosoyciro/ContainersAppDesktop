using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Models.Storage;
using ContainersDesktop.Core.Persistencia;
using ContainersDesktop.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Core.Services;
public class ListasServicio : IListasServicio
{
    private readonly string _dbFile = string.Empty;
    private readonly string _dbFullPath = string.Empty;
    private readonly ILogger<LoginViewModel> _logger;

    public ListasServicio(IOptions<Settings> settings, ILogger<LoginViewModel> logger)
    {
        _dbFile = Path.Combine(settings.Value.DBFolder, settings.Value.DBName);
        _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFile}";
        _logger = logger;
    }

    public async Task<int> CrearLista(Listas lista)
    {
        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            await db.OpenAsync();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            try
            {
                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO LISTAS (LISTAS_ID_ESTADO_REG, LISTAS_ID_LISTA, LISTAS_ID_LISTA_ORDEN, LISTAS_ID_LISTA_DESCRIP, LISTAS_FECHA_ACTUALIZACION)" +
                    "VALUES (@LISTAS_ID_ESTADO_REG, @LISTAS_ID_LISTA, @LISTAS_ID_LISTA_ORDEN, @LISTAS_ID_LISTA_DESCRIP, @LISTAS_FECHA_ACTUALIZACION);";
                insertCommand.Parameters.AddWithValue("@LISTAS_ID_ESTADO_REG", lista.LISTAS_ID_ESTADO_REG);
                insertCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA", lista.LISTAS_ID_LISTA);
                insertCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA_ORDEN", lista.LISTAS_ID_LISTA_ORDEN);
                insertCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA_DESCRIP", lista.LISTAS_ID_LISTA_DESCRIP);
                insertCommand.Parameters.AddWithValue("@LISTAS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));

                await insertCommand.ExecuteReaderAsync();

                var identity = await OperacionesComunes.GetIdentity(db);

                return identity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex.Message);
                throw;
            }
            finally
            {
                db.CloseAsync();
            }
        }        
    }

    public async Task<List<Listas>> ObtenerListas()
    {
        List<Listas> listas = new List<Listas>();
        
        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            try
            {
                await db.OpenAsync();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT LISTAS_ID_REG, LISTAS_ID_ESTADO_REG, LISTAS_ID_LISTA, LISTAS_ID_LISTA_ORDEN, LISTAS_ID_LISTA_DESCRIP, LISTAS_FECHA_ACTUALIZACION FROM LISTAS", db);

                SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

                while (query.Read())
                {
                    //if (query.GetString(1) == "A")
                    //{
                    var nuevaLista = new Listas()
                    {
                        LISTAS_ID_REG = query.GetInt32(0),
                        LISTAS_ID_ESTADO_REG = query.GetString(1),
                        LISTAS_ID_LISTA = query.GetInt32(2),
                        LISTAS_ID_LISTA_ORDEN = query.GetInt32(3),
                        LISTAS_ID_LISTA_DESCRIP = query.GetString(4),
                        LISTAS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(query.GetString(5)),
                    };
                    listas.Add(nuevaLista);
                    //}
                }

                return listas;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex.Message);
                throw;
            }
            finally
            {
                await db.CloseAsync();
            }            
        }
        
        
    }

    public async Task<bool> ActualizarLista(Listas lista)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                updateCommand.CommandText = "UPDATE LISTAS SET LISTAS_ID_LISTA = @LISTAS_ID_LISTA, LISTAS_ID_LISTA_ORDEN = @LISTAS_ID_LISTA_ORDEN, " +
                    "LISTAS_ID_LISTA_DESCRIP = @LISTAS_ID_LISTA_DESCRIP, LISTAS_FECHA_ACTUALIZACION = @LISTAS_FECHA_ACTUALIZACION " +
                    "WHERE LISTAS_ID_REG = @LISTAS_ID_REG";

                updateCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA", lista.LISTAS_ID_LISTA);
                updateCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA_ORDEN", lista.LISTAS_ID_LISTA_ORDEN);
                updateCommand.Parameters.AddWithValue("@LISTAS_ID_LISTA_DESCRIP", lista.LISTAS_ID_LISTA_DESCRIP);
                updateCommand.Parameters.AddWithValue("@LISTAS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));
                updateCommand.Parameters.AddWithValue("@LISTAS_ID_REG", lista.LISTAS_ID_REG);

                await updateCommand.ExecuteReaderAsync();

                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> BorrarLista(int id)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand deleteCommand = new SqliteCommand
                    ("UPDATE LISTAS SET LISTAS_ID_ESTADO_REG = 'B', LISTAS_FECHA_ACTUALIZACION = @CLALIST_FECHA_ACTUALIZACION WHERE LISTAS_ID_REG = @LISTAS_ID_REG", db);

                deleteCommand.Parameters.AddWithValue("@LISTAS_ID_REG", id);
                deleteCommand.Parameters.AddWithValue("@CLALIST_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));

                SqliteDataReader query = await deleteCommand.ExecuteReaderAsync();

            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
