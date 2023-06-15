using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.ViewModels;
using ContainersDesktop.Core.Models;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ContainersDesktop.Views;

public sealed partial class ListaPorTipoPage : Page
{   
    public ListaPorTipoViewModel ViewModel
    {
        get;
    }

    public ListaPorTipoPage()
    {
        ViewModel = App.GetService<ListaPorTipoViewModel>();
        this.InitializeComponent();
    }

    private void btnAgregar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var nuevoRegistro = new Listas()
        {
            LISTAS_ID_ESTADO_REG = "A",
            LISTAS_ID_LISTA = 1,
            LISTAS_ID_LISTA_ORDEN = 1,
            LISTAS_ID_LISTA_DESCRIP = "INGRESE VALOR"
        };
        ViewModel.Source.Add(nuevoRegistro);
    }

    private async void btnBorrar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ListaGrid!.SelectedItem != null)
            {
                //SelectedLista = ListaGrid!.SelectedItem as Listas;
                //ViewModel.BorrarLista(SelectedLista);
            }
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
    
    private async void ListaGrid_RowEditEnding(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEditEndingEventArgs e)
    {
        Console.WriteLine(e.Row.DataContext);
        try
        {
            ViewModel.SelectedLista = ListaGrid!.SelectedItem as Listas;
            //ViewModel.GuardarLista(SelectedLista);

            //var previousSortedColumn = ViewModel.CachedSortedColumn;
            //ListaGrid.ItemsSource = ViewModel.SortData(previousSortedColumn, );
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

    public ICommand NuevoCommand => new AsyncRelayCommand(OpenNewDialog);
    public ICommand BorrarCommand => new AsyncRelayCommand(BorrarRegistro);
    public ICommand AgregarCommand => new AsyncRelayCommand(AgregarRegistro);
    public ICommand VolverCommand => new RelayCommand(Volver);

    private async Task OpenNewDialog()
    {
        AgregarDialog.Title = "Nueva entrada de lista";
        AgregarDialog.PrimaryButtonText = "Agregar";
        AgregarDialog.PrimaryButtonCommand = AgregarCommand;
        AgregarDialog.DataContext = new Listas();
        await AgregarDialog.ShowAsync();
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
        await ViewModel.AgregarLista(AgregarDialog.DataContext as Listas);
    }

    private void Volver()
    {
        Frame.GoBack();
    }
}
