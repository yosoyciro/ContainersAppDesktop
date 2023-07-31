using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Models.Storage;
using ContainersDesktop.Core.Persistencia;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Core.Services;
public class DispositivosServicio : IDispositivosServicio
{
    private readonly string _dbFile;
    private readonly string _dbFullPath;

    public DispositivosServicio(IOptions<Settings> settings)
    {
        _dbFile = Path.Combine(settings.Value.DBFolder, settings.Value.DBName);
        _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFile}";
    }

    public async Task<bool> ActualizarDispositivo(Dispositivos dispositivo)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                updateCommand.CommandText = "UPDATE DISPOSITIVOS SET DISPOSITIVOS_DESCRIP = @DISPOSITIVOS_DESCRIP,  DISPOSITIVOS_CONTAINER = @DISPOSITIVOS_CONTAINER, " +
                    "DISPOSITIVOS_FECHA_ACTUALIZACION = @DISPOSITIVOS_FECHA_ACTUALIZACION " +
                    "WHERE DISPOSITIVOS_ID_REG = @DISPOSITIVOS_ID_REG";

                updateCommand.Parameters.AddWithValue("@DISPOSITIVOS_DESCRIP", dispositivo.DISPOSITIVOS_DESCRIP);
                updateCommand.Parameters.AddWithValue("@DISPOSITIVOS_CONTAINER", dispositivo.DISPOSITIVOS_CONTAINER);
                updateCommand.Parameters.AddWithValue("@DISPOSITIVOS_FECHA_ACTUALIZACION", dispositivo.DISPOSITIVOS_FECHA_ACTUALIZACION);
                updateCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_REG", dispositivo.DISPOSITIVOS_ID_REG);


                await updateCommand.ExecuteReaderAsync();                

                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> BorrarRecuperarDispositivo(int id, string accion)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand deleteCommand = new SqliteCommand
                    ("UPDATE DISPOSITIVOS SET DISPOSITIVOS_ID_ESTADO_REG=@DISPOSITIVOS_ID_ESTADO_REG, DISPOSITIVOS_FECHA_ACTUALIZACION = @DISPOSITIVOS_FECHA_ACTUALIZACION " +
                    "WHERE DISPOSITIVOS_ID_REG = @DISPOSITIVOS_ID_REG", db);

                deleteCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_REG", id);
                deleteCommand.Parameters.AddWithValue("@DISPOSITIVOS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));
                deleteCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_ESTADO_REG", accion);

                SqliteDataReader query = await deleteCommand.ExecuteReaderAsync();

            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<int> CrearDispositivo(Dispositivos dispositivo)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO DISPOSITIVOS(DISPOSITIVOS_ID_ESTADO_REG, DISPOSITIVOS_DESCRIP, DISPOSITIVOS_CONTAINER, DISPOSITIVOS_FECHA_ACTUALIZACION)" +
                    "VALUES(@DISPOSITIVOS_ID_ESTADO_REG, @DISPOSITIVOS_DESCRIP, @DISPOSITIVOS_CONTAINER, @DISPOSITIVOS_FECHA_ACTUALIZACION)";

                insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_ESTADO_REG", dispositivo.DISPOSITIVOS_ID_ESTADO_REG);
                insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_DESCRIP", dispositivo.DISPOSITIVOS_DESCRIP);
                insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_CONTAINER", dispositivo.DISPOSITIVOS_CONTAINER);
                insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_FECHA_ACTUALIZACION", dispositivo.DISPOSITIVOS_FECHA_ACTUALIZACION);

                await insertCommand.ExecuteReaderAsync();
                var identity = await OperacionesComunes.GetIdentity(db);

                return identity;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Dispositivos>> ObtenerDispositivos()
    {
        List<Dispositivos> dispositivosList = new();

        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            await db.OpenAsync();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT DISPOSITIVOS_ID_REG, DISPOSITIVOS_ID_ESTADO_REG, DISPOSITIVOS_DESCRIP, DISPOSITIVOS_CONTAINER FROM DISPOSITIVOS", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                //if (query.GetString(1) == "A")
                //{
                    var dispositivoObjeto = new Dispositivos()
                    {
                        DISPOSITIVOS_ID_REG = query.GetInt32(0),
                        DISPOSITIVOS_ID_ESTADO_REG = query.GetString(1),
                        DISPOSITIVOS_DESCRIP = query.GetString(2),
                        DISPOSITIVOS_CONTAINER = query.GetString(3),
                    };
                    dispositivosList.Add(dispositivoObjeto);
                //}
            }

            selectCommand.Dispose();
            query.Close();
            await query.DisposeAsync();
            db.Close();
            db.Dispose();
            SqliteConnection.ClearAllPools();

            GC.Collect();
        }

        
        return dispositivosList;
    }

    public async Task<bool> ExisteContainer(string cloudContainer)
    {
        Dispositivos dispositivo = new();
        bool existe;

        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            await db.OpenAsync();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT DISPOSITIVOS_ID_REG FROM DISPOSITIVOS WHERE DISPOSITIVOS_CONTAINER=@DISPOSITIVOS_CONTAINER", db);

            selectCommand.Parameters.AddWithValue("@DISPOSITIVOS_CONTAINER", cloudContainer);
            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            existe = query.HasRows;

            selectCommand.Dispose();
            query.Close();
            await query.DisposeAsync();
            db.Close();
            db.Dispose();
            SqliteConnection.ClearAllPools();

            GC.Collect();
        }

        return existe;
    }
}
