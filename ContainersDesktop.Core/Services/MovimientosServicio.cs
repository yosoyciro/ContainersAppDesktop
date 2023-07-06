using System.Collections.Generic;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Persistencia;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace ContainersDesktop.Core.Services;
public class MovimientosServicio : IMovimientosServicio
{
    #region ObtenerMovimientosObjeto
    public async Task<List<Movim>> ObtenerMovimientosObjeto(int idObjeto)
    {
        List<Movim> movimLista = new();

        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT MOVIM_ID_REG, MOVIM_ID_ESTADO_REG, MOVIM_FECHA, MOVIM_ID_OBJETO, MOVIM_TIPO_MOVIM_LISTA, MOVIM_TIPO_MOVIM, MOVIM_PESO_LISTA, MOVIM_PESO, " +
                "MOVIM_TRANSPORTISTA_LISTA, MOVIM_TRANSPORTISTA, MOVIM_CLIENTE_LISTA, MOVIM_CLIENTE, MOVIM_CHOFER_LISTA, MOVIM_CHOFER, MOVIM_CAMION_ID, " +
                "MOVIM_REMOLQUE_ID, MOVIM_ALBARAN_ID, MOVIM_OBSERVACIONES, MOVIM_ENTRADA_SALIDA_LISTA, MOVIM_ENTRADA_SALIDA, MOVIM_ALMACEN_LISTA, " +
                "MOVIM_ALMACEN, MOVIM_PDF, MOVIM_FECHA_ACTUALIZACION, MOVIM_ID_DISPOSITIVO " +
                "FROM MOVIM WHERE MOVIM_ID_OBJETO = @MOVIM_ID_OBJETO", db);

