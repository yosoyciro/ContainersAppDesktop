using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContainersDesktop.Core.Helpers;
public static class FormatoFecha
{
    public static string ConvertirAFechaCorta(string fechaEstandar)
    {
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        var fecha = Convert.ToDateTime(fechaEstandar);
        return fecha.Date.ToString(currentCulture.DateTimeFormat.ShortDatePattern);
    }

    public static string FechaEstandar(DateTime fecha)
    {
        //if (fecha.TimeOfDay == TimeSpan.Zero)
        //{
        //    TimeSpan hora = new TimeSpan(0, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //    DateTime fechaYHora = fecha.Add(hora);
        //    return fechaYHora.ToString("yyyy-MM-ddTHH:mm:ss");
        //}
        //else
        //{
        return fecha.ToString("yyyy-MM-ddTHH:mm:ss");
        //}
    }

    public static string ConvertirAFechaHora(string fechaEstandar)
    {
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        var fecha = Convert.ToDateTime(fechaEstandar);
        return fecha.ToString(currentCulture.DateTimeFormat);
    }
}
