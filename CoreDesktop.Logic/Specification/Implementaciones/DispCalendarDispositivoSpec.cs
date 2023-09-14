using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Logica.Specification.Implementaciones;
public class DispCalendarDispositivoSpec : BaseSpecification<DispCalendar>
{
    public DispCalendarDispositivoSpec(int idDispositivo) 
        : base(x => x.DISP_CALENDAR_ID_DISPOSITIVO == idDispositivo && x.Estado == "A")
    {
    }
}
