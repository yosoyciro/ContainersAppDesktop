using Azure.Data.Tables;
using Azure;

namespace ContainersDesktop.Logica.Contracts;
public interface IAzureTableStorage
{
    Task<List<TEntity>> LeerTableStorage<TEntity>(string tabla, string? filter = null, string? filterValue = null)
        where TEntity : class, ITableEntity;

    bool ValidarConnectionString(string tabla, string connectionString);    

    Task<bool> UpdateTable<TEntity>(TEntity t, string tabla, ETag eTag)
        where TEntity : ITableEntity;
}
