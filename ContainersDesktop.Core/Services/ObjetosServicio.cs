using System.Collections.Generic;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Models.Storage;
using ContainersDesktop.Core.Persistencia;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Core.Services;
public class ObjetosServicio : IObjetosServicio
{
    private readonly string _dbFile;
    private readonly string _dbFullPath;

    public ObjetosServicio(IOptions<Settings> settings)
    {
        _dbFile = Path.Combine(settings.Value.DBFolder, settings.Value.DBName);
        _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFile}";
    }

    public async Task<bool> ActualizarObjeto(Objetos objeto)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
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
                    "OBJ_FECHA_ACTUALIZACION = @OBJ_FECHA_ACTUALIZACION  " +
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
                updateCommand.Parameters.AddWithValue("@OBJ_FECHA_ACTUALIZACION", objeto.OBJ_FECHA_ACTUALIZACION);

                await updateCommand.ExecuteReaderAsync();

                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<int> CrearObjeto(Objetos objeto)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO OBJETOS (OBJ_MATRICULA, OBJ_ID_ESTADO_REG, OBJ_SIGLAS_LISTA, OBJ_SIGLAS, OBJ_MODELO_LISTA, " +
                    "OBJ_PROPIETARIO_LISTA, OBJ_PROPIETARIO, OBJ_MODELO, OBJ_ID_OBJETO, OBJ_VARIANTE_LISTA, OBJ_VARIANTE, OBJ_TIPO_LISTA, OBJ_TIPO, OBJ_INSPEC_CSC, " +                    
                    "OBJ_TARA_LISTA, OBJ_TARA, OBJ_PMP_LISTA, OBJ_PMP, OBJ_CARGA_UTIL, OBJ_ALTURA_EXTERIOR_LISTA, OBJ_ALTURA_EXTERIOR, OBJ_CUELLO_CISNE_LISTA," +
                    "OBJ_CUELLO_CISNE, OBJ_BARRAS_LISTA, OBJ_BARRAS, OBJ_CABLES_LISTA, OBJ_CABLES, OBJ_LINEA_VIDA_LISTA, OBJ_LINEA_VIDA, OBJ_OBSERVACIONES, OBJ_FECHA_ACTUALIZACION)" +
                    "VALUES (@OBJ_MATRICULA, @OBJ_ID_ESTADO_REG, @OBJ_SIGLAS_LISTA, @OBJ_SIGLAS, @OBJ_MODELO_LISTA, @OBJ_PROPIETARIO_LISTA, @OBJ_PROPIETARIO," +
                    "@OBJ_MODELO, @OBJ_ID_OBJETO, @OBJ_VARIANTE_LISTA, @OBJ_VARIANTE, @OBJ_TIPO_LISTA, @OBJ_TIPO, @OBJ_INSPEC_CSC, " +
                    "@OBJ_TARA_LISTA, @OBJ_TARA, @OBJ_PMP_LISTA, @OBJ_PMP, @OBJ_CARGA_UTIL, @OBJ_ALTURA_EXTERIOR_LISTA, @OBJ_ALTURA_EXTERIOR, @OBJ_CUELLO_CISNE_LISTA, " +
                    "@OBJ_CUELLO_CISNE, @OBJ_BARRAS_LISTA, @OBJ_BARRAS, @OBJ_CABLES_LISTA, @OBJ_CABLES, @OBJ_LINEA_VIDA_LISTA, @OBJ_LINEA_VIDA, @OBJ_OBSERVACIONES, @OBJ_FECHA_ACTUALIZACION);";

                insertCommand.Parameters.AddWithValue("@OBJ_MATRICULA", objeto.OBJ_MATRICULA);
                insertCommand.Parameters.AddWithValue("@OBJ_ID_ESTADO_REG", objeto.OBJ_ID_ESTADO_REG);
                insertCommand.Parameters.AddWithValue("@OBJ_SIGLAS_LISTA", objeto.OBJ_SIGLAS_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_SIGLAS", objeto.OBJ_SIGLAS);
                insertCommand.Parameters.AddWithValue("@OBJ_MODELO_LISTA", objeto.OBJ_MODELO_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_MODELO", objeto.OBJ_MODELO);
                insertCommand.Parameters.AddWithValue("@OBJ_ID_OBJETO", objeto.OBJ_ID_OBJETO);
                insertCommand.Parameters.AddWithValue("@OBJ_VARIANTE_LISTA", objeto.OBJ_VARIANTE_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_VARIANTE", objeto.OBJ_VARIANTE);
                insertCommand.Parameters.AddWithValue("@OBJ_TIPO_LISTA", objeto.OBJ_TIPO_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_TIPO", objeto.OBJ_TIPO);
                insertCommand.Parameters.AddWithValue("@OBJ_INSPEC_CSC", objeto.OBJ_INSPEC_CSC);
                insertCommand.Parameters.AddWithValue("@OBJ_PROPIETARIO_LISTA", objeto.OBJ_PROPIETARIO_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_PROPIETARIO", objeto.OBJ_PROPIETARIO);
                insertCommand.Parameters.AddWithValue("@OBJ_TARA_LISTA", objeto.OBJ_TARA_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_TARA", objeto.OBJ_TARA);
                insertCommand.Parameters.AddWithValue("@OBJ_PMP_LISTA", objeto.OBJ_PMP_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_PMP", objeto.OBJ_PMP);
                insertCommand.Parameters.AddWithValue("@OBJ_CARGA_UTIL", objeto.OBJ_CARGA_UTIL);
                insertCommand.Parameters.AddWithValue("@OBJ_ALTURA_EXTERIOR_LISTA", objeto.OBJ_ALTURA_EXTERIOR_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_ALTURA_EXTERIOR", objeto.OBJ_ALTURA_EXTERIOR);
                insertCommand.Parameters.AddWithValue("@OBJ_CUELLO_CISNE_LISTA", objeto.OBJ_CUELLO_CISNE_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_CUELLO_CISNE", objeto.OBJ_CUELLO_CISNE);
                insertCommand.Parameters.AddWithValue("@OBJ_BARRAS_LISTA", objeto.OBJ_BARRAS_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_BARRAS", objeto.OBJ_BARRAS);
                insertCommand.Parameters.AddWithValue("@OBJ_CABLES_LISTA", objeto.OBJ_CABLES_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_CABLES", objeto.OBJ_CABLES);
                insertCommand.Parameters.AddWithValue("@OBJ_LINEA_VIDA_LISTA", objeto.OBJ_LINEA_VIDA_LISTA);
                insertCommand.Parameters.AddWithValue("@OBJ_LINEA_VIDA", objeto.OBJ_LINEA_VIDA);
                insertCommand.Parameters.AddWithValue("@OBJ_OBSERVACIONES", objeto.OBJ_OBSERVACIONES);
                insertCommand.Parameters.AddWithValue("@OBJ_FECHA_ACTUALIZACION", objeto.OBJ_FECHA_ACTUALIZACION);

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

    public async Task<List<Objetos>> ObtenerObjetos()
    {
        List<Objetos> objetos = new List<Objetos>();

        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT OBJ_ID_REG, OBJ_MATRICULA, OBJ_ID_ESTADO_REG, OBJ_SIGLAS_LISTA, OBJ_SIGLAS, OBJ_MODELO_LISTA, OBJ_MODELO, OBJ_ID_OBJETO, OBJ_VARIANTE_LISTA, " +
                "OBJ_VARIANTE, OBJ_TIPO_LISTA, OBJ_TIPO, OBJ_INSPEC_CSC, OBJ_PROPIETARIO_LISTA, OBJ_PROPIETARIO, OBJ_TARA_LISTA, OBJ_TARA, OBJ_PMP_LISTA, OBJ_PMP, " +
                "OBJ_CARGA_UTIL, OBJ_ALTURA_EXTERIOR_LISTA, OBJ_ALTURA_EXTERIOR, OBJ_CUELLO_CISNE_LISTA, OBJ_CUELLO_CISNE, OBJ_BARRAS_LISTA, OBJ_BARRAS, OBJ_CABLES_LISTA, " +
                "OBJ_CABLES, OBJ_LINEA_VIDA_LISTA, OBJ_LINEA_VIDA, OBJ_OBSERVACIONES, OBJ_FECHA_ACTUALIZACION FROM OBJETOS", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();
            
            while (query.Read())
            {
                //if (query.GetString(2) == "A")
                //{
                    var nuevoObjeto = new Objetos()
                    {
                        OBJ_ID_REG = query.GetInt32(0),
                        OBJ_MATRICULA = query.GetString(1),
                        OBJ_ID_ESTADO_REG = query.GetString(2),
                        OBJ_SIGLAS_LISTA = query.GetInt32(3),
                        OBJ_SIGLAS = query.GetInt32(4),
                        OBJ_MODELO_LISTA = query.GetInt32(5),
                        OBJ_MODELO = query.GetInt32(6),
                        OBJ_ID_OBJETO = query.GetInt32(7),
                        OBJ_VARIANTE_LISTA = query.GetInt32(8),
                        OBJ_VARIANTE = query.GetInt32(9),
                        OBJ_TIPO_LISTA = query.GetInt32(10),
                        OBJ_TIPO = query.GetInt32(11),
                        OBJ_INSPEC_CSC = FormatoFecha.ConvertirAFechaHora(query.GetString(12)),
                        OBJ_PROPIETARIO_LISTA = query.GetInt32(13),
                        OBJ_PROPIETARIO = query.GetInt32(14),
                        OBJ_TARA_LISTA = query.GetInt32(15),
                        OBJ_TARA = query.GetInt32(16),
                        OBJ_PMP_LISTA = query.GetInt32(17),
                        OBJ_PMP = query.GetInt32(18),
                        OBJ_CARGA_UTIL = query.GetInt32(19),
                        OBJ_ALTURA_EXTERIOR_LISTA = query.GetInt32(20),
                        OBJ_ALTURA_EXTERIOR = query.GetInt32(21),
                        OBJ_CUELLO_CISNE_LISTA = query.GetInt32(22),
                        OBJ_CUELLO_CISNE = query.GetInt32(23),
                        OBJ_BARRAS_LISTA = query.GetInt32(24),
                        OBJ_BARRAS = query.GetInt32(25),
                        OBJ_CABLES_LISTA = query.GetInt32(26),
                        OBJ_CABLES = query.GetInt32(27),
                        OBJ_LINEA_VIDA_LISTA = query.GetInt32(28),
                        OBJ_LINEA_VIDA = query.GetInt32(29),
                        OBJ_OBSERVACIONES = query.GetString(30),
                        OBJ_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaCorta(query.GetString(31)),
                    };
                    objetos.Add(nuevoObjeto);
                //}                
            }
        }

        return objetos;
    }

    public async Task<Objetos> ObtenerObjetoPorId(int id)
    {     
        var objeto = new Objetos();
        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT OBJ_ID_REG, OBJ_MATRICULA, OBJ_ID_ESTADO_REG, OBJ_SIGLAS_LISTA, OBJ_SIGLAS, OBJ_MODELO_LISTA, OBJ_MODELO, OBJ_ID_OBJETO, OBJ_VARIANTE_LISTA, " +
                "OBJ_VARIANTE, OBJ_TIPO_LISTA, OBJ_TIPO, OBJ_INSPEC_CSC, OBJ_PROPIETARIO_LISTA, OBJ_PROPIETARIO, OBJ_TARA_LISTA, OBJ_TARA, OBJ_PMP_LISTA, OBJ_PMP, " +
                "OBJ_CARGA_UTIL, OBJ_ALTURA_EXTERIOR_LISTA, OBJ_ALTURA_EXTERIOR, OBJ_CUELLO_CISNE_LISTA, OBJ_CUELLO_CISNE, OBJ_BARRAS_LISTA, OBJ_BARRAS, OBJ_CABLES_LISTA, " +
                "OBJ_CABLES, OBJ_LINEA_VIDA_LISTA, OBJ_LINEA_VIDA, OBJ_OBSERVACIONES FROM OBJETOS WHERE OBJ_ID_REG = @OBJ_ID_REG", db);

            selectCommand.Parameters.AddWithValue("@OBJ_ID_REG", id);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                objeto.OBJ_ID_REG = query.GetInt32(0);
                objeto.OBJ_MATRICULA = query.GetString(1);
                objeto.OBJ_ID_ESTADO_REG = query.GetString(2);
                objeto.OBJ_SIGLAS_LISTA = query.GetInt32(3);
                objeto.OBJ_SIGLAS = query.GetInt32(4);
                objeto.OBJ_MODELO_LISTA = query.GetInt32(5);
                objeto.OBJ_MODELO = query.GetInt32(6);
                objeto.OBJ_ID_OBJETO = query.GetInt32(7);
                objeto.OBJ_VARIANTE_LISTA = query.GetInt32(8);
                objeto.OBJ_VARIANTE = query.GetInt32(9);
                objeto.OBJ_TIPO_LISTA = query.GetInt32(10);
                objeto.OBJ_TIPO = query.GetInt32(11);
                objeto.OBJ_INSPEC_CSC = query.GetString(12);
                objeto.OBJ_PROPIETARIO_LISTA = query.GetInt32(13);
                objeto.OBJ_PROPIETARIO = query.GetInt32(14);
                objeto.OBJ_TARA_LISTA = query.GetInt32(15);
                objeto.OBJ_TARA = query.GetInt32(16);
                objeto.OBJ_PMP_LISTA = query.GetInt32(17);
                objeto.OBJ_PMP = query.GetInt32(18);
                objeto.OBJ_CARGA_UTIL = query.GetInt32(19);
                objeto.OBJ_ALTURA_EXTERIOR_LISTA = query.GetInt32(20);
                objeto.OBJ_ALTURA_EXTERIOR = query.GetInt32(21);
                objeto.OBJ_CUELLO_CISNE_LISTA = query.GetInt32(22);
                objeto.OBJ_CUELLO_CISNE = query.GetInt32(23);
                objeto.OBJ_BARRAS_LISTA = query.GetInt32(24);
                objeto.OBJ_BARRAS = query.GetInt32(25);
                objeto.OBJ_CABLES_LISTA = query.GetInt32(26);
                objeto.OBJ_CABLES = query.GetInt32(27);
                objeto.OBJ_LINEA_VIDA_LISTA = query.GetInt32(28);
                objeto.OBJ_LINEA_VIDA = query.GetInt32(29);
                objeto.OBJ_OBSERVACIONES = query.GetString(30);
            }
        }

        return objeto;
    }

    public async Task<bool> BorrarObjeto(int id)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand deleteCommand = new SqliteCommand
                    ("UPDATE OBJETOS SET OBJ_ID_ESTADO_REG = 'B', OBJ_FECHA_ACTUALIZACION = @OBJ_FECHA_ACTUALIZACION WHERE OBJ_ID_REG = @OBJ_ID_REG", db);

                deleteCommand.Parameters.AddWithValue("@OBJ_ID_REG", id);
                deleteCommand.Parameters.AddWithValue("@OBJ_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));

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
