using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.DTO;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Azure;
using ContainersDesktop.Helpers;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Input;
using System.Diagnostics;

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
        Loaded += TareasProgramadasPage_Loaded;
    }

    private async void TareasProgramadasPage_Loaded(object sender, RoutedEventArgs e)
    {
        TareasProgramadasGrid.Background = new SolidColorBrush(ViewModel.GridColor);
        try
        {
            await ViewModel.CargarListasSource();

            TareasProgramadasGrid.ItemsSource = ViewModel.AplicarFiltro(null, chkMostrarTodos.IsChecked ?? false);
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }

    public ICommand AgregarCommand => new AsyncRelayCommand(OpenAgregarDialog);
    public ICommand AgregarRegistroCommand => new AsyncRelayCommand(AgregarRegistroCommand_Execute);
    public ICommand ModificarCommand => new AsyncRelayCommand(OpenModificarDialog);
    public ICommand ModificarRegistroCommand => new AsyncRelayCommand(ModificarRegistro);
    public ICommand BorrarRecuperarCommand => new AsyncRelayCommand(BorrarRecuperarCommand_Execute);
    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Execute);
    public ICommand SincronizarCommand => new AsyncRelayCommand(SincronizarCommand_Execute);

    private async Task OpenAgregarDialog()
    {
        dlgFormulario.Title = "Agregar Tarea Programada";
        dlgFormulario.PrimaryButtonCommand = AgregarRegistroCommand;

        dlgFormulario.DataContext = new TareaProgramadaDTO()
        {
            TAREAS_PROGRAMADAS_ID_ESTADO_REG = "A",
            TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = 0,
            TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = 0,
            TAREAS_PROGRAMADAS_FECHA_COMPLETADA = string.Empty,
            TAREAS_PROGRAMADAS_ORDENADO = string.Empty,            
        };

        await ViewModel.CargarFechasYHoras(ViewModel.LstDispositivosActivos.FirstOrDefault(x => x.MOVIM_ID_DISPOSITIVO > 0).MOVIM_ID_DISPOSITIVO);

        ViewModel.FormViewModel.Objeto = ViewModel.LstObjetosActivos.FirstOrDefault();
        ViewModel.FormViewModel.FechaProgramada = ViewModel.LstFechasDisponibles.Count > 0 ? ViewModel.LstFechasDisponibles.FirstOrDefault() : ViewModel.FormViewModel.FechaHoy;
        ViewModel.FormViewModel.HoraProgramada = ViewModel.LstHoras.FirstOrDefault(); //new TimeSpan(ViewModel.LstHoras.FirstOrDefault(), 0, 0);
        ViewModel.FormViewModel.UbicacionOrigen =  ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0);
        ViewModel.FormViewModel.UbicacionDestino = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0);
        ViewModel.FormViewModel.Dispositivo = ViewModel.LstDispositivosActivos.FirstOrDefault(x => x.MOVIM_ID_DISPOSITIVO > 0);
       
        await dlgFormulario.ShowAsync();
    }

    private async Task OpenModificarDialog()
    {
        if (!string.IsNullOrEmpty(ViewModel.Current.TAREAS_PROGRAMADAS_FECHA_COMPLETADA))
        {
            await Dialogs.Aviso(this.XamlRoot, "La Tarea ya fue completada, no se puede modificar");            
        }
        else
        {
            dlgFormulario.Title = "Modificar Tarea Programada";
            dlgFormulario.PrimaryButtonCommand = ModificarRegistroCommand;
            //AgregarDialog.IsPrimaryButtonEnabled = ViewModel.FormViewModel.IsValid;
            dlgFormulario.DataContext = ViewModel.Current;

            ViewModel.FormViewModel.Objeto = ViewModel.LstObjetosActivos.FirstOrDefault(x => x.MOVIM_ID_OBJETO == ViewModel.Current.TAREAS_PROGRAMADAS_OBJETO_ID_REG);
            ViewModel.FormViewModel.FechaProgramada = DateTime.Parse(ViewModel.Current.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA);
            ViewModel.FormViewModel.HoraProgramada = ViewModel.FormViewModel.FechaProgramada.Hour; //new TimeSpan(ViewModel.FormViewModel.FechaProgramada.Hour, ViewModel.FormViewModel.FechaProgramada.Minute, 0);
            ViewModel.FormViewModel.UbicacionOrigen = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.MOVIM_ALMACEN == ViewModel.Current.TAREAS_PROGRAMADAS_UBICACION_ORIGEN);
            ViewModel.FormViewModel.UbicacionDestino = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.MOVIM_ALMACEN == ViewModel.Current.TAREAS_PROGRAMADAS_UBICACION_DESTINO);
            ViewModel.FormViewModel.Dispositivo = ViewModel.LstDispositivosActivos.FirstOrDefault(x => x.MOVIM_ID_DISPOSITIVO == ViewModel.Current.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG);

            await dlgFormulario.ShowAsync();
        }
    }
    
    private async Task SincronizarCommand_Execute()
    {
        try
        {
            await ViewModel.SincronizarInformacion();
            ViewModel.AplicarFiltro(null, chkMostrarTodos.IsChecked ?? false);
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
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);

        }
    }

    private async Task ExportarCommand_Execute()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "TAREAS_PROGRAMADAS.csv");

        try
        {
            await Exportar.GenerarDatos(ViewModel.Items, filePath, this.XamlRoot);
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }

    private async Task BorrarRecuperarCommand_Execute()
    {
        var pregunta = ViewModel.EstadoActivo ? "Está seguro que desea dar de baja el registro?" : "Está seguro que desea recuperar el registro?";        
        ContentDialogResult result = await Dialogs.Pregunta(this.XamlRoot, pregunta);

        if (result == ContentDialogResult.Primary)
        {
            try
            {
                await ViewModel.BorrarRecuperarRegistro();
                TareasProgramadasGrid.ItemsSource = ViewModel.AplicarFiltro(null, chkMostrarTodos.IsChecked ?? false);
                //LimpiarIndicadorOden();      
            }
            catch (Exception ex)
            {
                await Dialogs.Error(this.XamlRoot, ex.Message);
            }
        }
    }

    private async Task AgregarRegistroCommand_Execute()
    {
        //var result = ViewModel.FormViewModel.ValidateUbicacion();
        var tareaProgramada = dlgFormulario.DataContext as TareaProgramadaDTO;
        AsignarDTO(ref tareaProgramada);
        await ViewModel.AgregarTareaProgramada(tareaProgramada);

        TareasProgramadasGrid.ItemsSource = ViewModel.AplicarFiltro(null, true);
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
        TimeSpan? time = new TimeSpan(1, 0, 0); //tpkHora.SelectedTime;
        var fechaHora = ViewModel.FormViewModel.FechaProgramada.Date.Add(time!.Value);

        tareaProgramada.TAREAS_PROGRAMADAS_OBJETO_ID_REG = ViewModel.FormViewModel.Objeto.MOVIM_ID_OBJETO;
        tareaProgramada.TAREAS_PROGRAMADAS_OBJETO_MATRICULA = ViewModel.FormViewModel.Objeto.DESCRIPCION;
        tareaProgramada.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.FechaEstandar(fechaHora);
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

    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {
        TareasProgramadasGrid.ItemsSource = ViewModel.AplicarFiltro(null, true);
    }

    private void chkMostrarTodos_Unchecked(object sender, RoutedEventArgs e)
    {
        TareasProgramadasGrid.ItemsSource = ViewModel.AplicarFiltro(null, false);
    }

    private async void cmbDispositivos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {                
        try
        {
            txtFecha.Date = null;
            var dispositivo = cmbDispositivos.SelectedItem as DispositivosDTO;

            await ViewModel.CargarFechasYHoras(dispositivo!.MOVIM_ID_DISPOSITIVO);            

            txtFecha.Date = ViewModel.LstFechasDisponibles.Count > 0 ? ViewModel.LstFechasDisponibles.FirstOrDefault() : ViewModel.FormViewModel.FechaHoy;                        

            cmbHora.ItemsSource = ViewModel.ObtenerHoras();
            cmbHora.SelectedIndex = 0;

            Debug.WriteLine(ViewModel.LstHoras);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private void txtFecha_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs e)
    {
        e.Item.IsBlackout = ViewModel.LstFechasDisponibles.Count > 0 
            ? 
                !ViewModel.LstFechasDisponibles.Contains(e.Item.Date.Date) 
                || e.Item.Date.DayOfWeek == DayOfWeek.Saturday 
                || e.Item.Date.DayOfWeek == DayOfWeek.Saturday 
            : 
                false;
    }

    private void cmbHora_Loaded(object sender, RoutedEventArgs e)
    {        
        //cmbHora.SelectedItem = ViewModel.LstHoras.FirstOrDefault();
        cmbHora.SelectedIndex = 0;
    }

    private void txtFecha_Opened(object sender, object e)
    {
        Debug.WriteLine("txtFecha_Opened");
        txtFecha.MinDate = ViewModel.LstFechasDisponibles.Count > 0 ? ViewModel.LstFechasDisponibles.FirstOrDefault() : ViewModel.FormViewModel.FechaHoy;
        txtFecha.MaxDate = DateTime.Now.AddDays(90);
        txtFecha.Date = ViewModel.LstFechasDisponibles.Count > 0 ? ViewModel.LstFechasDisponibles.FirstOrDefault() : ViewModel.FormViewModel.FechaHoy;
    }
}
