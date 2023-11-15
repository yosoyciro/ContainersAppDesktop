namespace ContainersDesktop.Logica.Contracts;
public interface IFileShareService
{
    public Task FileDownloadAsync(string fileShareName, string fileName, string filePath);
}
