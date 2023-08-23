using System.Diagnostics;
using System.IO;
using ContainersDesktop.Dominio.Models.Storage;

namespace ContainersDesktop.Comunes.Helpers;
public static class ArchivosCarpetas
{
    public static bool VerificarCarpeta(string folder)
    {
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
            return false;
        }

        return true;
    }

    public static DirectoryInfo? GetParentDirectory()
    {
        return Directory.GetParent(Path.GetDirectoryName(typeof(Settings).Assembly.Location));
    }

    public static void AbrirUbicacion(string folder)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            Arguments = folder,
            FileName = "explorer.exe",

        };
        Process.Start(startInfo);
    }
}
