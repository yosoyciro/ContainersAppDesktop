using Azure;
using Azure.Storage.Blobs;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models.Storage;
using ContainersDesktop.Models.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Core.Services;
public class AzureStorageManagement
{
    private readonly AzureStorageConfig _azureStorageConfig;
    private readonly Settings _settings;
    private readonly string _connectionString = string.Empty;
    private readonly string _cuenta = string.Empty;
    private BlobClient blobClient;
    private readonly string _dbNameDescarga = string.Empty;
    private readonly string _dbNameSubida = string.Empty;
    private readonly string _dbFile = string.Empty;
    private readonly string _dbFullPath = string.Empty;
    private readonly string _dbFolder = string.Empty;
    private string _dbPathTemp = string.Empty;

    public AzureStorageManagement(IOptions<AzureStorageConfig> azureStorageConfig, IOptions<Settings> settings)
    {
        _azureStorageConfig = azureStorageConfig.Value;
        _settings = settings.Value;
        _connectionString = _azureStorageConfig.ConnectionString;
        _cuenta = _azureStorageConfig.Cuenta;
        _dbNameDescarga = _settings.DBNameDescarga;
        _dbNameSubida = _settings.DBNameSubida;
        _dbFolder = _settings.DBFolder;
        _dbFile = settings.Value.DBName;
        _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFolder}{_dbFile}";
    }

    public async Task<string> DownloadFile(string contenedor)
    {
        ArchivosCarpetas.VerificarCarpeta($"{ArchivosCarpetas.GetParentDirectory()}{_dbFolder}\\{contenedor}");
        _dbPathTemp = Path.Combine($"{ArchivosCarpetas.GetParentDirectory()}{_dbFolder}\\{contenedor}\\", $"{_dbFile.Substring(0, _dbFile.Length-3)}{DateTime.Now.Ticks}.db");
        
        try
        {
            blobClient = new BlobClient(_connectionString, contenedor, _dbNameDescarga + ".db");

            if (await blobClient.ExistsAsync())
            {
                
                File.Delete(_dbPathTemp);

                using (var stream = File.OpenWrite(_dbPathTemp))
                {
                    var result = await blobClient.DownloadToAsync(stream);
                };

                await blobClient.DeleteAsync();
                return _dbPathTemp;
            }

            return string.Empty;
        }
        catch (SystemException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> UploadFile(string contenedor)
    {        
        try
        {
            blobClient = new BlobClient(_connectionString, contenedor, _dbNameSubida);

            //Borrar antes de subir
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
            }

            using (FileStream source = File.Open(_dbFullPath, FileMode.OpenOrCreate))
            {
                var result = await blobClient.UploadAsync(source);
            }
            return true;
        }
        catch (RequestFailedException)
        {
            throw;
        }
        catch (SystemException)
        {
            throw;
        }
    }

    public bool ExisteContainer(string contenedor)
    {
        BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

        var container = blobServiceClient.GetBlobContainerClient(contenedor);

        return container.Exists();        
    }
}
