using Azure.Data.Tables;
using Azure;

namespace ContainersDesktop.Logica.Services.ModelosStorage;
public class SubModulos : ITableEntity
{
    public string? Descripcion
    {
        get; set;
    }
    public string? Orden
    {
        get; set;
    }
    public string? RutaSubModulos
    {
        get; set;
    }
    public string? VersionActual
    {
        get; set;
    }
    public string? Dashboard
    {
        get; set;
    }
    public string? Icono
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
