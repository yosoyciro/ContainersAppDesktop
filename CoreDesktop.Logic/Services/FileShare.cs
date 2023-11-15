using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using ContainersDesktop.Dominio.Models.Storage;
using ContainersDesktop.Logica.Contracts;
using Microsoft.Extensions.Options;

namespace ContainersDesktop.Logica.Services;
public class FileShareService : IFileShareService
{
    private readonly IOptions<InfoModulo> infoModulo;
    private readonly string _connectionString;

    public FileShareService(IOptions<AzureStorageMeribia> options, IOptions<InfoModulo> infoModulo)
    {
        _connectionString = options.Value.ConnectionString!;
        this.infoModulo = infoModulo;
    }
    public async Task FileDownloadAsync(string fileShareName, string fileName, string filePath)
    {
        ShareClient share = new ShareClient(_connectionString, fileShareName);
        ShareDirectoryClient directory = share.GetDirectoryClient(infoModulo.Value.RowKey);
        ShareFileClient file = directory.GetFileClient(fileName);

        // Download the file
        var filePathDownload = Path.Combine(filePath, fileName);
        ShareFileDownloadInfo download = file.Download();
        using (FileStream stream = File.OpenWrite(filePathDownload))
        {
            await download.Content.CopyToAsync(stream);
        }
    }
}
