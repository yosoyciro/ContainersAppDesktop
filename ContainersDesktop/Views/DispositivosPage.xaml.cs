using System;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.Core.Helpers;
using Azure;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

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
        catch (RequestFailedException ex)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Sincronización";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = "Error en la Sincronización: " + ex.Message;

            await dialog.ShowAsync();
        }
        catch (SystemException ex)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Sincronización";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = "Error en la Sincronización: " + ex.Message;

            await dialog.ShowAsync();
        }
    }

    public ICommand AgregarCommand => new AsyncRelayCommand(OpenAgregarDialog);
    public ICommand AgregarRegistroCommand => new AsyncRelayCommand(AgregarRegistro);
    public ICommand ModificarCommand => new AsyncRelayCommand(OpenModificarDialog);
    public ICommand ModificarRegistroCommand => new AsyncRelayCommand(ModificarRegistro);
    public ICommand BorrarRecuperarCommand => new AsyncRelayCommand(BorrarRecuperarRegistro);   
    public ICommand SincronizarCommand => new AsyncRelayCommand(SincronizarDatos);
    public ICommand ExportarCommand => new AsyncRelayCommand(Exportar);

    private async Task OpenAgregarDialog()
    {
        AgregarDialog.Title = "Agregar dispositivo";
        AgregarDialog.PrimaryButtonCommand = AgregarRegistroCommand;
        AgregarDialog.DataContext = new Dispositivos()
        {
            DISPOSITIVOS_ID_ESTADO_REG = "A",
            DISPOSITIVOS_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now),
        };

        //Valores por defecto de los campos
        ViewModel.FormViewModel.Descripcion = string.Empty;
        ViewModel.FormViewModel.Container = string.Empty;
        await AgregarDialog.ShowAsync();
    }

    private async Task OpenModificarDialog()
    {
        AgregarDialog.Title = "Modificar dispositivo";
        AgregarDialog.PrimaryButtonCommand = ModificarRegistroCommand;
        AgregarDialog.DataContext = ViewModel.SelectedDispositivo;

        //Valores por defecto de los campos
        ViewModel.FormViewModel.Descripcion = ViewModel.SelectedDispositivo.DISPOSITIVOS_DESCRIP;
        ViewModel.FormViewModel.Container = ViewModel.SelectedDispositivo.DISPOSITIVOS_CONTAINER;
        await AgregarDialog.ShowAsync();
    }

    #region CRUD
    private async Task BorrarRecuperarRegistro()
    {
        ContentDialog bajaRegistroDialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = "Atención!",
            Content = ViewModel.EstadoActivo ? "Está seguro que desea dar de baja el registro?" : "Está seguro que desea recuperar el registro?",
            PrimaryButtonText = "Sí",
            CloseButtonText = "No"
        };

        ContentDialogResult result = await bajaRegistroDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            try
            {
                await ViewModel.BorrarRecuperarDispositivo();

                DispositivosGrid.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);
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
    }

    private async Task AgregarRegistro()
    {
        var dispositivo = AgregarDialog.DataContext as Dispositivos;
        dispositivo.DISPOSITIVOS_DESCRIP = ViewModel.FormViewModel.Descripcion;
        dispositivo.DISPOSITIVOS_CONTAINER = ViewModel.FormViewModel.Container;

        //Verifico que el container ya no haya sido asignado a otro movil
        if (await ViewModel.ExisteContainer(dispositivo, "local"))
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Validación";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = "El container ya está asignado a otro móvil";

            await dialog.ShowAsync();
        }
        else
        {
            //Verifico si el Container existe en la plataforma
            if (!await ViewModel.ExisteContainer(dispositivo, "cloud"))
            {
                ContentDialog dialog = new ContentDialog();

                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                dialog.XamlRoot = this.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = "Validación";
                dialog.CloseButtonText = "Cerrar";
                dialog.DefaultButton = ContentDialogButton.Close;
                dialog.Content = "El container no existe en la plataforma";

                await dialog.ShowAsync();
            }
            else
            {
                await ViewModel.CrearDispositivo(dispositivo);

                DispositivosGrid.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);
            }                
        }
    }

    private async Task ModificarRegistro()
    {
        var dispositivo = AgregarDialog.DataContext as Dispositivos;
        dispositivo.DISPOSITIVOS_DESCRIP = ViewModel.FormViewModel.Descripcion;
        dispositivo.DISPOSITIVOS_CONTAINER = ViewModel.FormViewModel.Container;
        await ViewModel.ActualizarDispositivo(dispositivo);

        DispositivosGrid.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);
    }

    #endregion

    #region Handlers eventos de controles
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

    private void txtDispositivo_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        ViewModel.FormViewModel.Descripcion = sender.Text;
    }

    private void txtCloudContainer_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        ViewModel.FormViewModel.Container = sender.Text;
    }
    #endregion

    private async Task Exportar()
    {
        //ExportToCSV
    }
}
