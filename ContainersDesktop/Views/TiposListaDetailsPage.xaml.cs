using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;
public sealed partial class TiposListaDetailsPage : Page
{
    public TiposListaDetailsViewModel ViewModel
    {
        get;
    }
    public TiposListaDetailsPage()
    {
        ViewModel = App.GetService<TiposListaDetailsViewModel>();
        this.InitializeComponent();
    }

    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.QueryText != "")
        {
            ClaListListView.ItemsSource = ViewModel.ApplyFilter(args.QueryText);
        }
        else
        {
            ClaListListView.ItemsSource = ViewModel.Items;
        }
    }
    
    public ICommand NuevoCommand => new AsyncRelayCommand(OpenNewDialog);
    public ICommand BorrarCommand => new AsyncRelayCommand(BorrarRegistro);
    public ICommand AgregarCommand => new AsyncRelayCommand(AgregarRegistro);
    public ICommand DetalleCommand => new RelayCommand(VerDetalle);

    private async Task OpenNewDialog()
    {
        EditDialog.Title = "Nueva lista";
        EditDialog.PrimaryButtonText = "Agregar";
        EditDialog.PrimaryButtonCommand = AgregarCommand;
        EditDialog.DataContext = new ClaList();
        await EditDialog.ShowAsync();
    }

    private async Task BorrarRegistro()
    {
        try
        {
            await ViewModel.BorrarLista();
        }
        catch (Exception ex)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Error";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = ex.Message;

            await dialog.ShowAsync();
        }
    }

    private async Task AgregarRegistro()
    {
        await ViewModel.AgregarLista(EditDialog.DataContext as ClaList);
    }

    private void VerDetalle()
    {
        Frame.Navigate(typeof(ListaPorTipoPage), ViewModel.SelectedClaList);
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (sender.Text == "")
            {
                ClaListListView.ItemsSource = ViewModel.Items;
            }
        }
    }
}
