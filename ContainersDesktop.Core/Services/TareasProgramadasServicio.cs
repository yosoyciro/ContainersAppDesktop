using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models.Storage;
using ContainersDesktop.Core.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Core.Services;
public class TareasProgramadasServicio : ITareasProgramadasServicio
{
    private readonly string _dbFile;

    public TareasProgramadasServicio(IOptions<Settings> settings)
    {
        _dbFile = Path.Combine(settings.Value.DBPath, settings.Value.DBName);
    }

    #region ObtenerPorObjeto
    public async Task<List<TareaProgramada>> ObtenerPorObjeto(int idObjeto)
    {
        List<TareaProgramada> movimLista = new();

        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFile}"))
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
                    TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaCorta(query.GetString(2)),
                    TAREAS_PROGRAMADAS_FECHA_COMPLETADA = FormatoFecha.ConvertirAFechaCorta(query.GetString(3)),
                    TAREAS_PROGRAMADAS_UBICACION_ORIGEN = query.GetInt32(4),
                    TAREAS_PROGRAMADAS_UBICACION_DESTINO = query.GetInt32(5),
                    TAREAS_PROGRAMADAS_ORDENADO = query.GetString(6),
                    TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = query.GetInt32(7),
                    TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaCorta(query.GetString(8)),
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
        
        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFile}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT TAREAS_PROGRAMADAS_ID_REG, TAREAS_PROGRAMADAS_OBJETO_ID_REG, TAREAS_PROGRAMADAS_FECHA_PROGRAMADA, TAREAS_PROGRAMADAS_FECHA_COMPLETADA, " +
                "TAREAS_PROGRAMADAS_UBICACION_ORIGEN, TAREAS_PROGRAMADAS_UBICACION_DESTINO, TAREAS_PROGRAMADAS_ORDENADO, TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG, " +
                "TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION FROM TAREAS_PROGRAMADAS", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                var movimObjeto = new TareaProgramada()
                {
                    TAREAS_PROGRAMADAS_ID_REG = query.GetInt32(0),
                    TAREAS_PROGRAMADAS_OBJETO_ID_REG = query.GetInt32(1),
                    TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaCorta(query.GetString(2)),
                    TAREAS_PROGRAMADAS_FECHA_COMPLETADA = query.GetString(3) != string.Empty ? FormatoFecha.ConvertirAFechaCorta(query.GetString(3)) : string.Empty,
                    TAREAS_PROGRAMADAS_UBICACION_ORIGEN = query.GetInt32(4),
                    TAREAS_PROGRAMADAS_UBICACION_DESTINO = query.GetInt32(5),
                    TAREAS_PROGRAMADAS_ORDENADO = query.GetString(6),
                    TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = query.GetInt32(7),
                    TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaCorta(query.GetString(8)),
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
                    "TAREAS_PROGRAMADAS_UBICACION_ORIGEN, TAREAS_PROGRAMADAS_UBICACION_DESTINO, TAREAS_PROGRAMADAS_ORDENADO, TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG " +
                    "FROM TAREAS_PROGRAMADAS", descargaDb);

                SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

                while (query.Read())
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
                    };

                    tareaProgramadasLista.Add(tareaProgramadaObjeto);
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

        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFile}"))
        {
            try
            {
                db.Open();

                // Use parameterized query to prevent SQL injection attacks
                var insertCommand = "UPDATE TAREAS_PROGRAMADAS SET TAREAS_PROGRAMADAS_ID_REG, TAREAS_PROGRAMADAS_OBJETO_ID_REG, TAREAS_PROGRAMADAS_FECHA_PROGRAMADA, TAREAS_PROGRAMADAS_FECHA_COMPLETADA, " +
                    "TAREAS_PROGRAMADAS_UBICACION_ORIGEN, TAREAS_PROGRAMADAS_UBICACION_DESTINO, TAREAS_PROGRAMADAS_ORDENADO, TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG) " +
                    "VALUES (@TAREAS_PROGRAMADAS_ID_REG, @TAREAS_PROGRAMADAS_OBJETO_ID_REG, @TAREAS_PROGRAMADAS_OBJETO_ID_REG, @TAREAS_PROGRAMADAS_FECHA_PROGRAMADA, @TAREAS_PROGRAMADAS_FECHA_COMPLETADA, " +
                    "@TAREAS_PROGRAMADAS_UBICACION_ORIGEN, @TAREAS_PROGRAMADAS_UBICACION_DESTINO, @TAREAS_PROGRAMADAS_ORDENADO, @TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG);";

                foreach (TareaProgramada tareaProgramada in tareaProgramadasLista)
                {
                    //Verifico si el registro existe
                    SqliteCommand selectCommand = new SqliteCommand("SELECT TAREAS_PROGRAMADAS_ID_REG FROM " +
                        " TAREAS_PROGRAMADAS WHERE MOVIM_ID_REG_MOBILE = @MOVIM_ID_REG_MOBILE AND MOVIM_ID_DISPOSITIVO = @MOVIM_ID_DISPOSITIVO", db);
                    //selectCommand.Parameters.AddWithValue("@MOVIM_ID_REG_MOBILE", movim.MOVIM_ID_REG);
                    selectCommand.Parameters.AddWithValue("@MOVIM_ID_DISPOSITIVO", idDispositivo);
                    SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

                    //if (!query.HasRows)
                    //{
                    //    using (var cmd = new SqliteCommand(insertCommand, db))
                    //    {
                    //        cmd.Parameters.AddWithValue("@MOVIM_ID_REG_MOBILE", movim.MOVIM_ID_REG);
                    //        cmd.Parameters.AddWithValue("@MOVIM_ID_ESTADO_REG", movim.MOVIM_ID_ESTADO_REG);
                    //        cmd.Parameters.AddWithValue("@MOVIM_ID_DISPOSITIVO", idDispositivo);
                    //        cmd.Parameters.AddWithValue("@MOVIM_FECHA", movim.MOVIM_FECHA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_ID_OBJETO", movim.MOVIM_ID_OBJETO);
                    //        cmd.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM_LISTA", movim.MOVIM_TIPO_MOVIM_LISTA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM", movim.MOVIM_TIPO_MOVIM);
                    //        cmd.Parameters.AddWithValue("@MOVIM_PESO_LISTA", movim.MOVIM_PESO_LISTA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_PESO", movim.MOVIM_PESO);
                    //        cmd.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA_LISTA", movim.MOVIM_TRANSPORTISTA_LISTA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA", movim.MOVIM_TRANSPORTISTA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_CLIENTE_LISTA", movim.MOVIM_CLIENTE_LISTA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_CLIENTE", movim.MOVIM_CLIENTE);
                    //        cmd.Parameters.AddWithValue("@MOVIM_CHOFER_LISTA", movim.MOVIM_CHOFER_LISTA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_CHOFER", movim.MOVIM_CHOFER);
                    //        cmd.Parameters.AddWithValue("@MOVIM_CAMION_ID", movim.MOVIM_CAMION_ID);
                    //        cmd.Parameters.AddWithValue("@MOVIM_ALBARAN_ID", movim.MOVIM_ALBARAN_ID);
                    //        cmd.Parameters.AddWithValue("@MOVIM_REMOLQUE_ID", movim.MOVIM_REMOLQUE_ID);
                    //        cmd.Parameters.AddWithValue("@MOVIM_OBSERVACIONES", movim.MOVIM_OBSERVACIONES);
                    //        cmd.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA_LISTA", movim.MOVIM_ENTRADA_SALIDA_LISTA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA", movim.MOVIM_ENTRADA_SALIDA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_ALMACEN_LISTA", movim.MOVIM_ALMACEN_LISTA);
                    //        cmd.Parameters.AddWithValue("@MOVIM_ALMACEN", movim.MOVIM_ALMACEN);
                    //        cmd.Parameters.AddWithValue("@MOVIM_PDF", movim.MOVIM_PDF);
                    //        cmd.Parameters.AddWithValue("@MOVIM_FECHA_ACTUALIZACION", movim.MOVIM_FECHA_ACTUALIZACION);
                    //        await cmd.ExecuteNonQueryAsync();
                    //    }
                    //}
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

    public async Task<bool> ActualizarObjeto(Objetos objeto)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFile}"))
            {
                await db.OpenAsync();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                updateCommand.CommandText = "UPDATE OBJETOS SET OBJ_MATRICULA = @OBJ_MATRICULA, OBJ_ID_ESTADO_REG = @OBJ_ID_ESTADO_REG, " +
                    "OBJ_SIGLAS_LISTA = @OBJ_SIGLAS_LISTA, OBJ_SIGLAS = @OBJ_SIGLAS, OBJ_MODELO_LISTA = @OBJ_MODELO_LISTA, OBJ_MODELO = @OBJ_MODELO, " +
                    "OBJ_ID_OBJETO = @OBJ_ID_OBJETO, OBJ_VARIANTE_LISTA = @OBJ_VARIANTE_LISTA, OBJ_VARIANTE = @OBJ_VARIANTE, OBJ_TIPO_LISTA = @OBJ_TIPO_LISTA, OBJ_TIPO = @OBJ_TIPO, " +
                    "OBJ_INSPEC_CSC = @OBJ_INSPEC_CSC, OBJ_PROPIETARIO_LISTA = @OBJ_PROPIETARIO_LISTA, OBJ_PROPIETARIO = @OBJ_PROPIETARIO, OBJ_TARA_LISTA = @OBJ_TARA_LISTA, OBJ_TARA = @OBJ_TARA, " +
                    "OBJ_PMP_LISTA = @OBJ_PMP_LISTA, OBJ_PMP = @OBJ_PMP, OBJ_CARGA_UTIL = @OBJ_CARGA_UTIL, OBJ_ALTURA_EXTERIOR_LISTA = @OBJ_ALTURA_EXTERIOR_LISTA, OBJ_ALTURA_EXTERIOR = @OBJ_ALTURA_EXTERIOR, " +
                    "OBJ_CUELLO_CISNE_LISTA = @OBJ_CUELLO_CISNE_LISTA, OBJ_CUELLO_CISNE = @OBJ_CUELLO_CISNE, OBJ_BARRAS_LISTA = @OBJ_BARRAS_LISTA, OBJ_BARRAS = @OBJ_BARRAS, " +
                    "OBJ_CABLES_LISTA = @OBJ_CABLES_LISTA, OBJ_CABLES = @OBJ_CABLES, OBJ_LINEA_VIDA_LISTA = @OBJ_LINEA_VIDA_LISTA, OBJ_LINEA_VIDA = @OBJ_LINEA_VIDA, OBJ_OBSERVACIONES = @OBJ_OBSERVACIONES, " +
                    "OBJ_FECHA_ACTUALIZACION = @OBJ_FECHA_ACTUALIZACION,  " +
                    "WHERE OBJ_ID_REG = @OBJ_ID_REG";

                updateCommand.Parameters.AddWithValue("@OBJ_ID_REG", objeto.OBJ_ID_REG);
                updateCommand.Parameters.AddWithValue("@OBJ_MATRICULA", objeto.OBJ_MATRICULA);
                updateCommand.Parameters.AddWithValue("@OBJ_ID_ESTADO_REG", objeto.OBJ_ID_ESTADO_REG);
                updateCommand.Parameters.AddWithValue("@OBJ_SIGLAS_LISTA", objeto.OBJ_SIGLAS_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_SIGLAS", objeto.OBJ_SIGLAS);
                updateCommand.Parameters.AddWithValue("@OBJ_MODELO_LISTA", objeto.OBJ_MODELO_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_MODELO", objeto.OBJ_MODELO);
                updateCommand.Parameters.AddWithValue("@OBJ_ID_OBJETO", objeto.OBJ_ID_OBJETO);
                updateCommand.Parameters.AddWithValue("@OBJ_VARIANTE_LISTA", objeto.OBJ_VARIANTE_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_VARIANTE", objeto.OBJ_VARIANTE);
                updateCommand.Parameters.AddWithValue("@OBJ_TIPO_LISTA", objeto.OBJ_TIPO_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_TIPO", objeto.OBJ_TIPO);
                updateCommand.Parameters.AddWithValue("@OBJ_INSPEC_CSC", objeto.OBJ_INSPEC_CSC);
                updateCommand.Parameters.AddWithValue("@OBJ_PROPIETARIO_LISTA", objeto.OBJ_PROPIETARIO_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_PROPIETARIO", objeto.OBJ_PROPIETARIO);
                updateCommand.Parameters.AddWithValue("@OBJ_TARA_LISTA", objeto.OBJ_TARA_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_TARA", objeto.OBJ_TARA);
                updateCommand.Parameters.AddWithValue("@OBJ_PMP_LISTA", objeto.OBJ_PMP_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_PMP", objeto.OBJ_PMP);
                updateCommand.Parameters.AddWithValue("@OBJ_CARGA_UTIL", objeto.OBJ_CARGA_UTIL);
                updateCommand.Parameters.AddWithValue("@OBJ_ALTURA_EXTERIOR_LISTA", objeto.OBJ_ALTURA_EXTERIOR_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_ALTURA_EXTERIOR", objeto.OBJ_ALTURA_EXTERIOR);
                updateCommand.Parameters.AddWithValue("@OBJ_CUELLO_CISNE_LISTA", objeto.OBJ_CUELLO_CISNE_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_CUELLO_CISNE", objeto.OBJ_CUELLO_CISNE);
                updateCommand.Parameters.AddWithValue("@OBJ_BARRAS_LISTA", objeto.OBJ_BARRAS_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_BARRAS", objeto.OBJ_BARRAS);
                updateCommand.Parameters.AddWithValue("@OBJ_CABLES_LISTA", objeto.OBJ_CABLES_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_CABLES", objeto.OBJ_CABLES);
                updateCommand.Parameters.AddWithValue("@OBJ_LINEA_VIDA_LISTA", objeto.OBJ_LINEA_VIDA_LISTA);
                updateCommand.Parameters.AddWithValue("@OBJ_LINEA_VIDA", objeto.OBJ_LINEA_VIDA);
                updateCommand.Parameters.AddWithValue("@OBJ_OBSERVACIONES", objeto.OBJ_OBSERVACIONES);
                updateCommand.Parameters.AddWithValue("@OBJ_FECHA_ACTUALIZACION", DateTime.Now.ToShortDateString());

                await updateCommand.ExecuteReaderAsync();

                return true;
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
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFile}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                updateCommand.CommandText = "UPDATE TAREAS_PROGRAMADAS SET TAREAS_PROGRAMADAS_OBJETO_ID_REG=@TAREAS_PROGRAMADAS_OBJETO_ID_REG, TAREAS_PROGRAMADAS_FECHA_PROGRAMADA=@TAREAS_PROGRAMADAS_FECHA_PROGRAMADA " +
                    "TAREAS_PROGRAMADAS_FECHA_COMPLETADA=@TAREAS_PROGRAMADAS_FECHA_COMPLETADA, TAREAS_PROGRAMADAS_UBICACION_ORIGEN=@TAREAS_PROGRAMADAS_UBICACION_ORIGEN " +
                    "TAREAS_PROGRAMADAS_UBICACION_DESTINO=@TAREAS_PROGRAMADAS_UBICACION_DESTINO, TAREAS_PROGRAMADAS_ORDENADO=@TAREAS_PROGRAMADAS_ORDENADO, TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG=@TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG " +
                    "TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION=@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION WHERE TAREAS_PROGRAMADAS_ID_REG=@TAREAS_PROGRAMADAS_ID_REG;";
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_ID_REG);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_PROGRAMADA", tareaProgramada.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_COMPLETADA", tareaProgramada.TAREAS_PROGRAMADAS_FECHA_COMPLETADA);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_UBICACION_ORIGEN", tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_ORIGEN);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_UBICACION_DESTINO", tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_DESTINO);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_ORDENADO", tareaProgramada.TAREAS_PROGRAMADAS_ORDENADO);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG", tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG);
                updateCommand.Parameters.AddWithValue("@TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION", tareaProgramada.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION);

                await updateCommand.ExecuteReaderAsync();

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
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFile}"))
            {
                db.Open();

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