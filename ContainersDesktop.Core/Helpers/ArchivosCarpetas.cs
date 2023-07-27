using ContainersDesktop.Core.Models.Storage;

namespace ContainersDesktop.Core.Helpers;
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

    public static DirectoryInfo? GetFullPath()
    {
        return Directory.GetParent(Path.GetDirectoryName(typeof(Settings).Assembly.Location));
    }
}
