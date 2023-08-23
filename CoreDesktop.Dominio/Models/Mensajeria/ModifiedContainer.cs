using ContainersDesktop.Dominio.Models;

namespace CoreDesktop.Dominio.Models.Mensajeria;
public class ModifiedContainer : Command
{
    public int OBJ_ID_REG
    {
        get; private set;
    }

    public int OBJ_SIGLAS
    {
        get;
        private set;
    }

    public int OBJ_SIGLAS_LISTA
    {    
        get; private set; 
    }

    public ModifiedContainer(Objetos objeto)
    {
        OBJ_ID_REG = objeto.OBJ_ID_REG;
        OBJ_SIGLAS = objeto.OBJ_SIGLAS;
        OBJ_SIGLAS_LISTA = objeto.OBJ_SIGLAS_LISTA;
    }
}
