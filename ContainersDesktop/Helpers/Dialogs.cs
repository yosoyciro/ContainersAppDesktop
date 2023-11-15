using ContainersDesktop.Comunes.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
            CloseButtonText = Constantes.errorDialog_CloseButtonText.GetLocalized(),
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
            Title = Constantes.preguntaDialog_Title.GetLocalized(),
            Content = message,
            PrimaryButtonText = Constantes.preguntaDialog_PrimaryButtonText.GetLocalized(),
            CloseButtonText = Constantes.preguntaDialog_CloseButtonText.GetLocalized(),
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
            Title = Constantes.avisoDialog_Title.GetLocalized(),
            CloseButtonText = Constantes.avisoDialog_CloseButtonText.GetLocalized(),
            DefaultButton = ContentDialogButton.Close,
            Content = message,
        };

        await avisoDialog.ShowAsync();
    }
}
