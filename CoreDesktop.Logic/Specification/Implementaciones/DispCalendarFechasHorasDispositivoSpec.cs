using ContainersDesktop.Comunes.Helpers;
using CoreDesktop.Dominio.Models;

namespace ContainersDesktop.Logica.Specification.Implementaciones;
public class DispCalendarFechasHorasDispositivoSpec : BaseSpecification<DispCalendar>
{
    public DispCalendarFechasHorasDispositivoSpec(int idDispositivo) : base(x =>
    x.DISP_CALENDAR_ID_DISPOSITIVO == idDispositivo && x.Estado == "A")
    { }    
}
