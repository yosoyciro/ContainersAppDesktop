namespace ContainersDesktop.Comunes.Helpers;
public static class Colores
{
    public static string ColorToHex(Windows.UI.Color color)
    {
        var r = color.R.ToString("X2");
        var g = color.G.ToString("X2");
        var b = color.B.ToString("X2");
        var a = color.A.ToString("X2");

        string hex = string.Format("#{0}{1}{2}{3}", a, r, g, b);
        return hex;
    }

    //public static string ColorToHex(System.Drawing.Color color)
    //{
    //    var r = color.R.ToString("X2");
    //    var g = color.G.ToString("X2");
    //    var b = color.B.ToString("X2");

    //    string hex = string.Format("#{0}{1}{2}", r, g, b);
    //    return hex;
    //}

    public static Windows.UI.Color HexToColor(string hex)
    {
        //hex = hex.Replace("#", ""); // Eliminar el símbolo '#' si está presente
        var a = (byte)255; // byte.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
        var r = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        var g = byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
        var b = byte.Parse(hex.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);

        // Opcional: Si deseas especificar un valor alfa (transparencia)
        

        return Windows.UI.Color.FromArgb(a, r, g, b);
    }

    //public static System.Drawing.Color HexToColor2(string hex)
    //{
    //    //hex = hex.Replace("#", ""); // Eliminar el símbolo '#' si está presente
    //    var r = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
    //    var g = byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
    //    var b = byte.Parse(hex.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);

    //    // Opcional: Si deseas especificar un valor alfa (transparencia)
    //    byte a = 255; // Valor alfa máximo (no transparente)

    //    return System.Drawing.Color.FromArgb(a, r, g, b);
    //}
}
