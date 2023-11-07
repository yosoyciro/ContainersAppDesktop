using Azure.Data.Tables;
using Azure;

namespace ContainersDesktop.Logica.Services.ModelosStorage;
public class Modulos : ITableEntity
{
    public string? Nombre
    {
        get; set;
    }
    public string? RutaAcceso
    {
        get; set;
    }
    public string? VersionActual
    {
        get;
        set;
    }
    public string? Icono
    {
        get;
        set;
    }
    public int Orden
    {
        get;
        set;
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
