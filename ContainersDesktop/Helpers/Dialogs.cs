using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PlayFab.EconomyModels;

namespace ContainersDesktop.Helpers;
public static class Dialogs
{
    public static async Task Error(XamlRoot root, string message)
    {
        ContentDialog errorDialog = new ContentDialog
        {
            XamlRoot = root,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Error!",
            Content = $"Error {message}",
            CloseButtonText = "Cerrar",
            DefaultButton = ContentDialogButton.Close,
    };

        await errorDialog.ShowAsync();
    }
    public static async Task<ContentDialogResult> Pregunta(XamlRoot root, string message)
    {
        ContentDialog preguntaDialog = new ContentDialog
        {
            XamlRoot = root,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Pregunta!",
            Content = message,
            PrimaryButtonText = "Sí",
            CloseButtonText = "No",
            DefaultButton = ContentDialogButton.Primary,
        };

        return await preguntaDialog.ShowAsync();
    }

    public static async Task Aviso(XamlRoot root, string message)
    {
        ContentDialog avisoDialog = new ContentDialog
        {
            XamlRoot = root,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Atención!",
            CloseButtonText = "Cerrar",
            DefaultButton = ContentDialogButton.Close,
            Content = message,
        };

        await avisoDialog.ShowAsync();
    }
}
