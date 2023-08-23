using System;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.Comunes.Helpers;
using Azure;
using ContainersDesktop.Helpers;
using WinUIEx.Messaging;
using Windows.UI.Popups;

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

    private async void DispositivosPage_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await ViewModel.CargarSource();
            grdDispositivos.ItemsSource = ViewModel.ApplyFilter(null, false);
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }        
    }
        
    public ICommand AgregarCommand => new AsyncRelayCommand(OpenAgregarDialog);
    public ICommand AgregarRegistroCommand => new AsyncRelayCommand(AgregarRegistro);
    public ICommand ModificarCommand => new AsyncRelayCommand(OpenModificarDialog);
    public ICommand ModificarRegistroCommand => new AsyncRelayCommand(ModificarRegistro);
    public ICommand BorrarRecuperarCommand => new AsyncRelayCommand(BorrarRecuperarRegistro);   
    public ICommand SincronizarCommand => new AsyncRelayCommand(SincronizarCommand_Execute);
    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Executed);

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

    private async Task SincronizarCommand_Execute()
    {
        try
        {
            await ViewModel.SincronizarInformacion();
            grdDispositivos.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);

            await Dialogs.Aviso(this.XamlRoot, "Sincronización realizada!");
        }
        catch (RequestFailedException ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
        catch (SystemException ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }

    #region CRUD
    private async Task BorrarRecuperarRegistro()
    {        
        var pregunta = ViewModel.EstadoActivo ? "Está seguro que desea dar de baja el registro?" : "Está seguro que desea recuperar el registro?";
        ContentDialogResult result = await Dialogs.Pregunta(this.XamlRoot, pregunta);

        if (result == ContentDialogResult.Primary)
        {
            try
            {
                await ViewModel.BorrarRecuperarDispositivo();

                grdDispositivos.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);
            }
            catch (Exception ex)
            {
                await Dialogs.Error(this.XamlRoot, ex.Message);
            }
        }
    }

    private async Task AgregarRegistro()
    {
        var dispositivo = AgregarDialog.DataContext as Dispositivos;
        dispositivo.DISPOSITIVOS_DESCRIP = ViewModel.FormViewModel.Descripcion;
        dispositivo.DISPOSITIVOS_CONTAINER = ViewModel.FormViewModel.Container;

        await ViewModel.CrearDispositivo(dispositivo);
        grdDispositivos.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);
    }

    private async Task ModificarRegistro()
    {
        var dispositivo = AgregarDialog.DataContext as Dispositivos;
        dispositivo.DISPOSITIVOS_DESCRIP = ViewModel.FormViewModel.Descripcion;
        dispositivo.DISPOSITIVOS_CONTAINER = ViewModel.FormViewModel.Container;

        await ViewModel.ActualizarDispositivo(dispositivo);
        grdDispositivos.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);
    }

    #endregion

    #region Handlers eventos de controles
    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.QueryText != "")
        {
            grdDispositivos.ItemsSource = ViewModel.ApplyFilter(args.QueryText, chkMostrarTodos.IsChecked ?? false);
        }
    }

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (sender.Text == "")
            {
                grdDispositivos.ItemsSource = ViewModel.ApplyFilter(sender.Text, chkMostrarTodos.IsChecked ?? false);
            }
        }
    }

    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {
        grdDispositivos.ItemsSource = ViewModel.ApplyFilter(SearchBox.Text, true);
    }

    private void chkMostrarTodos_Unchecked(object sender, RoutedEventArgs e)
    {
        grdDispositivos.ItemsSource = ViewModel.ApplyFilter(SearchBox.Text, false);
    }

    private void txtDispositivo_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        ViewModel.FormViewModel.Descripcion = sender.Text;
    }

    private void txtCloudContainer_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        ViewModel.FormViewModel.Container = sender.Text;

        //if (ViewModel.FormViewModel.IsValid)
        //{
        //    //Verifico que el container ya no haya sido asignado a otro movil
        //    if (await ViewModel.ExisteContainer(sender.Text, "local"))
        //    {
        //        ContentDialog avisoDialog = new ContentDialog
        //        {
        //            XamlRoot = this.Content.XamlRoot,
        //            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
        //            Title = "Atención!",
        //            CloseButtonText = "Cerrar",
        //            DefaultButton = ContentDialogButton.Close,
        //            Content = "Test",
        //        };

        //        await avisoDialog.ShowAsync();
        //    }
        //    else
        //    {
        //        ////Verifico si el Container existe en la plataforma
        //        //if (!await ViewModel.ExisteContainer(sender.Text, "cloud"))
        //        //{
        //        //    await Dialogs.Aviso(this.XamlRoot, "El container no existe en la plataforma");
        //        //}
        //        //else
        //        //{

        //        //}
        //    }
        //}        
    }
    #endregion

    private async Task ExportarCommand_Executed()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DISPOSITIVOS.csv");

        try
        {
            await Exportar.GenerarDatos(ViewModel.Source, filePath, this.XamlRoot);
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }
}
