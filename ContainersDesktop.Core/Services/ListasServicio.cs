using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace ContainersDesktop.Core.Services;
public class ListasServicio : IListasServicio
{
    public Task<bool> CrearLista(Listas lista) => throw new NotImplementedException();
    public async Task<List<Listas>> ObtenerListas()
    {
        List<Listas> listas = new List<Listas>();

        var dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Containers.db");
        using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT LISTAS_ID_REG, LISTAS_ID_ESTADO_REG, LISTAS_ID_LISTA, LISTAS_ID_LISTA_ORDEN, LISTAS_ID_LISTA_DESCRIP FROM LISTAS", db);

            SqliteDataReader query = await selectCommand.ExecuteReaderAsync();

            while (query.Read())
            {
                var nuevaLista = new Listas()
                {
                    LISTAS_ID_REG = query.GetInt32(0),
                    LISTAS_ID_ESTADO_REG = query.GetString(1),
                    LISTAS_ID_LISTA = query.GetInt32(2),
                    LISTAS_ID_LISTA_ORDEN = query.GetInt32(3),
                    LISTAS_ID_LISTA_DESCRIP = query.GetString(4),
                };
                listas.Add(nuevaLista);
            }
        }

        return listas;
    }

    public Task<bool> ActualizarLista(Listas lista) => throw new NotImplementedException();
}