            selectCommand.Parameters.AddWithValue("@MOVIM_ID_OBJETO", idObjeto);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                //if (query.GetString(1) == "A")
                //{
                    var movimObjeto = new Movim()
                    {
                        MOVIM_ID_REG = query.GetInt32(0),
                        MOVIM_ID_ESTADO_REG = query.GetString(1),
                        MOVIM_FECHA = FormatoFecha.ConvertirAFechaCorta(query.GetString(2)),
                        MOVIM_ID_OBJETO = query.GetInt32(3),
                        MOVIM_TIPO_MOVIM_LISTA = query.GetInt32(4),
                        MOVIM_TIPO_MOVIM = query.GetInt32(5),
                        MOVIM_PESO_LISTA = query.GetInt32(6),
                        MOVIM_PESO = query.GetInt32(7),
                        MOVIM_TRANSPORTISTA_LISTA = query.GetInt32(8),
                        MOVIM_TRANSPORTISTA = query.GetInt32(9),
                        MOVIM_CLIENTE_LISTA = query.GetInt32(10),
                        MOVIM_CLIENTE = query.GetInt32(11),
                        MOVIM_CHOFER_LISTA = query.GetInt32(12),
                        MOVIM_CHOFER = query.GetInt32(13),
                        MOVIM_CAMION_ID = query.GetString(14),
                        MOVIM_REMOLQUE_ID = query.GetString(15),
                        MOVIM_ALBARAN_ID = query.GetString(16),
                        MOVIM_OBSERVACIONES = query.GetString(17),
                        MOVIM_ENTRADA_SALIDA_LISTA = query.GetInt32(18),
                        MOVIM_ENTRADA_SALIDA = query.GetInt32(19),
                        MOVIM_ALMACEN_LISTA = query.GetInt32(20),
                        MOVIM_ALMACEN = query.GetInt32(21),
                        MOVIM_PDF = query.GetString(22),
                        MOVIM_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaCorta(query.GetString(23)),
                        MOVIM_ID_DISPOSITIVO = query.GetInt32(24),
                    };
                    movimLista.Add(movimObjeto);
                //}
            }
        }

        return movimLista;
    }
    #endregion

    #region ObtenerMovimientosTodos
    public async Task<List<Movim>> ObtenerMovimientosTodos()
    {
        List<Movim> movimLista = new();

        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT MOVIM_ID_REG, MOVIM_ID_ESTADO_REG, MOVIM_FECHA, MOVIM_ID_OBJETO, MOVIM_TIPO_MOVIM_LISTA, MOVIM_TIPO_MOVIM, MOVIM_PESO_LISTA, MOVIM_PESO, " +
                "MOVIM_TRANSPORTISTA_LISTA, MOVIM_TRANSPORTISTA, MOVIM_CLIENTE_LISTA, MOVIM_CLIENTE, MOVIM_CHOFER_LISTA, MOVIM_CHOFER, MOVIM_CAMION_ID, " +
                "MOVIM_REMOLQUE_ID, MOVIM_ALBARAN_ID, MOVIM_OBSERVACIONES, MOVIM_ENTRADA_SALIDA_LISTA, MOVIM_ENTRADA_SALIDA, MOVIM_ALMACEN_LISTA, " +
                "MOVIM_ALMACEN, MOVIM_PDF, MOVIM_FECHA_ACTUALIZACION, MOVIM_ID_DISPOSITIVO FROM MOVIM", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                //if (query.GetString(1) == "A")
                //{
                    var movimObjeto = new Movim()
                    {
                        MOVIM_ID_REG = query.GetInt32(0),
                        MOVIM_ID_ESTADO_REG = query.GetString(1),
                        MOVIM_FECHA = FormatoFecha.ConvertirAFechaCorta(query.GetString(2)),
                        MOVIM_ID_OBJETO = query.GetInt32(3),
                        MOVIM_TIPO_MOVIM_LISTA = query.GetInt32(4),
                        MOVIM_TIPO_MOVIM = query.GetInt32(5),
                        MOVIM_PESO_LISTA = query.GetInt32(6),
                        MOVIM_PESO = query.GetInt32(7),
                        MOVIM_TRANSPORTISTA_LISTA = query.GetInt32(8),
                        MOVIM_TRANSPORTISTA = query.GetInt32(9),
                        MOVIM_CLIENTE_LISTA = query.GetInt32(10),
                        MOVIM_CLIENTE = query.GetInt32(11),
                        MOVIM_CHOFER_LISTA = query.GetInt32(12),
                        MOVIM_CHOFER = query.GetInt32(13),
                        MOVIM_CAMION_ID = query.GetString(14),
                        MOVIM_REMOLQUE_ID = query.GetString(15),
                        MOVIM_ALBARAN_ID = query.GetString(16),
                        MOVIM_OBSERVACIONES = query.GetString(17),
                        MOVIM_ENTRADA_SALIDA_LISTA = query.GetInt32(18),
                        MOVIM_ENTRADA_SALIDA = query.GetInt32(19),
                        MOVIM_ALMACEN_LISTA = query.GetInt32(20),
                        MOVIM_ALMACEN = query.GetInt32(21),
                        MOVIM_PDF = query.GetString(22),
                        MOVIM_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaCorta(query.GetString(23)),
                        MOVIM_ID_DISPOSITIVO = query.GetInt32(24),
                    };
                    movimLista.Add(movimObjeto);
                //}
            }
        }

        return movimLista;
    }
    #endregion

    #region SincronizarMovimientos
    public async Task<bool> SincronizarMovimientos(string dbDescarga, int idDispositivo)
    {
        List<Movim> movimLista = new();
        using (SqliteConnection descargaDb = new SqliteConnection($"Filename={dbDescarga};Pooling=false"))
        {
            try
            {
                await descargaDb.OpenAsync();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT MOVIM_ID_REG, MOVIM_ID_ESTADO_REG, MOVIM_FECHA, MOVIM_ID_OBJETO, MOVIM_TIPO_MOVIM_LISTA, MOVIM_TIPO_MOVIM, MOVIM_PESO_LISTA, MOVIM_PESO, " +
                    "MOVIM_TRANSPORTISTA_LISTA, MOVIM_TRANSPORTISTA, MOVIM_CLIENTE_LISTA, MOVIM_CLIENTE, MOVIM_CHOFER_LISTA, MOVIM_CHOFER, MOVIM_CAMION_ID, " +
                    "MOVIM_REMOLQUE_ID, MOVIM_ALBARAN_ID, MOVIM_OBSERVACIONES, MOVIM_ENTRADA_SALIDA_LISTA, MOVIM_ENTRADA_SALIDA, MOVIM_ALMACEN_LISTA, " +
                    "MOVIM_ALMACEN, MOVIM_PDF, MOVIM_FECHA_ACTUALIZACION " +
                    "FROM Movim", descargaDb);

                SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

                while (query.Read())
                {
                    if (query.GetString(1) == "A")
                    {
                        var movimObjeto = new Movim()
                        {
                            MOVIM_ID_REG = query.GetInt32(0),
                            MOVIM_ID_ESTADO_REG = query.GetString(1),
                            MOVIM_FECHA = query.GetString(2),
                            MOVIM_ID_OBJETO = query.GetInt32(3),
                            MOVIM_TIPO_MOVIM_LISTA = query.GetInt32(4),
                            MOVIM_TIPO_MOVIM = query.GetInt32(5),
                            MOVIM_PESO_LISTA = query.GetInt32(6),
                            MOVIM_PESO = query.GetInt32(7),
                            MOVIM_TRANSPORTISTA_LISTA = query.GetInt32(8),
                            MOVIM_TRANSPORTISTA = query.GetInt32(9),
                            MOVIM_CLIENTE_LISTA = query.GetInt32(10),
                            MOVIM_CLIENTE = query.GetInt32(11),
                            MOVIM_CHOFER_LISTA = query.GetInt32(12),
                            MOVIM_CHOFER = query.GetInt32(13),
                            MOVIM_CAMION_ID = query.GetString(14),
                            MOVIM_REMOLQUE_ID = query.GetString(15),
                            MOVIM_ALBARAN_ID = query.GetString(16),
                            MOVIM_OBSERVACIONES = query.GetString(17),
                            MOVIM_ENTRADA_SALIDA_LISTA = query.GetInt32(18),
                            MOVIM_ENTRADA_SALIDA = query.GetInt32(19),
                            MOVIM_ALMACEN_LISTA = query.GetInt32(20),
                            MOVIM_ALMACEN = query.GetInt32(21),
                            MOVIM_PDF = query.GetString(22),
                            MOVIM_FECHA_ACTUALIZACION = query.GetString(23),
                        };

                        movimLista.Add(movimObjeto);
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

        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");        
        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
        {
            try
            {
                db.Open();                

                // Use parameterized query to prevent SQL injection attacks
                var insertCommand = "INSERT INTO MOVIM (MOVIM_ID_REG_MOBILE, MOVIM_ID_DISPOSITIVO, MOVIM_ID_ESTADO_REG, MOVIM_FECHA, MOVIM_ID_OBJETO, MOVIM_TIPO_MOVIM_LISTA, MOVIM_TIPO_MOVIM, " +
                    "MOVIM_PESO_LISTA, MOVIM_PESO, MOVIM_TRANSPORTISTA_LISTA, MOVIM_TRANSPORTISTA, MOVIM_CLIENTE_LISTA, MOVIM_CLIENTE, MOVIM_CHOFER_LISTA, MOVIM_CHOFER, " +
                    "MOVIM_CAMION_ID, MOVIM_REMOLQUE_ID, MOVIM_ALBARAN_ID, MOVIM_OBSERVACIONES, MOVIM_ENTRADA_SALIDA_LISTA, MOVIM_ENTRADA_SALIDA, MOVIM_ALMACEN_LISTA, MOVIM_ALMACEN, " +
                    "MOVIM_PDF, MOVIM_FECHA_ACTUALIZACION)" +
                    "VALUES (@MOVIM_ID_REG_MOBILE, @MOVIM_ID_DISPOSITIVO, @MOVIM_ID_ESTADO_REG, @MOVIM_FECHA, @MOVIM_ID_OBJETO, @MOVIM_TIPO_MOVIM_LISTA, @MOVIM_TIPO_MOVIM, " +
                    "@MOVIM_PESO_LISTA, @MOVIM_PESO, @MOVIM_TRANSPORTISTA_LISTA, @MOVIM_TRANSPORTISTA, @MOVIM_CLIENTE_LISTA, @MOVIM_CLIENTE, @MOVIM_CHOFER_LISTA, @MOVIM_CHOFER, " +
                    "@MOVIM_CAMION_ID, @MOVIM_REMOLQUE_ID, @MOVIM_ALBARAN_ID, @MOVIM_OBSERVACIONES, @MOVIM_ENTRADA_SALIDA_LISTA, @MOVIM_ENTRADA_SALIDA, @MOVIM_ALMACEN_LISTA, @MOVIM_ALMACEN, " +
                    "@MOVIM_PDF, @MOVIM_FECHA_ACTUALIZACION);";

                foreach (Movim movim in movimLista)
                {
                    //Verifico si el registro existe
                    SqliteCommand selectCommand = new SqliteCommand("SELECT MOVIM_ID_REG FROM " +
                        " MOVIM WHERE MOVIM_ID_REG_MOBILE = @MOVIM_ID_REG_MOBILE AND MOVIM_ID_DISPOSITIVO = @MOVIM_ID_DISPOSITIVO", db);
                    selectCommand.Parameters.AddWithValue("@MOVIM_ID_REG_MOBILE", movim.MOVIM_ID_REG);
                    selectCommand.Parameters.AddWithValue("@MOVIM_ID_DISPOSITIVO", idDispositivo);
                    SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

                    if (!query.HasRows)
                    {
                        using (var cmd = new SqliteCommand(insertCommand, db))
                        {
                            cmd.Parameters.AddWithValue("@MOVIM_ID_REG_MOBILE", movim.MOVIM_ID_REG);
                            cmd.Parameters.AddWithValue("@MOVIM_ID_ESTADO_REG", movim.MOVIM_ID_ESTADO_REG);
                            cmd.Parameters.AddWithValue("@MOVIM_ID_DISPOSITIVO", idDispositivo);
                            cmd.Parameters.AddWithValue("@MOVIM_FECHA", movim.MOVIM_FECHA);
                            cmd.Parameters.AddWithValue("@MOVIM_ID_OBJETO", movim.MOVIM_ID_OBJETO);
                            cmd.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM_LISTA", movim.MOVIM_TIPO_MOVIM_LISTA);
                            cmd.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM", movim.MOVIM_TIPO_MOVIM);
                            cmd.Parameters.AddWithValue("@MOVIM_PESO_LISTA", movim.MOVIM_PESO_LISTA);
                            cmd.Parameters.AddWithValue("@MOVIM_PESO", movim.MOVIM_PESO);
                            cmd.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA_LISTA", movim.MOVIM_TRANSPORTISTA_LISTA);
                            cmd.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA", movim.MOVIM_TRANSPORTISTA);
                            cmd.Parameters.AddWithValue("@MOVIM_CLIENTE_LISTA", movim.MOVIM_CLIENTE_LISTA);
                            cmd.Parameters.AddWithValue("@MOVIM_CLIENTE", movim.MOVIM_CLIENTE);
                            cmd.Parameters.AddWithValue("@MOVIM_CHOFER_LISTA", movim.MOVIM_CHOFER_LISTA);
                            cmd.Parameters.AddWithValue("@MOVIM_CHOFER", movim.MOVIM_CHOFER);
                            cmd.Parameters.AddWithValue("@MOVIM_CAMION_ID", movim.MOVIM_CAMION_ID);
                            cmd.Parameters.AddWithValue("@MOVIM_ALBARAN_ID", movim.MOVIM_ALBARAN_ID);
                            cmd.Parameters.AddWithValue("@MOVIM_REMOLQUE_ID", movim.MOVIM_REMOLQUE_ID);
                            cmd.Parameters.AddWithValue("@MOVIM_OBSERVACIONES", movim.MOVIM_OBSERVACIONES);
                            cmd.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA_LISTA", movim.MOVIM_ENTRADA_SALIDA_LISTA);
                            cmd.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA", movim.MOVIM_ENTRADA_SALIDA);
                            cmd.Parameters.AddWithValue("@MOVIM_ALMACEN_LISTA", movim.MOVIM_ALMACEN_LISTA);
                            cmd.Parameters.AddWithValue("@MOVIM_ALMACEN", movim.MOVIM_ALMACEN);
                            cmd.Parameters.AddWithValue("@MOVIM_PDF", movim.MOVIM_PDF);
                            cmd.Parameters.AddWithValue("@MOVIM_FECHA_ACTUALIZACION", movim.MOVIM_FECHA_ACTUALIZACION);
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

    public async Task<bool> ActualizarObjeto(Objetos objeto)
    {
        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");

        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
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

    #region CrearMovimiento
    public async Task<int> CrearMovimiento(Movim movim)
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
                insertCommand.CommandText = "INSERT INTO MOVIM(MOVIM_ID_REG_MOBILE, MOVIM_ID_DISPOSITIVO, MOVIM_ID_ESTADO_REG, MOVIM_FECHA, MOVIM_ID_OBJETO, MOVIM_TIPO_MOVIM_LISTA, MOVIM_TIPO_MOVIM, " +
                    "MOVIM_PESO_LISTA, MOVIM_PESO, MOVIM_TRANSPORTISTA_LISTA, MOVIM_TRANSPORTISTA, MOVIM_CLIENTE_LISTA, MOVIM_CLIENTE, MOVIM_CHOFER_LISTA, MOVIM_CHOFER, " +
                    "MOVIM_CAMION_ID, MOVIM_REMOLQUE_ID, MOVIM_ALBARAN_ID, MOVIM_OBSERVACIONES, MOVIM_ENTRADA_SALIDA_LISTA, MOVIM_ENTRADA_SALIDA, MOVIM_ALMACEN_LISTA, MOVIM_ALMACEN, " +
                    "MOVIM_PDF, MOVIM_FECHA_ACTUALIZACION)" +
                    "VALUES (@MOVIM_ID_REG_MOBILE, @MOVIM_ID_DISPOSITIVO, @MOVIM_ID_ESTADO_REG, @MOVIM_FECHA, @MOVIM_ID_OBJETO, @MOVIM_TIPO_MOVIM_LISTA, @MOVIM_TIPO_MOVIM, " +
                    "@MOVIM_PESO_LISTA, @MOVIM_PESO, @MOVIM_TRANSPORTISTA_LISTA, @MOVIM_TRANSPORTISTA, @MOVIM_CLIENTE_LISTA, @MOVIM_CLIENTE, @MOVIM_CHOFER_LISTA, @MOVIM_CHOFER, " +
                    "@MOVIM_CAMION_ID, @MOVIM_REMOLQUE_ID, @MOVIM_ALBARAN_ID, @MOVIM_OBSERVACIONES, @MOVIM_ENTRADA_SALIDA_LISTA, @MOVIM_ENTRADA_SALIDA, @MOVIM_ALMACEN_LISTA, @MOVIM_ALMACEN, " +
                    "@MOVIM_PDF, @MOVIM_FECHA_ACTUALIZACION);";
                insertCommand.Parameters.AddWithValue("@MOVIM_ID_REG_MOBILE", movim.MOVIM_ID_REG_MOBILE);
                insertCommand.Parameters.AddWithValue("@MOVIM_ID_ESTADO_REG", movim.MOVIM_ID_ESTADO_REG);
                insertCommand.Parameters.AddWithValue("@MOVIM_ID_DISPOSITIVO", movim.MOVIM_ID_DISPOSITIVO);
                insertCommand.Parameters.AddWithValue("@MOVIM_FECHA", movim.MOVIM_FECHA);
                insertCommand.Parameters.AddWithValue("@MOVIM_ID_OBJETO", movim.MOVIM_ID_OBJETO);
                insertCommand.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM_LISTA", movim.MOVIM_TIPO_MOVIM_LISTA);
                insertCommand.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM", movim.MOVIM_TIPO_MOVIM);
                insertCommand.Parameters.AddWithValue("@MOVIM_PESO_LISTA", movim.MOVIM_PESO_LISTA);
                insertCommand.Parameters.AddWithValue("@MOVIM_PESO", movim.MOVIM_PESO);
                insertCommand.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA_LISTA", movim.MOVIM_TRANSPORTISTA_LISTA);
                insertCommand.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA", movim.MOVIM_TRANSPORTISTA);
                insertCommand.Parameters.AddWithValue("@MOVIM_CLIENTE_LISTA", movim.MOVIM_CLIENTE_LISTA);
                insertCommand.Parameters.AddWithValue("@MOVIM_CLIENTE", movim.MOVIM_CLIENTE);
                insertCommand.Parameters.AddWithValue("@MOVIM_CHOFER_LISTA", movim.MOVIM_CHOFER_LISTA);
                insertCommand.Parameters.AddWithValue("@MOVIM_CHOFER", movim.MOVIM_CHOFER);
                insertCommand.Parameters.AddWithValue("@MOVIM_CAMION_ID", movim.MOVIM_CAMION_ID);
                insertCommand.Parameters.AddWithValue("@MOVIM_ALBARAN_ID", movim.MOVIM_ALBARAN_ID);
                insertCommand.Parameters.AddWithValue("@MOVIM_REMOLQUE_ID", movim.MOVIM_REMOLQUE_ID);
                insertCommand.Parameters.AddWithValue("@MOVIM_OBSERVACIONES", movim.MOVIM_OBSERVACIONES);
                insertCommand.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA_LISTA", movim.MOVIM_ENTRADA_SALIDA_LISTA);
                insertCommand.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA", movim.MOVIM_ENTRADA_SALIDA);
                insertCommand.Parameters.AddWithValue("@MOVIM_ALMACEN_LISTA", movim.MOVIM_ALMACEN_LISTA);
                insertCommand.Parameters.AddWithValue("@MOVIM_ALMACEN", movim.MOVIM_ALMACEN);
                insertCommand.Parameters.AddWithValue("@MOVIM_PDF", movim.MOVIM_PDF);
                insertCommand.Parameters.AddWithValue("@MOVIM_FECHA_ACTUALIZACION", movim.MOVIM_FECHA_ACTUALIZACION);

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

    #region ActualizarMovimiento
    public async Task<bool> ActualizarMovimiento(Movim movim)
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
                updateCommand.CommandText = "UPDATE MOVIM SET MOVIM_ID_REG_MOBILE=0, MOVIM_ID_DISPOSITIVO=@MOVIM_ID_DISPOSITIVO, MOVIM_ID_ESTADO_REG=@MOVIM_ID_ESTADO_REG, " +
                    "MOVIM_FECHA=@MOVIM_FECHA, MOVIM_ID_OBJETO=@MOVIM_ID_OBJETO, MOVIM_TIPO_MOVIM_LISTA=@MOVIM_TIPO_MOVIM_LISTA, MOVIM_TIPO_MOVIM=@MOVIM_TIPO_MOVIM, " +
                    "MOVIM_PESO_LISTA=@MOVIM_PESO_LISTA, MOVIM_PESO=@MOVIM_PESO, MOVIM_TRANSPORTISTA_LISTA=@MOVIM_TRANSPORTISTA_LISTA, MOVIM_TRANSPORTISTA=@MOVIM_TRANSPORTISTA, " +
                    "MOVIM_CLIENTE_LISTA=@MOVIM_CLIENTE_LISTA, MOVIM_CLIENTE=@MOVIM_CLIENTE, MOVIM_CHOFER_LISTA=@MOVIM_CHOFER_LISTA, MOVIM_CHOFER=@MOVIM_CHOFER, " +
                    "MOVIM_CAMION_ID=@MOVIM_CAMION_ID, MOVIM_REMOLQUE_ID=@MOVIM_REMOLQUE_ID, MOVIM_ALBARAN_ID=@MOVIM_ALBARAN_ID, MOVIM_OBSERVACIONES=@MOVIM_OBSERVACIONES, " +
                    "MOVIM_ENTRADA_SALIDA_LISTA=@MOVIM_ENTRADA_SALIDA_LISTA, MOVIM_ENTRADA_SALIDA=@MOVIM_ENTRADA_SALIDA, MOVIM_ALMACEN_LISTA=@MOVIM_ALMACEN_LISTA, " +
                    "MOVIM_ALMACEN=@MOVIM_ALMACEN, MOVIM_PDF=@MOVIM_PDF, MOVIM_FECHA_ACTUALIZACION=@MOVIM_FECHA_ACTUALIZACION " +
                    "WHERE MOVIM_ID_REG=@MOVIM_ID_REG;";
                updateCommand.Parameters.AddWithValue("@MOVIM_ID_REG", movim.MOVIM_ID_REG);
                updateCommand.Parameters.AddWithValue("@MOVIM_ID_DISPOSITIVO", movim.MOVIM_ID_DISPOSITIVO);
                updateCommand.Parameters.AddWithValue("@MOVIM_ID_ESTADO_REG", movim.MOVIM_ID_ESTADO_REG);
                updateCommand.Parameters.AddWithValue("@MOVIM_FECHA", movim.MOVIM_FECHA);
                updateCommand.Parameters.AddWithValue("@MOVIM_ID_OBJETO", movim.MOVIM_ID_OBJETO);
                updateCommand.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM_LISTA", movim.MOVIM_TIPO_MOVIM_LISTA);
                updateCommand.Parameters.AddWithValue("@MOVIM_TIPO_MOVIM", movim.MOVIM_TIPO_MOVIM);
                updateCommand.Parameters.AddWithValue("@MOVIM_PESO_LISTA", movim.MOVIM_PESO_LISTA);
                updateCommand.Parameters.AddWithValue("@MOVIM_PESO", movim.MOVIM_PESO);
                updateCommand.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA_LISTA", movim.MOVIM_TRANSPORTISTA_LISTA);
                updateCommand.Parameters.AddWithValue("@MOVIM_TRANSPORTISTA", movim.MOVIM_TRANSPORTISTA);
                updateCommand.Parameters.AddWithValue("@MOVIM_CLIENTE_LISTA", movim.MOVIM_CLIENTE_LISTA);
                updateCommand.Parameters.AddWithValue("@MOVIM_CLIENTE", movim.MOVIM_CLIENTE);
                updateCommand.Parameters.AddWithValue("@MOVIM_CHOFER_LISTA", movim.MOVIM_CHOFER_LISTA);
                updateCommand.Parameters.AddWithValue("@MOVIM_CHOFER", movim.MOVIM_CHOFER);
                updateCommand.Parameters.AddWithValue("@MOVIM_CAMION_ID", movim.MOVIM_CAMION_ID);
                updateCommand.Parameters.AddWithValue("@MOVIM_ALBARAN_ID", movim.MOVIM_ALBARAN_ID);
                updateCommand.Parameters.AddWithValue("@MOVIM_REMOLQUE_ID", movim.MOVIM_REMOLQUE_ID);
                updateCommand.Parameters.AddWithValue("@MOVIM_OBSERVACIONES", movim.MOVIM_OBSERVACIONES);
                updateCommand.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA_LISTA", movim.MOVIM_ENTRADA_SALIDA_LISTA);
                updateCommand.Parameters.AddWithValue("@MOVIM_ENTRADA_SALIDA", movim.MOVIM_ENTRADA_SALIDA);
                updateCommand.Parameters.AddWithValue("@MOVIM_ALMACEN_LISTA", movim.MOVIM_ALMACEN_LISTA);
                updateCommand.Parameters.AddWithValue("@MOVIM_ALMACEN", movim.MOVIM_ALMACEN);
                updateCommand.Parameters.AddWithValue("@MOVIM_PDF", movim.MOVIM_PDF);
                updateCommand.Parameters.AddWithValue("@MOVIM_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar());

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

    #region BajaMovimiento
    public async Task<bool> BorrarMovimiento(int id)
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
                updateCommand.CommandText = "UPDATE MOVIM SET MOVIM_ID_ESTADO_REG=@MOVIM_ID_ESTADO_REG WHERE MOVIM_ID_REG=@MOVIM_ID_REG;";
                updateCommand.Parameters.AddWithValue("@MOVIM_ID_REG", id);
                updateCommand.Parameters.AddWithValue("@MOVIM_ID_ESTADO_REG", "B");                

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
