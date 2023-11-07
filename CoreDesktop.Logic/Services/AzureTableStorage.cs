using Azure.Data.Tables;
using Azure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ContainersDesktop.Logica.Contracts;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models.Storage;

namespace ContainersDesktop.Logica.Services;
public class AzureTableStorage : IAzureTableStorage
{
    private readonly ILogger<AzureTableStorage> _logger;
    private readonly string _connectionString = string.Empty;

    public AzureTableStorage(ILogger<AzureTableStorage> logger, IOptions<AzureStorageMeribia> options)
    {
        _logger = logger;
        _connectionString = options.Value.ConnectionString!;
    }

    private TableClient GetTableClient(string tableName, string? connectionString = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception(Constantes.UI_AzureConnectionNoDefinida.GetLocalized());
            }
            var tableServiceClient = new TableServiceClient(_connectionString);

            var tableClient = tableServiceClient.GetTableClient(tableName: tableName);

            //await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<List<TEntity>> LeerTableStorage<TEntity>(string tabla, string? filter = null, string? filterValue = null) where TEntity : class, ITableEntity
    {
        try
        {
            var tableClient = GetTableClient(tabla);
            var queryResultsFilter = !string.IsNullOrEmpty(filter) ? tableClient.QueryAsync<TEntity>(filter: $"{filter} eq '{filterValue}'") : tableClient.QueryAsync<TEntity>();

            return await queryResultsFilter.ToListAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool ValidarConnectionString(string tabla, string connectionString)
    {
        var tableClient = GetTableClient(tabla);

        return tableClient == null ? false : true;
    }
    
    public async Task<bool> UpdateTable<TEntity>(TEntity t, string tabla, ETag eTag) where TEntity : ITableEntity
    {
        try
        {
            var tableClient = GetTableClient(tabla);

            var response = await tableClient.UpdateEntityAsync(t, eTag);

            return !response.IsError;
        }
        catch (Exception)
        {

            throw;
        }

    }
}
