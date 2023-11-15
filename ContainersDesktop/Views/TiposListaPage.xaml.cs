using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Helpers;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;
public sealed partial class TiposListaPage : Page
{
    public TiposListaViewModel ViewModel
    {
        get;
    }
    public TiposListaPage()
    {
        ViewModel = App.GetService<TiposListaViewModel>();
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
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }

    public ICommand DetalleCommand => new RelayCommand(DetalleCommand_Executed);
    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Executed);

    private async Task ExportarCommand_Executed()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TIPOSLISTA.csv");

        try
        {
            await Exportar.GenerarDatos(ViewModel.Items, filePath, this.XamlRoot);
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }    

    private void DetalleCommand_Executed()
    {
        Frame.Navigate(typeof(ListaPorTipoPage), ViewModel.Current);
    }

    #region Filtros
    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        //if (args.QueryText != "")
        //{
        //    ClaListListView.ItemsSource = ViewModel.ApplyFilterViewModel(ViewModel.Current, args.QueryText);
        //}
        //else
        //{
        //    ClaListListView.ItemsSource = ViewModel.Items;
        //}
        ViewModel.Filter = args.QueryText;
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (sender.Text == "")
            {
                //ClaListListView.ItemsSource = ViewModel.ApplyFilter(ViewModel.Current, SearchBox.Text); // ViewModel.Items;
                ViewModel.Filter = null;
            }
        }
    }

    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {
        ClaListListView.ItemsSource = ViewModel.ApplyFilter(ViewModel.Current, SearchBox.Text);
    }
    #endregion


}
