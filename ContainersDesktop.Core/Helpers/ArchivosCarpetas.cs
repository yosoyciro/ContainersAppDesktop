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
}
