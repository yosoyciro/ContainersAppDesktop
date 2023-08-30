using System.ComponentModel.DataAnnotations;
using ContainersDesktop.Dominio.Models.Base;

namespace ContainersDesktop.Dominio.Models;
public class Movim : BaseEntity
{
    public int MOVIM_ID_REG_MOBILE
    {
        get;
        set;
    }

    public int MOVIM_ID_DISPOSITIVO
    {
        get;
        set;
    }

    public string? MOVIM_ID_ESTADO_REG
    {
        get;
        set;
    }

    public string? MOVIM_FECHA
    {
        get;
        set;
    }

    public int MOVIM_ID_OBJETO
    {
        get;
        set;
    }

    public int MOVIM_TIPO_MOVIM_LISTA
    {
        get;
        set;
    }

    public int MOVIM_TIPO_MOVIM
    {
        get;
        set;
    }

    public int MOVIM_PESO_LISTA
    {
        get;
        set;
    }

    public int MOVIM_PESO
    {
        get;
        set;
    }

    public int MOVIM_TRANSPORTISTA_LISTA
    {
        get;
        set;
    }

    public int MOVIM_TRANSPORTISTA
    {
        get;
        set;
    }

    public int MOVIM_CLIENTE_LISTA
    {
        get;
        set;
    }

    public int MOVIM_CLIENTE
    {
        get;
        set;
    }

    public int MOVIM_CHOFER_LISTA
    {
        get;
        set;
    }

    public int MOVIM_CHOFER
    {
        get;
        set;
    }

    public string? MOVIM_CAMION_ID
    {
        get;
        set;
    }

    public string? MOVIM_REMOLQUE_ID
    {
        get;
        set;
    }

    public string? MOVIM_ALBARAN_ID
    {
        get;
        set;
    }

    public string? MOVIM_OBSERVACIONES
    {
        get;
        set;
    }

    public int MOVIM_ENTRADA_SALIDA_LISTA
    {
        get;
        set;
    }

    public int MOVIM_ENTRADA_SALIDA
    {
        get;
        set;
    }

    public int MOVIM_ALMACEN_LISTA
    {
        get;
        set;
    }

    public int MOVIM_ALMACEN
    {
        get;
        set;
    }

    public string? MOVIM_PDF
    {
        get;
        set;
    }

    public string? MOVIM_FECHA_ACTUALIZACION
    {
        get;
        set;
    }

    public int MOVIM_TAREA_PROGRAMADA_ID_REG
    {
        get;
        set;
    }

    public double MOVIM_DISPOSITIVO_LATITUD
    {
        get;
        set;
    }

    public double MOVIM_DISPOSITIVO_LONGITUD
    {
        get;
        set;
    }
}
