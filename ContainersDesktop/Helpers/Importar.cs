using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace ContainersDesktop.Helpers;
public static class Importar
{
    public static async Task ImportarDatos(XamlRoot root)
    {       

        ContentDialog importarDialog = new ContentDialog
        {
            XamlRoot = root,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Atención!",
            Content = $"Se importó el archivo",
            PrimaryButtonText = "Abrir ubicación",
            CloseButtonText = "Cerrar"
        };

        await importarDialog.ShowAsync();
    }
}
