using Azure.Storage.Blobs;
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
    private readonly string _dbName = string.Empty;
    private readonly string _dbNameDescarga = string.Empty;
    private readonly string _dbNameSubida = string.Empty;
    private readonly string _dbPath = string.Empty;
    private string _dbPathTemp = string.Empty;

    public AzureStorageManagement(IOptions<AzureStorageConfig> azureStorageConfig, IOptions<Settings> settings)
    {
        _azureStorageConfig = azureStorageConfig.Value;
        _settings = settings.Value;
        _connectionString = _azureStorageConfig.ConnectionString; //configuration.GetSection("AzureStorageConfig").GetRequiredSection("ConnectionString").Value;
        _cuenta = _azureStorageConfig.Cuenta; //configuration.GetSection("AzureStorageConfig").GetRequiredSection("Cuenta").Value;
        _dbNameDescarga = _settings.DBNameDescarga; //configuration.GetSection("Settings").GetRequiredSection("DBNameDescarga").Value;
        _dbNameSubida = _settings.DBNameSubida; //configuration.GetSection("Settings").GetRequiredSection("DBNameSubida").Value;
        _dbName = _settings.DBName; //.GetSection("Settings").GetRequiredSection("DBName").Value;
        _dbPath = Path.Combine(_settings.DBPath, _settings.DBName);
    }

    public async Task<string> DownloadFile(string contenedor)
    {
        _dbPathTemp = Path.Combine(_dbPath, _dbNameDescarga + contenedor + DateTime.Now.Ticks + ".db");
        blobClient = new BlobClient(_connectionString, contenedor, _dbNameDescarga + ".db");
        try
        {
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> UploadFile(string contenedor)
    {
        //dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _dbName);
        blobClient = new BlobClient(_connectionString, contenedor, _dbNameSubida);
        try
        {
            //Borrar antes de subir
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
            }

            using (FileStream source = File.Open(_dbPath, FileMode.OpenOrCreate))
            {
                var result = await blobClient.UploadAsync(source);
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
