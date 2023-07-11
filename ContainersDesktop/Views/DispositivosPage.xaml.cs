using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.Core.Helpers;

namespace ContainersDesktop.Views;
public sealed partial class DispositivosPage : Page
{    
    public DispositivosViewModel ViewModel
    {
        get;
    }
    public DispositivosPage()
    {
        ViewModel = App.GetService<DispositivosViewModel>();
        InitializeComponent();
        Loaded += DispositivosPage_Loaded;
    }

    private void DispositivosPage_Loaded(object sender, RoutedEventArgs e)
    {
        DispositivosGrid.ItemsSource = ViewModel.ApplyFilter(null, false);
    }
    
    private async Task SincronizarDatos()
    {
        try
        {
            await ViewModel.SincronizarInformacion();

            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Sincronización";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = "Sincronización realizada!";

            await dialog.ShowAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public ICommand NuevoCommand => new AsyncRelayCommand(OpenNewDialog);
    public ICommand BorrarCommand => new AsyncRelayCommand(BorrarDispositivo);
    public ICommand AgregarCommand => new AsyncRelayCommand(AgregarDispositivo);
    public ICommand SincronizarCommand => new AsyncRelayCommand(SincronizarDatos);

    private async Task OpenNewDialog()
    {
        AgregarDialog.Title = "Nuevo dispositivo";
        AgregarDialog.PrimaryButtonText = "Agregar";
        AgregarDialog.PrimaryButtonCommand = AgregarCommand;
        AgregarDialog.DataContext = new Dispositivos()
        {
            DISPOSITIVOS_ID_ESTADO_REG = "A",
            DISPOSITIVOS_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now),
        };
        await AgregarDialog.ShowAsync();
    }

    private async Task BorrarDispositivo()
    {
        try
        {
            await ViewModel.BorrarDispositivo();
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

    private async Task AgregarDispositivo()
    {
        var dispositivo = AgregarDialog.DataContext as Dispositivos;
        await ViewModel.CrearDispositivo(dispositivo);

    }

    private async void DispositivosGrid_RowEditEnding(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEditEndingEventArgs e)
    {
        try
        {
            var dispositivo = DispositivosGrid!.SelectedItem as Dispositivos;
            dispositivo.DISPOSITIVOS_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);
            await ViewModel.ActualizarDispositivo(dispositivo);
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
    
    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.QueryText != "")
        {
            DispositivosGrid.ItemsSource = ViewModel.ApplyFilter(args.QueryText, chkMostrarTodos.IsChecked ?? false);
        }
    }

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (sender.Text == "")
            {
                DispositivosGrid.ItemsSource = ViewModel.ApplyFilter(sender.Text, chkMostrarTodos.IsChecked ?? false);
            }
        }
    }

    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {
        DispositivosGrid.ItemsSource = ViewModel.ApplyFilter(SearchBox.Text, true);
    }

    private void chkMostrarTodos_Unchecked(object sender, RoutedEventArgs e)
    {
        DispositivosGrid.ItemsSource = ViewModel.ApplyFilter(SearchBox.Text, false);
    }
}
