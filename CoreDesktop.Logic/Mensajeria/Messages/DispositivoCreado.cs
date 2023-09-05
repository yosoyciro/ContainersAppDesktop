using CoreDesktop.Dominio.Models.Mensajeria;

namespace ContainersDesktop.Logica.Mensajeria.Messages;
public class DispositivoCreado : Message
{
    public int DISPOSITIVOS_ID_REG
    {
        get;
        set;
    }

    public string? DISPOSITIVOS_ID_ESTADO_REG
    {
        get;
        set;
    }

    public string? DISPOSITIVOS_DESCRIP
    {
        get;
        set;
    }

    public string? DISPOSITIVOS_CONTAINER
    {
        get;
        set;
    }

    public string? DISPOSITIVOS_FECHA_ACTUALIZACION
    {
        get;
        set;
    }
}
