using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using ContainersDesktop.Infraestructura.Contracts.Services;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ContainersDesktop.Infraestructura.Persistencia.Repositorios;
public class ClaListServicio : IClaListServicio
{
    private readonly string _dbFile;
    private readonly string _dbFullPath;

    public ClaListServicio(IOptions<Settings> settings)
    {
        _dbFile = Path.Combine(settings.Value.DBFolder, settings.Value.DBName);
        _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFile}";
    }
    public async Task<bool> ActualizarClaLista(ClaList claList)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                updateCommand.CommandText = "UPDATE CLALIST SET CLALIST_DESCRIP = @CLALIST_DESCRIP WHERE CLALIST_ID_REG = @CLALIST_ID_REG";

                updateCommand.Parameters.AddWithValue("@CLALIST_DESCRIP", claList.CLALIST_DESCRIP);
                updateCommand.Parameters.AddWithValue("@CLALIST_ID_REG", claList.CLALIST_ID_REG);                
                

                await updateCommand.ExecuteReaderAsync();

                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> CrearClaLista(ClaList claList)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO CLALIST(CLALIST_ID_ESTADO_REG, CLALIST_DESCRIP, CLALIST_FECHA_ACTUALIZACION)" +
                    "VALUES(@CLALIST_ID_ESTADO_REG, @CLALIST_DESCRIP, @CLALIST_FECHA_ACTUALIZACION)";

                insertCommand.Parameters.AddWithValue("@CLALIST_DESCRIP", claList.CLALIST_DESCRIP);
                insertCommand.Parameters.AddWithValue("@CLALIST_ID_ESTADO_REG", "A");
                insertCommand.Parameters.AddWithValue("@CLALIST_FECHA_ACTUALIZACION", FormatoFecha.FechaEstandar(DateTime.Now));

                await insertCommand.ExecuteReaderAsync();

                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<ClaList>> ObtenerClaListas()
    {
        List<ClaList> clasList = new();

        using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT CLALIST_ID_REG, CLALIST_ID_ESTADO_REG, CLALIST_DESCRIP, CLALIST_FECHA_ACTUALIZACION FROM CLALIST", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                //if (query.GetString(1) == "A")
                //{
                    var clasListObjeto = new ClaList()
                    {
                        CLALIST_ID_REG = query.GetInt32(0),
                        CLALIST_ID_ESTADO_REG = query.GetString(1),
                        CLALIST_DESCRIP = query.GetString(2),             
                        CLALIST_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(query.GetString(3)),
                    };
                    clasList.Add(clasListObjeto);
                //}
            }
        }

        return clasList;
    }

    public async Task<bool> BorrarClaLista(int id)
    {
        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbFullPath}"))
            {
                db.Open();

                SqliteCommand deleteCommand = new SqliteCommand
                    ("UPDATE CLALIST SET CLALIST_ID_ESTADO_REG = 'B', CLALIST_FECHA_ACTUALIZACION = @CLALIST_FECHA_ACTUALIZACION WHERE CLALIST_ID_REG = @CLALIST_ID_REG", db);

                deleteCommand.Parameters.AddWithValue("@CLALIST_ID_REG", id);
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
