using System.Globalization;
using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.Logica.Specification.Implementaciones;
public class DispCalendarFechasHorasDispositivoSpec : BaseSpecification<DispCalendar>
{
    public DispCalendarFechasHorasDispositivoSpec(int idDispositivo, DateTime fechaHoy) : base(x =>
    x.DISP_CALENDAR_FECHA >= fechaHoy &&
    x.DISP_CALENDAR_ID_DISPOSITIVO == idDispositivo && 
    x.Estado == "A")
    { }    
}
