using Azure.Data.Tables;
using Azure;

namespace ContainersDesktop.Logica.Services.ModelosStorage;
public class SubModulosUsuarios : ITableEntity
{
    public string? SubModuloId
    {
        get; set;
    }
    public string? VersionInstalada
    {
        get; set;
    }
    public bool ActualizacionAutorizada
    {
        get; set;
    }
    public string? VersionAutorizada
    {
        get; set;
    }    
    public string? PartitionKey
    {
        get; set;
    }
    public string? RowKey
    {
        get; set;
    }
    public DateTimeOffset? Timestamp
    {
        get; set;
    }
    public ETag ETag
    {
        get; set;
    }
}
