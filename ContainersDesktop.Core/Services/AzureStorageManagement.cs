//using Azure.Storage.Blobs;
using Azure.Storage.Blobs;
using ContainersDesktop.Models.Storage;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;
using Windows.Storage;

namespace ContainersDesktop.Core.Services;
public class AzureStorageManagement
{
    private readonly string connectionString = string.Empty;
    private readonly string cuenta = string.Empty;
    private BlobClient blobClient;
    private readonly string _dbName = string.Empty;
    private readonly string _dbNameDescarga = string.Empty;
    private readonly string _dbNameSubida = string.Empty;
    private string dbPath = string.Empty;

    public AzureStorageManagement(IConfiguration configuration)
    {
        connectionString = configuration.GetSection("AzureStorageConfig").GetRequiredSection("ConnectionString").Value;
        cuenta = configuration.GetSection("AzureStorageConfig").GetRequiredSection("Cuenta").Value;
        _dbNameDescarga = configuration.GetSection("Settings").GetRequiredSection("DBNameDescarga").Value;
        _dbNameSubida = configuration.GetSection("Settings").GetRequiredSection("DBNameSubida").Value;
        _dbName = configuration.GetSection("Settings").GetRequiredSection("DBName").Value;
    }

    public async Task<string> DownloadFile(string contenedor)
    {
        dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _dbNameDescarga + contenedor + DateTime.Now.Ticks + ".db");
        blobClient = new BlobClient(connectionString, contenedor, _dbNameDescarga + ".db");
        try
        {
            if (await blobClient.ExistsAsync())
            {
                File.Delete(dbPath);

                using (var stream = File.OpenWrite(dbPath))
                {
                    var result = await blobClient.DownloadToAsync(stream);
                };

                await blobClient.DeleteAsync();
                return dbPath;
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
        dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _dbName);
        blobClient = new BlobClient(connectionString, contenedor, _dbNameSubida);
        try
        {
            //Borrar antes de subir
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
            }

            using (FileStream source = File.Open(dbPath, FileMode.OpenOrCreate))
            //using (var memoryStream = new MemoryStream(File.OpenRead(_dbPath))
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
