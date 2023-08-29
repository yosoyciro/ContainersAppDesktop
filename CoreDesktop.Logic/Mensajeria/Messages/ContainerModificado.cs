using ContainersDesktop.Dominio.Models;
using CoreDesktop.Dominio.Models.Mensajeria;

namespace CoreDesktopLogica.Mensajeria.Messages;
public class ContainerModificado : Message
{
    public int OBJ_ID_REG
    {
        get; set;
    }

    public int OBJ_SIGLAS
    {
        get;
        set;
    }

    public int OBJ_SIGLAS_LISTA
    {    
        get; set; 
    }

    public ContainerModificado(int OBJ_ID_REG, int OBJ_SIGLAS, int OBJ_SIGLAS_LISTA)
    {
        this.OBJ_ID_REG = OBJ_ID_REG;
        this.OBJ_SIGLAS = OBJ_SIGLAS;
        this.OBJ_SIGLAS_LISTA = OBJ_SIGLAS_LISTA;
    }
}
