using ContainersDesktop.Dominio.Models;
using CoreDesktop.Dominio.Models.Mensajeria;

namespace ContainersDesktop.Logica.Mensajeria.Messages;
public class MovimCreado : Message
{
    public int MOVIM_ID_REG
    {
        get; set;
    }
    public string MOVIM_ID_ESTADO_REG
    {
        get; set;
    }
    public string? MOVIM_FECHA
    {
        get; set;
    }
    public int MOVIM_ID_OBJETO
    {
        get; set;
    }
    public int MOVIM_TIPO_MOVIM_LISTA
    {
        get; set;
    }
    public int MOVIM_TIPO_MOVIM
    {
        get; set;
    }
    public int MOVIM_PESO_LISTA
    {
        get; set;
    }
    public int MOVIM_PESO
    {
        get; set;
    }
    public int MOVIM_TRANSPORTISTA_LISTA
    {
        get; set;
    }
    public int MOVIM_TRANSPORTISTA
    {
        get; set;
    }
    public int MOVIM_CLIENTE_LISTA
    {
        get; set;
    }
    public int MOVIM_CLIENTE
    {
        get; set;
    }
    public int MOVIM_CHOFER_LISTA
    {
        get; set;
    }
    public int MOVIM_CHOFER
    {
        get; set;
    }
    public string? MOVIM_CAMION_ID
    {
        get; set;
    }
    public string? MOVIM_REMOLQUE_ID
    {
        get; set;
    }
    public string? MOVIM_ALBARAN_ID
    {
        get; set;
    }
    public string? MOVIM_OBSERVACIONES
    {
        get; set;
    }
    public int MOVIM_ENTRADA_SALIDA_LISTA
    {
        get; set;
    }
    public int MOVIM_ENTRADA_SALIDA
    {
        get; set;
    }
    public int MOVIM_ALMACEN_LISTA
    {
        get; set;
    }
    public int MOVIM_ALMACEN
    {
        get; set;
    }
    public string? MOVIM_PDF
    {
        get; set;
    }
    public string MOVIM_FECHA_ACTUALIZACION
    {
        get; set;
    }
    public int MOVIM_TAREA_PROGRAMADA_ID_REG
    {
        get; set;
    }
    public double MOVIM_DISPOSITIVO_LATITUD
    {
        get; set;
    }
    public double MOVIM_DISPOSITIVO_LONGITUD
    {
        get; set;
    }

    //public MovimCreado(Movim movim)
    //{
    //    MOVIM_ID_REG = movim.ID;
    //    MOVIM_ID_ESTADO_REG = movim.MOVIM_ID_ESTADO_REG;
    //    MOVIM_FECHA = movim.MOVIM_FECHA;
    //    MOVIM_ID_OBJETO = movim.MOVIM_ID_OBJETO;
    //    MOVIM_TIPO_MOVIM_LISTA = movim.MOVIM_TIPO_MOVIM_LISTA;
    //    MOVIM_TIPO_MOVIM = movim.MOVIM_TIPO_MOVIM;
    //    MOVIM_PESO_LISTA = movim.MOVIM_PESO_LISTA;
    //    MOVIM_PESO = movim.MOVIM_PESO;
    //    MOVIM_TRANSPORTISTA_LISTA = movim.MOVIM_TRANSPORTISTA_LISTA;
    //    MOVIM_TRANSPORTISTA = movim.MOVIM_TRANSPORTISTA;
    //    MOVIM_CLIENTE_LISTA = movim.MOVIM_CLIENTE_LISTA;
    //    MOVIM_CLIENTE = movim.MOVIM_CLIENTE;
    //    MOVIM_CHOFER_LISTA = movim.MOVIM_CHOFER_LISTA;
    //    MOVIM_CHOFER = movim.MOVIM_CHOFER;
    //    MOVIM_CAMION_ID = movim.MOVIM_CAMION_ID;
    //    MOVIM_REMOLQUE_ID = movim.MOVIM_REMOLQUE_ID;
    //    MOVIM_ALBARAN_ID = movim.MOVIM_ALBARAN_ID;
    //    MOVIM_OBSERVACIONES = movim.MOVIM_OBSERVACIONES;
    //    MOVIM_ENTRADA_SALIDA_LISTA = movim.MOVIM_ENTRADA_SALIDA_LISTA;
    //    MOVIM_ENTRADA_SALIDA = movim.MOVIM_ENTRADA_SALIDA;
    //    MOVIM_ALMACEN_LISTA = movim.MOVIM_ALMACEN_LISTA;
    //    MOVIM_ALMACEN = movim.MOVIM_ALMACEN;
    //    MOVIM_PDF = movim.MOVIM_PDF;
    //    MOVIM_FECHA_ACTUALIZACION = movim.MOVIM_FECHA_ACTUALIZACION;
    //    MOVIM_TAREA_PROGRAMADA_ID_REG = movim.MOVIM_TAREA_PROGRAMADA_ID_REG;
    //    MOVIM_DISPOSITIVO_LATITUD = movim.MOVIM_DISPOSITIVO_LATITUD;
    //    MOVIM_DISPOSITIVO_LONGITUD = movim.MOVIM_DISPOSITIVO_LONGITUD;
    //}
}
