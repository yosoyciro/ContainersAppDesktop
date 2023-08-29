using System;
using System.Globalization;
using System.Threading;

namespace ContainersDesktop.Comunes.Helpers;
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
        return fecha.ToString("yyyy-MM-ddTHH:mm:ss");
    }

    public static string ConvertirAFechaHora(string fechaEstandar)
    {
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        var fechaCulture = Convert.ToDateTime(fechaEstandar);
        var fecha = fechaCulture.ToString(currentCulture.DateTimeFormat.ShortDatePattern);
        var hora = fechaCulture.ToString(currentCulture.DateTimeFormat.ShortTimePattern);

        return $"{fecha} {hora}";
    }
}
