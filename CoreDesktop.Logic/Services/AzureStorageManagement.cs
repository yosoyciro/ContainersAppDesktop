using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using ContainersDesktop.Dominio.Models.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Logica.Services;
public class AzureStorageManagement
{
    private readonly AzureStorageConfig _azureStorageConfig;
    private readonly string _connectionString = string.Empty;
    private BlobClient blobClient;
    private readonly ILogger<AzureStorageManagement> _logger;

    public AzureStorageManagement(IOptions<AzureStorageConfig> azureStorageConfig, ILogger<AzureStorageManagement> logger)
    {
        _azureStorageConfig = azureStorageConfig.Value;
        _connectionString = _azureStorageConfig.ConnectionString;               
        _logger = logger;
    }

    //public async Task<string> DownloadFile(string contenedor)
    //{
    //    ArchivosCarpetas.VerificarCarpeta($"{ArchivosCarpetas.GetParentDirectory()}{_dbFolder}\\{contenedor}");
    //    _dbPathTemp = Path.Combine($"{ArchivosCarpetas.GetParentDirectory()}{_dbFolder}\\{contenedor}\\", $"{_dbFile.Substring(0, _dbFile.Length-3)}{DateTime.Now.Ticks}.db");
        
    //    try
    //    {
    //        blobClient = new BlobClient(_connectionString, contenedor, _dbNameDescarga + ".db");

    //        if (await blobClient.ExistsAsync())
    //        {
                
    //            File.Delete(_dbPathTemp);

    //            using (var stream = File.OpenWrite(_dbPathTemp))
    //            {
    //                var result = await blobClient.DownloadToAsync(stream);
    //            };

    //            await blobClient.DeleteAsync();
    //            return _dbPathTemp;
    //        }

    //        return string.Empty;
    //    }
    //    catch (SystemException ex)
    //    {
    //        _logger.LogError(ex.Message);
    //        throw;
    //    }
    //}

    public async Task<bool> UploadFile(string contenedor, string dbNameSubida, string dbSubidaFullPath)
    {        
        try
        {            
            blobClient = new BlobClient(_connectionString, contenedor, dbNameSubida);

            //Borrar antes de subir
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
            }

            using (FileStream source = File.Open(dbSubidaFullPath, FileMode.Open))
            {
                var result = await blobClient.UploadAsync(source);
            }

            return true;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
        catch (SystemException ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    //public bool ExisteContainer(string contenedor)
    //{
    //    BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

    //    var container = blobServiceClient.GetBlobContainerClient(contenedor);

    //    return container.Exists();        
    //}

    public bool ConsultarDispositivo(string container)
    {
        try
        {
            var tableClient = new TableClient(_connectionString, "Dispositivos");
            Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"RowKey eq '{container}'");

            return queryResultsFilter.Count() > 0 ? true : false;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
