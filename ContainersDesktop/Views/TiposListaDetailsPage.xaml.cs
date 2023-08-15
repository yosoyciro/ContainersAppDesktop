using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Core.Helpers;
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
        Loaded += TiposListaDetailsPage_Loaded;
    }

    private async void TiposListaDetailsPage_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await ViewModel.CargarSource();
        }
        catch (Exception ex)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Atenci�n!",
                Content = $"Error {ex.Message}",
                CloseButtonText = "Ok"
            };

            await errorDialog.ShowAsync();
        }
    }

    public ICommand DetalleCommand => new RelayCommand(DetalleCommand_Executed);
    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Executed);

    private async Task ExportarCommand_Executed()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TIPOSLISTA.csv");

        try
        {
            Exportar.GenerarDatos(ViewModel.Items, filePath);

            ContentDialog bajaRegistroDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Atenci�n!",
                Content = $"Se gener� el archivo {filePath}",
                CloseButtonText = "Ok"
            };

            await bajaRegistroDialog.ShowAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }    

    private void DetalleCommand_Executed()
    {
        Frame.Navigate(typeof(ListaPorTipoPage), ViewModel.SelectedClaList);
    }

    #region Filtros
    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.QueryText != "")
        {
            ClaListListView.ItemsSource = ViewModel.AplicarFiltro(args.QueryText);
        }
        else
        {
            ClaListListView.ItemsSource = ViewModel.Items;
        }
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

    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {
        ClaListListView.ItemsSource = ViewModel.AplicarFiltro(SearchBox.Text);
    }
    #endregion


}
