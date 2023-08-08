using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models.Storage;
using ContainersDesktop.Core.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using ContainersDesktop.Core.Persistencia;

namespace ContainersDesktop.Core.Services;
public class TareasProgramadasServicio : ITareasProgramadasServicio
{
    private readonly string _dbFile;
    private readonly string _dbFullPath;

    public TareasProgramadasServicio(IOptions<Settings> settings)
    {
        _dbFile = Path.Combine(settings.Value.DBFolder, settings.Value.DBName);
        _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFile}";
    }

    #region ObtenerPorObjeto
    public async Task<List<TareaProgramada>> ObtenerPorObjeto(int idObjeto)
    {
        List<TareaProgramada> movimLista = new();

        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT TAREAS_PROGRAMADAS_ID_REG, TAREAS_PROGRAMADAS_OBJETO_ID_REG, TAREAS_PROGRAMADAS_FECHA_PROGRAMADA, TAREAS_PROGRAMADAS_FECHA_COMPLETADA, " +
                "TAREAS_PROGRAMADAS_UBICACION_ORIGEN, TAREAS_PROGRAMADAS_UBICACION_DESTINO, TAREAS_PROGRAMADAS_ORDENADO, TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG, " +
                "TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION FROM TAREAS_PROGRAMADAS WHERE TAREAS_PROGRAMADAS_OBJETO_ID_REG = @TAREAS_PROGRAMADAS_OBJETO_ID_REG", db);

            selectCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_OBJETO_ID_REG", idObjeto);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                var movimObjeto = new TareaProgramada()
                {
                    TAREAS_PROGRAMADAS_ID_REG = query.GetInt32(0),
                    TAREAS_PROGRAMADAS_OBJETO_ID_REG = query.GetInt32(1),
                    TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaHora(query.GetString(2)),
                    TAREAS_PROGRAMADAS_FECHA_COMPLETADA = FormatoFecha.ConvertirAFechaHora(query.GetString(3)),
                    TAREAS_PROGRAMADAS_UBICACION_ORIGEN = query.GetInt32(4),
                    TAREAS_PROGRAMADAS_UBICACION_DESTINO = query.GetInt32(5),
                    TAREAS_PROGRAMADAS_ORDENADO = query.GetString(6),
                    TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = query.GetInt32(7),
                    TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(query.GetString(8)),
                };
                movimLista.Add(movimObjeto);
            }
        }

        return movimLista;
    }
    #endregion

    #region ObtenerTodos
    public async Task<List<TareaProgramada>> ObtenerTodos()
    {
        List<TareaProgramada> movimLista = new();
        
        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT TAREAS_PROGRAMADAS_ID_REG, TAREAS_PROGRAMADAS_OBJETO_ID_REG, TAREAS_PROGRAMADAS_FECHA_PROGRAMADA, TAREAS_PROGRAMADAS_FECHA_COMPLETADA, " +
                "TAREAS_PROGRAMADAS_UBICACION_ORIGEN, TAREAS_PROGRAMADAS_UBICACION_DESTINO, TAREAS_PROGRAMADAS_ORDENADO, TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG, " +
                "TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION, TAREAS_PROGRAMADAS_ID_ESTADO_REG FROM TAREAS_PROGRAMADAS", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                var movimObjeto = new TareaProgramada()
                {
                    TAREAS_PROGRAMADAS_ID_REG = query.GetInt32(0),
                    TAREAS_PROGRAMADAS_OBJETO_ID_REG = query.GetInt32(1),
                    TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaHora(query.GetString(2)),
                    TAREAS_PROGRAMADAS_FECHA_COMPLETADA = query.GetString(3) != string.Empty ? FormatoFecha.ConvertirAFechaHora(query.GetString(3)) : string.Empty,
                    TAREAS_PROGRAMADAS_UBICACION_ORIGEN = query.GetInt32(4),
                    TAREAS_PROGRAMADAS_UBICACION_DESTINO = query.GetInt32(5),
                    TAREAS_PROGRAMADAS_ORDENADO = query.GetString(6),
                    TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = query.GetInt32(7),
                    TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(query.GetString(8)),
                    TAREAS_PROGRAMADAS_ID_ESTADO_REG = query.GetString(9),
                };
                movimLista.Add(movimObjeto);
            }
        }

        return movimLista;
    }
    #endregion

    #region Sincronizar
    public async Task<bool> Sincronizar(string dbDescarga, int idDispositivo)
    {
        List<TareaProgramada> tareaProgramadasLista = new();
        using (SqliteConnection descargaDb = new SqliteConnection($"Filename={dbDescarga};Pooling=false"))
        {
            try
            {
                await descargaDb.OpenAsync();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT TAREAS_PROGRAMADAS_ID_REG, TAREAS_PROGRAMADAS_OBJETO_ID_REG, TAREAS_PROGRAMADAS_FECHA_PROGRAMADA, TAREAS_PROGRAMADAS_FECHA_COMPLETADA, " +
                    "TAREAS_PROGRAMADAS_UBICACION_ORIGEN, TAREAS_PROGRAMADAS_UBICACION_DESTINO, TAREAS_PROGRAMADAS_ORDENADO, TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG, " +
                    "TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD, TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD " +
                    "FROM TAREAS_PROGRAMADAS WHERE TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = @TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG", descargaDb);
                selectCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG", idDispositivo);
                SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

                while (query.Read())
                {
                    if (!string.IsNullOrEmpty(query.GetString(3)))
                    {
                        var tareaProgramadaObjeto = new TareaProgramada()
                        {
                            TAREAS_PROGRAMADAS_ID_REG = query.GetInt32(0),
                            TAREAS_PROGRAMADAS_OBJETO_ID_REG = query.GetInt32(1),
                            TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = query.GetString(2),
                            TAREAS_PROGRAMADAS_FECHA_COMPLETADA = query.GetString(3),
                            TAREAS_PROGRAMADAS_UBICACION_ORIGEN = query.GetInt32(4),
                            TAREAS_PROGRAMADAS_UBICACION_DESTINO = query.GetInt32(5),
                            TAREAS_PROGRAMADAS_ORDENADO = query.GetString(6),
                            TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = query.GetInt32(7),
                            TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = query.GetDouble(8),
                            TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = query.GetDouble(9),
                        };

                        tareaProgramadasLista.Add(tareaProgramadaObjeto);
                    }
                }

                await query.CloseAsync();
                await selectCommand.DisposeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await descargaDb.CloseAsync();
                await descargaDb.DisposeAsync();
            }
        }

        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            try
            {
                await db.OpenAsync();

                var updateCommand = "UPDATE TAREAS_PROGRAMADAS SET " +
                    "TAREAS_PROGRAMADAS_FECHA_COMPLETADA=@TAREAS_PROGRAMADAS_FECHA_COMPLETADA, " +
                    "TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD=@TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD, " +
                    "TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD=@TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD, " +
                    "TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION=@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION " +
                    "WHERE TAREAS_PROGRAMADAS_ID_REG=@TAREAS_PROGRAMADAS_ID_REG;";

                foreach (TareaProgramada tareaProgramada in tareaProgramadasLista)
                {
                    //Verifico si el registro existe
                    SqliteCommand selectCommand = new SqliteCommand("SELECT TAREAS_PROGRAMADAS_ID_REG FROM " +
                        " TAREAS_PROGRAMADAS WHERE TAREAS_PROGRAMADAS_ID_REG = @TAREAS_PROGRAMADAS_ID_REG AND TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = @TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG", db);
                    selectCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG);
                    selectCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG);
                    SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

                    if (query.HasRows)
                    {
                        using (var cmd = new SqliteCommand(updateCommand, db))
                        {
                            cmd.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_COMPLETADA", tareaProgramada.TAREAS_PROGRAMADAS_FECHA_COMPLETADA);
                            cmd.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD);
                            cmd.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD);
                            cmd.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_ID_REG);
                            cmd.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

                await db.CloseAsync();
            }
        }
    }
    #endregion    

    #region Agregar
    public async Task<int> Agregar(TareaProgramada tareaProgramada)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO TAREAS_PROGRAMADAS(TAREAS_PROGRAMADAS_OBJETO_ID_REG, TAREAS_PROGRAMADAS_FECHA_PROGRAMADA, TAREAS_PROGRAMADAS_FECHA_COMPLETADA, " +
                    "TAREAS_PROGRAMADAS_UBICACION_ORIGEN, TAREAS_PROGRAMADAS_UBICACION_DESTINO, TAREAS_PROGRAMADAS_ORDENADO, TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG, " +
                    "TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION, TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD, TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD, TAREAS_PROGRAMADAS_ID_ESTADO_REG) " +
                    "VALUES (@TAREAS_PROGRAMADAS_OBJETO_ID_REG, @TAREAS_PROGRAMADAS_FECHA_PROGRAMADA, @TAREAS_PROGRAMADAS_FECHA_COMPLETADA, " +
                    "@TAREAS_PROGRAMADAS_UBICACION_ORIGEN, @TAREAS_PROGRAMADAS_UBICACION_DESTINO, @TAREAS_PROGRAMADAS_ORDENADO, @TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG, " +
                    "@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION, @TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD, @TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD, @TAREAS_PROGRAMADAS_ID_ESTADO_REG);";
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_OBJETO_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_OBJETO_ID_REG);
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_PROGRAMADA", tareaProgramada.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA);
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_COMPLETADA", tareaProgramada.TAREAS_PROGRAMADAS_FECHA_COMPLETADA);
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_UBICACION_ORIGEN", tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_ORIGEN);
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_UBICACION_DESTINO", tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_DESTINO);
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ORDENADO", tareaProgramada.TAREAS_PROGRAMADAS_ORDENADO);
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG);
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD);
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD);
                insertCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ID_ESTADO_REG", "A");

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
    #endregion

    #region Modificar
    public async Task<bool> Modificar(TareaProgramada tareaProgramada)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                await db.OpenAsync();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                updateCommand.CommandText = "UPDATE TAREAS_PROGRAMADAS SET TAREAS_PROGRAMADAS_OBJETO_ID_REG=@TAREAS_PROGRAMADAS_OBJETO_ID_REG, TAREAS_PROGRAMADAS_FECHA_PROGRAMADA=@TAREAS_PROGRAMADAS_FECHA_PROGRAMADA, " +
                    "TAREAS_PROGRAMADAS_FECHA_COMPLETADA=@TAREAS_PROGRAMADAS_FECHA_COMPLETADA, TAREAS_PROGRAMADAS_UBICACION_ORIGEN=@TAREAS_PROGRAMADAS_UBICACION_ORIGEN, " +
                    "TAREAS_PROGRAMADAS_UBICACION_DESTINO=@TAREAS_PROGRAMADAS_UBICACION_DESTINO, TAREAS_PROGRAMADAS_ORDENADO=@TAREAS_PROGRAMADAS_ORDENADO, TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG=@TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG, " +
                    "TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION=@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION, TAREAS_PROGRAMADAS_ID_ESTADO_REG=@TAREAS_PROGRAMADAS_ID_ESTADO_REG " +
                    "WHERE TAREAS_PROGRAMADAS_ID_REG=@TAREAS_PROGRAMADAS_ID_REG;";
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_ID_REG);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_OBJETO_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_OBJETO_ID_REG);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_PROGRAMADA", tareaProgramada.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_COMPLETADA", tareaProgramada.TAREAS_PROGRAMADAS_FECHA_COMPLETADA);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_UBICACION_ORIGEN", tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_ORIGEN);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_UBICACION_DESTINO", tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_DESTINO);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ORDENADO", tareaProgramada.TAREAS_PROGRAMADAS_ORDENADO);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ID_ESTADO_REG", "A");

                await updateCommand.ExecuteReaderAsync();

                await db.CloseAsync();
                await db.DisposeAsync();

                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion

    #region Borrar
    public async Task<bool> Borrar(int id)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                await db.OpenAsync();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                updateCommand.CommandText = "UPDATE TAREAS_PROGRAMADAS SET TAREAS_PROGRAMADAS_ID_ESTADO_REG=@TAREAS_PROGRAMADAS_ID_ESTADO_REG, " +
                    " TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION=@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION " +
                    " WHERE TAREAS_PROGRAMADAS_ID_REG=@TAREAS_PROGRAMADAS_ID_REG;";
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ID_REG", id);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ID_ESTADO_REG", "B");
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));

                await updateCommand.ExecuteReaderAsync();

                await db.CloseAsync();
                await db.DisposeAsync();


                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion
}