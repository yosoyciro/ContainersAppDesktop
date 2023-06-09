using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using Microsoft.Data.Sqlite;
using Windows.Media.Playlists;
using Windows.Storage;

namespace ContainersDesktop.Core.Services;
public class DispositivosServicio : IDispositivosServicio
{
    public async Task<bool> ActualizarDispositivo(Dispositivos dispositivo)
    {
        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");

        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
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
                updateCommand.Parameters.AddWithValue("@DISPOSITIVOS_FECHA_ACTUALIZACION", DateTime.Now.ToShortDateString());
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

    public async Task<bool> BorrarDispositivo(int id)
    {
        try
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand deleteCommand = new SqliteCommand
                    ("UPDATE DISPOSITIVOS SET DISPOSITIVOS_ID_ESTADO_REG = 'B', DISPOSITIVOS_FECHA_ACTUALIZACION = @DISPOSITIVOS_FECHA_ACTUALIZACION "+
                    "WHERE DISPOSITIVOS_ID_REG = @DISPOSITIVOS_ID_REG", db);

                deleteCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_REG", id);
                deleteCommand.Parameters.AddWithValue("@DISPOSITIVOS_FECHA_ACTUALIZACION", DateTime.Now.ToShortDateString());

                SqliteDataReader query = await deleteCommand.ExecuteReaderAsync();

            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> CrearDispositivo(Dispositivos dispositivo)
    {
        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");

        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO DISPOSITIVOS(DISPOSITIVOS_ID_ESTADO_REG, DISPOSITIVOS_DESCRIP, DISPOSITIVOS_CONTAINER, DISPOSITIVOS_FECHA_ACTUALIZACION)" +
                    "VALUES(@DISPOSITIVOS_ID_ESTADO_REG, @DISPOSITIVOS_DESCRIP, @DISPOSITIVOS_CONTAINER, @DISPOSITIVOS_FECHA_ACTUALIZACION)";

                insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_ID_ESTADO_REG", "A");
                insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_DESCRIP", dispositivo.DISPOSITIVOS_DESCRIP);
                insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_CONTAINER", dispositivo.DISPOSITIVOS_CONTAINER);
                insertCommand.Parameters.AddWithValue("@DISPOSITIVOS_FECHA_ACTUALIZACION", DateTime.Now.ToShortDateString());

                await insertCommand.ExecuteReaderAsync();

                return true;
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

        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
        {
            await db.OpenAsync();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT DISPOSITIVOS_ID_REG, DISPOSITIVOS_ID_ESTADO_REG, DISPOSITIVOS_DESCRIP, DISPOSITIVOS_CONTAINER FROM DISPOSITIVOS", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                if (query.GetString(1) == "A")
                {
                    var dispositivoObjeto = new Dispositivos()
                    {
                        DISPOSITIVOS_ID_REG = query.GetInt32(0),
                        DISPOSITIVOS_ID_ESTADO_REG = query.GetString(1),
                        DISPOSITIVOS_DESCRIP = query.GetString(2),
                        DISPOSITIVOS_CONTAINER = query.GetString(3),
                    };
                    dispositivosList.Add(dispositivoObjeto);
                }
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
}
