using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace ContainersDesktop.Core.Services;
public class MovimientosServicio : IMovimientosServicio
{
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
                if (query.GetString(1) == "A")
                {
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
                }
            }
        }

        return movimLista;
    }

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
                if (query.GetString(1) == "A")
                {
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
                }
            }
        }

        return movimLista;
    }

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
}
