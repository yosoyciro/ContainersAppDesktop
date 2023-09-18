using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Logica.Specification.Implementaciones;
public class TareasProgramadasArchivosSpec : BaseSpecification<TareaProgramadaArchivo> 
{
    public TareasProgramadasArchivosSpec(int tareaProgramadaIdReg) : base(x =>
        x.TAREAS_PROGRAMADAS_ARCHIVOS_TAREAS_PROGRAMADAS_ID_REG == tareaProgramadaIdReg
    )
    {  }
}
