using ContainersDesktop.Dominio.Models;
using CoreDesktop.Dominio.Models.Mensajeria;

namespace CoreDesktopLogica.Mensajeria.Messages;
public class ContainerCreado : Message
{
    public int OBJ_ID_REG
    {
        get; private set;
    }
    public string? OBJ_MATRICULA
    {
        get; private set;
    }

    public string? OBJ_ID_ESTADO_REG
    {
        get;
        private set;
    }
    public int OBJ_SIGLAS_LISTA
    {
        get; private set;
    }
    public int OBJ_SIGLAS
    {
        get; private set;
    }
    public int OBJ_MODELO_LISTA
    {
        get; private set;
    }
    public int OBJ_MODELO
    {
        get; private set;
    }

    public int OBJ_ID_OBJETO
    {
        get;
        private set;
    }
    public int OBJ_VARIANTE_LISTA
    {
        get; private set;
    }
    public int OBJ_VARIANTE
    {
        get; private set;
    }
    public int OBJ_TIPO_LISTA
    {
        get; private set;
    }
    public int OBJ_TIPO
    {
        get; private set;
    }
    public string? OBJ_INSPEC_CSC
    {
        get; private set;
    }
    public int OBJ_PROPIETARIO_LISTA
    {
        get; private set;
    }
    public int OBJ_PROPIETARIO
    {
        get; private set;
    }
    public int OBJ_TARA_LISTA
    {
        get; private set;
    }
    public int OBJ_TARA
    {
        get; private set;
    }
    public int OBJ_PMP_LISTA
    {
        get;
        private set;
    }
    public int OBJ_PMP
    {
        get; private set;
    }
    public int OBJ_CARGA_UTIL
    {
        get; private set;
    }
    public int OBJ_ALTURA_EXTERIOR_LISTA
    {
        get; private set;
    }
    public int OBJ_ALTURA_EXTERIOR
    {
        get; private set;
    }
    public int OBJ_CUELLO_CISNE_LISTA
    {
        get; private set;
    }
    public int OBJ_CUELLO_CISNE
    {
        get; private set;
    }
    public int OBJ_BARRAS_LISTA
    {
        get; private set;
    }
    public int OBJ_BARRAS
    {
        get; private set;
    }
    public int OBJ_CABLES_LISTA
    {
        get; private set;
    }
    public int OBJ_CABLES
    {
        get; private set;
    }
    public int OBJ_LINEA_VIDA_LISTA
    {
        get; private set;
    }
    public int OBJ_LINEA_VIDA
    {
        get; private set;
    }
    public string? OBJ_OBSERVACIONES
    {
        get; private set;
    }
    public string? OBJ_FECHA_ACTUALIZACION
    {
        get;
        private set;
    }
    public string? OBJ_COLOR
    {
        get; private set;
    }

    public ContainerCreado(Objeto objeto)
    {
        OBJ_ID_REG = objeto.ID;
        OBJ_MATRICULA = objeto.OBJ_MATRICULA;
        OBJ_ID_ESTADO_REG = objeto.OBJ_ID_ESTADO_REG;
        OBJ_SIGLAS_LISTA = objeto.OBJ_SIGLAS_LISTA;
        OBJ_SIGLAS = objeto.OBJ_SIGLAS;
        OBJ_MODELO_LISTA = objeto.OBJ_MODELO_LISTA;
        OBJ_MODELO = objeto.OBJ_MODELO;
        OBJ_ID_OBJETO = objeto.OBJ_ID_OBJETO;
        OBJ_VARIANTE_LISTA = objeto.OBJ_VARIANTE_LISTA;
        OBJ_VARIANTE = objeto.OBJ_VARIANTE;
        OBJ_TIPO_LISTA = objeto.OBJ_TIPO_LISTA;
        OBJ_TIPO = objeto.OBJ_TIPO;
        OBJ_INSPEC_CSC = objeto.OBJ_INSPEC_CSC;
        OBJ_PROPIETARIO_LISTA = objeto.OBJ_PROPIETARIO_LISTA;
        OBJ_PROPIETARIO = objeto.OBJ_PROPIETARIO;
        OBJ_TARA_LISTA = objeto.OBJ_TARA_LISTA;
        OBJ_TARA = objeto.OBJ_TARA;
        OBJ_PMP_LISTA = objeto.OBJ_PMP_LISTA;
        OBJ_PMP = objeto.OBJ_PMP;
        OBJ_CARGA_UTIL = objeto.OBJ_CARGA_UTIL;
        OBJ_ALTURA_EXTERIOR_LISTA = objeto.OBJ_ALTURA_EXTERIOR_LISTA;
        OBJ_ALTURA_EXTERIOR = objeto.OBJ_ALTURA_EXTERIOR;
        OBJ_CUELLO_CISNE_LISTA = objeto.OBJ_CUELLO_CISNE_LISTA;
        OBJ_CUELLO_CISNE = objeto.OBJ_CUELLO_CISNE;
        OBJ_BARRAS_LISTA = objeto.OBJ_BARRAS_LISTA;
        OBJ_BARRAS = objeto.OBJ_BARRAS;
        OBJ_CABLES_LISTA = objeto.OBJ_CABLES_LISTA;
        OBJ_CABLES = objeto.OBJ_CABLES;
        OBJ_LINEA_VIDA_LISTA = objeto.OBJ_LINEA_VIDA_LISTA;
        OBJ_LINEA_VIDA = objeto.OBJ_LINEA_VIDA;
        OBJ_OBSERVACIONES = objeto.OBJ_OBSERVACIONES;
        OBJ_FECHA_ACTUALIZACION = objeto.OBJ_FECHA_ACTUALIZACION;
        OBJ_COLOR = objeto.OBJ_COLOR;
    }
}
