using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace ContainersDesktop.Core.Services;
public class SincronizacionServicio : ISincronizacionServicio
{
    public async Task<bool> CrearSincronizacion(Sincronizaciones sincronizacion)
    {
        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath};Pooling=false"))
        {            
            await db.OpenAsync();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;
            try
            {
                

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO SINCRONIZACIONES(SINCRONIZACIONES_FECHA_HORA_INICIO, SINCRONIZACIONES_FECHA_HORA_FIN, SINCRONIZACIONES_DISPOSITIVO_ORIGEN, SINCRONIZACIONES_RESULTADO)" +
                    "VALUES(@SINCRONIZACIONES_FECHA_HORA_INICIO, @SINCRONIZACIONES_FECHA_HORA_FIN, @SINCRONIZACIONES_DISPOSITIVO_ORIGEN, @SINCRONIZACIONES_RESULTADO)";

                insertCommand.Parameters.AddWithValue("@SINCRONIZACIONES_FECHA_HORA_INICIO", sincronizacion.SINCRONIZACIONES_FECHA_HORA_INICIO);
                insertCommand.Parameters.AddWithValue("@SINCRONIZACIONES_FECHA_HORA_FIN", sincronizacion.SINCRONIZACIONES_FECHA_HORA_FIN);
                insertCommand.Parameters.AddWithValue("@SINCRONIZACIONES_DISPOSITIVO_ORIGEN", sincronizacion.SINCRONIZACIONES_DISPOSITIVO_ORIGEN);
                insertCommand.Parameters.AddWithValue("@SINCRONIZACIONES_RESULTADO", sincronizacion.SINCRONIZACIONES_RESULTADO);

                await insertCommand.ExecuteReaderAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await db.CloseAsync();
                await insertCommand.DisposeAsync();
                await db.DisposeAsync();
                //SqliteConnection.ClearAllPools();
                //GC.Collect();
            }
        }
        
    }

    public async Task<List<Sincronizaciones>> ObtenerSincronizaciones()
    {
        List<Sincronizaciones> sincronizacionList = new();

        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
        {
            await db.OpenAsync();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT SINCRONIZACIONES_ID_REG, SINCRONIZACIONES_FECHA_HORA_INICIO, SINCRONIZACIONES_FECHA_HORA_FIN, SINCRONIZACIONES_DISPOSITIVO_ORIGEN, SINCRONIZACIONES_RESULTADO " +
                "FROM SINCRONIZACIONES", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                //if (query.GetString(1) == "A")
                //{
                var sincronizacionObjeto = new Sincronizaciones()
                {
                    SINCRONIZACIONES_ID_REG = query.GetInt32(0),
                    SINCRONIZACIONES_FECHA_HORA_INICIO = query.GetString(1),
                    SINCRONIZACIONES_FECHA_HORA_FIN = query.GetString(2),
                    SINCRONIZACIONES_DISPOSITIVO_ORIGEN = query.GetInt32(3),
                    SINCRONIZACIONES_RESULTADO = query.GetString(4),
                };
                sincronizacionList.Add(sincronizacionObjeto);
                //}
            }
            
            await db.CloseAsync();
        }

        return sincronizacionList;
    }
}
