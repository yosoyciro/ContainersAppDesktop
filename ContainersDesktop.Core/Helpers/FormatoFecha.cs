using System.Globalization;

namespace ContainersDesktop.Core.Helpers;
public static class FormatoFecha
{
    public static string ConvertirAFechaCorta(string fechaEstandar)
    {
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        var fecha = Convert.ToDateTime(fechaEstandar);
        return fecha.Date.ToString(currentCulture.DateTimeFormat.ShortDatePattern);
    }

    public static string FechaEstandar(DateTime fecha = new DateTime())
    {
        return fecha.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
