using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace ContainersDesktop.Core.Services;
public class ClaListServicio : IClaListServicio
{
    public async Task<bool> ActualizarClaLista(ClaList claList)
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
        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");

        try
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO CLALIST(CLALIST_ID_ESTADO_REG, CLALIST_DESCRIP, CLALIST_FECHA_ACTUALIZACION)" +
                    "VALUES(@CLALIST_ID_ESTADO_REG, @CLALIST_DESCRIP, @CLALIST_FECHA_ACTUALIZACION)";

                insertCommand.Parameters.AddWithValue("@CLALIST_DESCRIP", claList.CLALIST_DESCRIP);
                insertCommand.Parameters.AddWithValue("@CLALIST_ID_ESTADO_REG", "A");
                insertCommand.Parameters.AddWithValue("@CLALIST_FECHA_ACTUALIZACION", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));

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

        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT CLALIST_ID_REG, CLALIST_ID_ESTADO_REG, CLALIST_DESCRIP FROM CLALIST", db);

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
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand deleteCommand = new SqliteCommand
                    ("UPDATE CLALIST SET CLALIST_ID_ESTADO_REG = 'B', CLALIST_FECHA_ACTUALIZACION = @CLALIST_FECHA_ACTUALIZACION WHERE CLALIST_ID_REG = @CLALIST_ID_REG", db);

                deleteCommand.Parameters.AddWithValue("@CLALIST_ID_REG", id);
                deleteCommand.Parameters.AddWithValue("@CLALIST_FECHA_ACTUALIZACION", DateTime.Now.ToShortDateString());

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
