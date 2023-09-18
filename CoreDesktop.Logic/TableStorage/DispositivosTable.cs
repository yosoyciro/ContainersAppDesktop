using Azure;
using Azure.Data.Tables;

namespace ContainersDesktop.Logica.TableStorage;
public class DispositivosTable : ITableEntity
{
    public string Movil
    {
        get; set;
    }
    public string Usuario
    {
        get; set;
    }
    public string PartitionKey
    {
        get; set;
    }
    public string RowKey
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
