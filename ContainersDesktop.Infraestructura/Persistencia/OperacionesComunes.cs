using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ContainersDesktop.Core.Persistencia;
public static class OperacionesComunes
{    
    public static async Task<int> GetIdentity(SqliteConnection db)
    {
        SqliteCommand selectCommand = new SqliteCommand
                ("SELECT last_insert_rowid()", db);

        SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

        var identity = 0;
        while (query.Read())
        {
            identity = query.GetInt32(0);
        }

        return identity;
    }    
}
