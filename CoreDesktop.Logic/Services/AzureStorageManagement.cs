using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Logica.Services;
public class AzureStorageManagement
{
    private readonly AzureStorageConfig _azureStorageConfig;
    //private readonly Settings _settings;
    private readonly string _connectionString = string.Empty;
    private readonly string _cuenta = string.Empty;
    private BlobClient blobClient;
    private readonly string _dbNameDescarga = string.Empty;
    private readonly string _dbNameSubida = string.Empty;
    private readonly string _dbFile = string.Empty;
    private readonly string _dbFullPath = string.Empty;
    private readonly string _dbFolder = string.Empty;
    private string _dbPathTemp = string.Empty;
    private readonly ILogger<AzureStorageManagement> _logger;

    public AzureStorageManagement(IOptions<AzureStorageConfig> azureStorageConfig, IOptions<Settings> settings, ILogger<AzureStorageManagement> logger)
    {
        _azureStorageConfig = azureStorageConfig.Value;
        //_settings = settings.Value;
        _connectionString = _azureStorageConfig.ConnectionString;
        _cuenta = _azureStorageConfig.Cuenta;
        _dbNameDescarga = settings.Value.DBNameDescarga;
        _dbNameSubida = settings.Value.DBNameSubida;
        _dbFolder = settings.Value.DBFolder;
        _dbFile = settings.Value.DBName;
        _dbFullPath = $"{ArchivosCarpetas.GetParentDirectory()}{_dbFolder}\\{_dbFile}";
        _logger = logger;
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
            _logger.LogError(ex.Message);
            throw;
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

            using (FileStream source = File.Open(_dbFullPath, FileMode.Open))
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

    public bool ExisteContainer(string contenedor)
    {
        BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

        var container = blobServiceClient.GetBlobContainerClient(contenedor);

        return container.Exists();        
    }
}
