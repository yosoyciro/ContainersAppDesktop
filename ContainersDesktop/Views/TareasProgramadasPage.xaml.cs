using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.DTO;
using CommunityToolkit.WinUI.UI.Controls;
using ContainersDesktop.Core.Models;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ContainersDesktop.Views;

public sealed partial class TareasProgramadasPage : Page
{
    public TareasProgramadasViewModel ViewModel
    {
        get;
    }
    public TareasProgramadasPage()
    {
        ViewModel = App.GetService<TareasProgramadasViewModel>();
        this.InitializeComponent();
    }

    public ICommand AgregarCommand => new AsyncRelayCommand(OpenAgregarDialog);
    public ICommand AgregarRegistroCommand => new AsyncRelayCommand(AgregarRegistro);
    public ICommand ModificarCommand => new AsyncRelayCommand(OpenModificarDialog);
    public ICommand ModificarRegistroCommand => new AsyncRelayCommand(ModificarRegistro);
    public ICommand BorrarCommand => new AsyncRelayCommand(BorrarRegistro);

    private async Task OpenAgregarDialog()
    {
        dlgFormulario.Title = "Agregar Tarea Programada";
        dlgFormulario.PrimaryButtonCommand = AgregarRegistroCommand;

        dlgFormulario.DataContext = new TareaProgramadaDTO()
        {
            TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = 0,
            TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = 0,
            TAREAS_PROGRAMADAS_FECHA_COMPLETADA = string.Empty,
            TAREAS_PROGRAMADAS_ORDENADO = string.Empty,            
        };

        cmbObjetos.SelectedItem = ViewModel.LstObjetosActivos.FirstOrDefault();
        txtFecha.Date = DateTime.Now;
        cmbUbicacionOrigen.SelectedItem = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0);
        cmbUbicacionDestino.SelectedItem = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0);
        cmbDispositivos.SelectedItem = ViewModel.LstDispositivosActivos.FirstOrDefault(x => x.MOVIM_ID_DISPOSITIVO > 0);

        await dlgFormulario.ShowAsync();
    }

    private async Task OpenModificarDialog()
    {
        if (!string.IsNullOrEmpty(ViewModel.Current.TAREAS_PROGRAMADAS_FECHA_COMPLETADA))
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Atención!";
            dialog.CloseButtonText = "Cerrar";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = "La Tarea ya fue completada, no se puede modificar";

            await dialog.ShowAsync();
        }
        else
        {
            dlgFormulario.Title = "Modificar Tarea Programada";
            dlgFormulario.PrimaryButtonCommand = ModificarRegistroCommand;
            //AgregarDialog.IsPrimaryButtonEnabled = ViewModel.FormViewModel.IsValid;
            dlgFormulario.DataContext = ViewModel.Current;

            ViewModel.FormViewModel.Objeto = ViewModel.LstObjetosActivos.FirstOrDefault(x => x.MOVIM_ID_OBJETO == ViewModel.Current.TAREAS_PROGRAMADAS_OBJETO_ID_REG);
            ViewModel.FormViewModel.FechaProgramada = DateTime.Parse(ViewModel.Current.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA);
            ViewModel.FormViewModel.UbicacionOrigen = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.MOVIM_ALMACEN == ViewModel.Current.TAREAS_PROGRAMADAS_UBICACION_ORIGEN);
            ViewModel.FormViewModel.UbicacionDestino = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.MOVIM_ALMACEN == ViewModel.Current.TAREAS_PROGRAMADAS_UBICACION_DESTINO);
            ViewModel.FormViewModel.Dispositivo = ViewModel.LstDispositivosActivos.FirstOrDefault(x => x.MOVIM_ID_DISPOSITIVO == ViewModel.Current.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG);

            await dlgFormulario.ShowAsync();
        }
    }

    private async Task BorrarRegistro()
    {
    //    ContentDialog bajaRegistroDialog = new ContentDialog
    //    {
    //        XamlRoot = this.XamlRoot,
    //        Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
    //        Title = "Atención!",
    //        Content = "Está seguro que desea dar de baja el registro?",
    //        PrimaryButtonText = "Sí",
    //        CloseButtonText = "No"
    //    };

    //    ContentDialogResult result = await bajaRegistroDialog.ShowAsync();

    //    if (result == ContentDialogResult.Primary)
    //    {
    //        try
    //        {
    //            await ViewModel.BorrarLista();
    //            ListaGrid.ItemsSource = ViewModel.AplicarFiltro(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
    //            LimpiarIndicadorOden();
    //        }
    //        catch (Exception ex)
    //        {
    //            ContentDialog dialog = new ContentDialog();

    //            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
    //            dialog.XamlRoot = this.XamlRoot;
    //            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
    //            dialog.Title = "Error";
    //            dialog.CloseButtonText = "Cerrar";
    //            dialog.DefaultButton = ContentDialogButton.Close;
    //            dialog.Content = ex.Message;

    //            await dialog.ShowAsync();
    //        }
    //    }
    }

    private async Task AgregarRegistro()
    {
        var tareaProgramada = dlgFormulario.DataContext as TareaProgramadaDTO;
        AsignarDTO(ref tareaProgramada);        
        await ViewModel.AgregarTareaProgramada(tareaProgramada);

        //TareasProgramadasGrid.ItemsSource = ViewModel.AplicarFiltro(null, true);
        //LimpiarIndicadorOden();
    }

    private async Task ModificarRegistro()
    {
        var tareaProgramada = dlgFormulario.DataContext as TareaProgramadaDTO;
        AsignarDTO(ref tareaProgramada);
        await ViewModel.ActualizarTareaProgramada(tareaProgramada);
        
        TareasProgramadasGrid.ItemsSource = ViewModel.AplicarFiltro(null, true);
        //LimpiarIndicadorOden();
    }

    private void AsignarDTO(ref TareaProgramadaDTO tareaProgramada)
    {
        tareaProgramada.TAREAS_PROGRAMADAS_OBJETO_ID_REG = ViewModel.FormViewModel.Objeto.MOVIM_ID_OBJETO;
        tareaProgramada.TAREAS_PROGRAMADAS_OBJETO_MATRICULA = ViewModel.FormViewModel.Objeto.DESCRIPCION;
        tareaProgramada.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.FechaEstandar(ViewModel.FormViewModel.FechaProgramada!.Value.Date);
        tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_ORIGEN = ViewModel.FormViewModel.UbicacionOrigen.MOVIM_ALMACEN;
        tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION = ViewModel.FormViewModel.UbicacionOrigen.DESCRIPCION;
        tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_DESTINO = ViewModel.FormViewModel.UbicacionDestino.MOVIM_ALMACEN;
        tareaProgramada.TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION = ViewModel.FormViewModel.UbicacionDestino.DESCRIPCION;
        tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = ViewModel.FormViewModel.Dispositivo.MOVIM_ID_DISPOSITIVO;
        tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION = ViewModel.FormViewModel.Dispositivo.DESCRIPCION;
        tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = 0;
        tareaProgramada.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = 0;
    }

    private void TareasProgramadasGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        // Add sorting indicator, and sort
        var isAscending = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending;
        TareasProgramadasGrid.ItemsSource = ViewModel.SortData(e.Column.Tag.ToString(), isAscending);
        e.Column.SortDirection = isAscending
            ? DataGridSortDirection.Ascending
            : DataGridSortDirection.Descending;

        // Remove sorting indicators from other columns
        foreach (var column in TareasProgramadasGrid.Columns)
        {
            if (column.Tag != null && column.Tag.ToString() != e.Column.Tag.ToString())
            {
                column.SortDirection = null;
            }
        }
    }
}
