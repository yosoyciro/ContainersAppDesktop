using System.ComponentModel.DataAnnotations;

namespace ContainersDesktop.Core.Models;
public class Objetos
{
    [Key]
    public int OBJ_ID_REG
    {
        get; set;
    }
    public string? OBJ_MATRICULA
    {
        get; set;
    }

    public string? OBJ_ID_ESTADO_REG
    {
        get;
        set;
    }
    public int OBJ_SIGLAS_LISTA
    {
        get; set;
    }
    public int OBJ_SIGLAS
    {
        get; set;
    }
    public int OBJ_MODELO_LISTA
    {
        get; set;
    }
    public int OBJ_MODELO
    {
        get; set;
    }

    public int OBJ_ID_OBJETO
    {
        get;
        set;
    }
    public int OBJ_VARIANTE_LISTA
    {
        get; set;
    }
    public int OBJ_VARIANTE
    {
        get; set;
    }
    public int OBJ_TIPO_LISTA
    {
        get; set;
    }
    public int OBJ_TIPO
    {
        get; set;
    }
    public string? OBJ_INSPEC_CSC
    {
        get; set;
    }
    public int OBJ_PROPIETARIO_LISTA
    {
        get; set;
    }
    public int OBJ_PROPIETARIO
    {
        get; set;
    }
    public int OBJ_TARA_LISTA
    {
        get; set;
    }
    public int OBJ_TARA
    {
        get; set;
    }
    public int OBJ_PMP_LISTA
    {
        get;
        set;
    }
    public int OBJ_PMP
    {
        get; set;
    }
    public int OBJ_CARGA_UTIL
    {
        get; set;
    }
    public int OBJ_ALTURA_EXTERIOR_LISTA
    {
        get; set;
    }
    public int OBJ_ALTURA_EXTERIOR
    {
        get; set;
    }
    public int OBJ_CUELLO_CISNE_LISTA
    {
        get; set;
    }
    public int OBJ_CUELLO_CISNE
    {
        get; set;
    }
    public int OBJ_BARRAS_LISTA
    {
        get; set;
    }
    public int OBJ_BARRAS
    {
        get; set;
    }
    public int OBJ_CABLES_LISTA
    {
        get; set;
    }
    public int OBJ_CABLES
    {
        get; set;
    }
    public int OBJ_LINEA_VIDA_LISTA
    {
        get; set;
    }
    public int OBJ_LINEA_VIDA
    {
        get; set;
    }
    public string? OBJ_OBSERVACIONES
    {
        get; set;
    }
    public string? OBJ_FECHA_ACTUALIZACION
    {
        get;
        set;
    }
    public string? OBJ_COLOR
    {
        get; set;
    }
}
