using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Windows.Input;

namespace ContainersDesktop.Core.Helpers;
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
