namespace ContainersDesktop.Core.Helpers;
public static class FormatoFecha
{
    public static string ConvertirAFechaCorta(string fechaEstandar)
    {
        var fecha = Convert.ToDateTime(fechaEstandar);
        return fecha.Date.ToShortDateString();
    }

    public static string FechaEstandar(DateTime fecha = new DateTime())
    {
        return fecha.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
