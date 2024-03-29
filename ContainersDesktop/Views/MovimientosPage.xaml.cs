using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using ContainersDesktop.Helpers;
using ContainersDesktop.Dominio.DTO;
using CommunityToolkit.WinUI.UI.Controls;
using Azure;
using ContainersDesktop.ViewModels;
using ContainersDesktop.Comunes.Helpers;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContainersDesktop.Views;
public sealed partial class MovimientosPage : Page
{
    public MovimientosViewModel ViewModel
    {
        get;
    }
    public MovimientosPage()
    {
        ViewModel = App.GetService<MovimientosViewModel>();
        InitializeComponent();
        Loaded += MovimientosContainerPage_Loaded;
    }

    private async void MovimientosContainerPage_Loaded(object sender, RoutedEventArgs e)
    {
        MovimientosGrid.Background = new SolidColorBrush(ViewModel.GridColor);
        try
        {
            await ViewModel.CargarListasSource();
            MovimientosGrid.ItemsSource = ViewModel.ApplyFilter(null, false);
        }
        catch (Exception ex)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Atención!",
                Content = $"Error {ex.Message}",
                CloseButtonText = "Ok"
            };

            await errorDialog.ShowAsync();
        }
        
    }

    #region Commands y dialogs
    public ICommand AgregarCommand => new AsyncRelayCommand(AbrirAgregarDialog);
    public ICommand AgregarMovimientoCommand => new AsyncRelayCommand(AgregarMovimiento);
    public ICommand ModificarCommand => new AsyncRelayCommand(AbrirModificarDialog);
    public ICommand ModificarMovimientoCommand => new AsyncRelayCommand(ModificarMovimiento);
    public ICommand BorrarRecuperarCommand => new AsyncRelayCommand(BorrarRecuperarCommand_Execute);    
    public ICommand VolverCommand => new RelayCommand(Volver);
    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Execute);
    public ICommand SincronizarCommand => new AsyncRelayCommand(SincronizarCommand_Execute);

    private async Task AbrirModificarDialog()
    {
        Dialog.Title = "Modificar tarea";
        Dialog.PrimaryButtonCommand = ModificarMovimientoCommand;
        Dialog.DataContext = ViewModel.Current;

        //Valores x defecto combos
        txtFecha.Date = DateTime.Parse(ViewModel.Current.MOVIM_FECHA);
        tpkHora.SelectedTime = new TimeSpan(txtFecha.Date.Value.Hour, txtFecha.Date.Value.Minute, 0);
        ComboObjetos.SelectedItem = ViewModel.LstObjetosActivos.FirstOrDefault(x => x.MOVIM_ID_OBJETO == ViewModel.Current.MOVIM_ID_OBJETO);
        ComboTiposMovimiento.SelectedItem = ViewModel.LstTiposMovimientoActivos.FirstOrDefault(x => x.MOVIM_TIPO_MOVIM == ViewModel.Current.MOVIM_TIPO_MOVIM) ?? ViewModel.LstTiposMovimientoActivos.FirstOrDefault();
        ComboPesos.SelectedItem = ViewModel.LstPesosActivos.FirstOrDefault(x => x.MOVIM_PESO == ViewModel.Current.MOVIM_PESO) ?? ViewModel.LstPesosActivos.FirstOrDefault();
        ComboTransportistas.SelectedItem = ViewModel.LstTransportistasActivos.FirstOrDefault(x => x.MOVIM_TRANSPORTISTA == ViewModel.Current.MOVIM_TRANSPORTISTA) ?? ViewModel.LstTransportistasActivos.FirstOrDefault();
        ComboClientes.SelectedItem = ViewModel.LstClientesActivos.FirstOrDefault(x => x.MOVIM_CLIENTE == ViewModel.Current.MOVIM_CLIENTE) ?? ViewModel.LstClientesActivos.FirstOrDefault();
        ComboChoferes.SelectedItem = ViewModel.LstChoferesActivos.FirstOrDefault(x => x.MOVIM_CHOFER == ViewModel.Current.MOVIM_CHOFER) ?? ViewModel.LstChoferesActivos.FirstOrDefault();
        ComboEntradaSalida.SelectedItem = ViewModel.LstEntradaSalidaActivos.FirstOrDefault(x => x.MOVIM_ENTRADA_SALIDA == ViewModel.Current.MOVIM_ENTRADA_SALIDA) ?? ViewModel.LstEntradaSalidaActivos.FirstOrDefault();
        ComboAlmacenes.SelectedItem = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.MOVIM_ALMACEN == ViewModel.Current.MOVIM_ALMACEN) ?? ViewModel.LstAlmacenesActivos.FirstOrDefault();
        cmbUbicacionOrigen.SelectedItem = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.MOVIM_ALMACEN == ViewModel.FormViewModel.UbicacionOrigen.MOVIM_ALMACEN) ?? ViewModel.LstAlmacenesActivos.FirstOrDefault();
        cmbUbicacionDestino.SelectedItem = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.MOVIM_ALMACEN == ViewModel.FormViewModel.UbicacionDestino.MOVIM_ALMACEN) ?? ViewModel.LstAlmacenesActivos.FirstOrDefault();
        cmbDispositivos.SelectedItem = ViewModel.LstDispositivosActivos.FirstOrDefault(x => x.MOVIM_ID_DISPOSITIVO == ViewModel.Current.MOVIM_ID_DISPOSITIVO) ?? ViewModel.LstDispositivosActivos.FirstOrDefault();

        await Dialog.ShowAsync();
    }

    private async Task AbrirAgregarDialog()
    {
        Dialog.Title = "Agregar tarea";
        Dialog.PrimaryButtonCommand = AgregarMovimientoCommand;
        Dialog.DataContext = new MovimDTO()
        {
            MOVIM_ID_REG_MOBILE = 0,
            MOVIM_ID_ESTADO_REG = "A",
            MOVIM_FECHA = FormatoFecha.ConvertirAFechaCorta(DateTime.Now.Date.ToString()),
            MOVIM_CAMION_ID = string.Empty,
            MOVIM_REMOLQUE_ID = string.Empty,
            MOVIM_ALBARAN_ID = string.Empty,
            MOVIM_OBSERVACIONES = string.Empty,
            MOVIM_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now),
            MOVIM_PDF = string.Empty,
        };

        //valores x defecto para los combo
        ViewModel.FormViewModel.Fecha = DateTime.Now.Date;
        tpkHora.SelectedTime = new TimeSpan(txtFecha.Date.Value.Hour, txtFecha.Date.Value.Minute, 0);
        ComboObjetos.SelectedItem = ViewModel.LstObjetosActivos.FirstOrDefault();
        ComboTiposMovimiento.SelectedItem = ViewModel.LstTiposMovimientoActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstTiposMovimientoActivos.FirstOrDefault();
        ComboPesos.SelectedItem = ViewModel.LstPesosActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstPesosActivos.FirstOrDefault();
        ComboTransportistas.SelectedItem = ViewModel.LstTransportistasActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstTransportistasActivos.FirstOrDefault();
        ComboClientes.SelectedItem = ViewModel.LstClientesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstClientesActivos.FirstOrDefault();
        ComboChoferes.SelectedItem = ViewModel.LstChoferesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstChoferesActivos.FirstOrDefault();
        ComboEntradaSalida.SelectedItem = ViewModel.LstEntradaSalidaActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstEntradaSalidaActivos.FirstOrDefault();
        ComboAlmacenes.SelectedItem = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstAlmacenesActivos.FirstOrDefault();
        cmbUbicacionOrigen.SelectedItem = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstAlmacenesActivos.FirstOrDefault();
        cmbUbicacionDestino.SelectedItem = ViewModel.LstAlmacenesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstAlmacenesActivos.FirstOrDefault();
        cmbDispositivos.SelectedItem = ViewModel.LstDispositivosActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstDispositivosActivos.FirstOrDefault();

        await Dialog.ShowAsync();
    }
    private async Task SincronizarCommand_Execute()
    {
        try
        {
            await ViewModel.SincronizarInformacion();
            MovimientosGrid.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);

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


    private async Task ExportarCommand_Execute()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "HISTORIAL_TAREAS.csv");

        try
        {
            await Exportar.GenerarDatos(ViewModel.Items, filePath, this.XamlRoot);            
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }

    #endregion

    #region Metodos CRUD
    private async Task AgregarMovimiento()
    {
        //Obtengo el item seleccionado de cada combo
        var objeto = ComboObjetos.SelectedItem as ObjetosDTO;
        var tipoMovimiento = ComboTiposMovimiento.SelectedItem as TiposMovimientoDTO;
        var peso = ComboPesos.SelectedItem as PesosDTO;
        var transportista = ComboTransportistas.SelectedItem as TransportistasDTO;
        var cliente = ComboClientes.SelectedItem as ClientesDTO;
        var chofer = ComboChoferes.SelectedItem as ChoferesDTO;
        var entradaSalida = ComboEntradaSalida.SelectedItem as EntradaSalidaDTO;
        var almacen = ComboAlmacenes.SelectedItem as AlmacenesDTO;
        var dispositivo = cmbDispositivos.SelectedItem as DispositivosDTO;
        var ubicacionOrigen = cmbUbicacionOrigen.SelectedItem as AlmacenesDTO;
        var ubicacionDestino = cmbUbicacionDestino.SelectedItem as AlmacenesDTO;

        var nuevoMovimiento = Dialog.DataContext as MovimDTO;
        //asigno los valores al objeto que voy a grabar
        TimeSpan? time = tpkHora.SelectedTime;
        var fechaHora = ViewModel.FormViewModel.Fecha!.Value.Date.Add(time!.Value);

        nuevoMovimiento.MOVIM_ID_DISPOSITIVO = dispositivo.MOVIM_ID_DISPOSITIVO;
        nuevoMovimiento.MOVIM_DISPOSITIVO_DESCRIPCION = dispositivo.DESCRIPCION;
        nuevoMovimiento.MOVIM_ID_REG_MOBILE = 0;
        nuevoMovimiento.MOVIM_FECHA = FormatoFecha.FechaEstandar(fechaHora);
        nuevoMovimiento.MOVIM_ID_OBJETO = objeto.MOVIM_ID_OBJETO;
        nuevoMovimiento.MOVIM_MATRICULA_OBJ = objeto.DESCRIPCION;
        nuevoMovimiento.MOVIM_TIPO_MOVIM = tipoMovimiento.MOVIM_TIPO_MOVIM;
        nuevoMovimiento.MOVIM_TIPO_MOVIM_LISTA = tipoMovimiento.LISTAS_ID_LISTA;
        nuevoMovimiento.MOVIM_TIPO_MOVIM_DESCRIPCION = tipoMovimiento.DESCRIPCION;
        nuevoMovimiento.MOVIM_PESO = peso.MOVIM_PESO;
        nuevoMovimiento.MOVIM_PESO_LISTA = peso.LISTAS_ID_LISTA;
        nuevoMovimiento.MOVIM_PESO_DESCRIPCION = peso.DESCRIPCION;
        nuevoMovimiento.MOVIM_TRANSPORTISTA = transportista.MOVIM_TRANSPORTISTA;
        nuevoMovimiento.MOVIM_TRANSPORTISTA_LISTA = transportista.LISTAS_ID_LISTA;
        nuevoMovimiento.MOVIM_TRANSPORTISTA_DESCRIPCION = transportista.DESCRIPCION;
        nuevoMovimiento.MOVIM_CLIENTE = cliente.MOVIM_CLIENTE;
        nuevoMovimiento.MOVIM_CLIENTE_LISTA = cliente.LISTAS_ID_LISTA;
        nuevoMovimiento.MOVIM_CLIENTE_DESCRIPCION = cliente.DESCRIPCION;
        nuevoMovimiento.MOVIM_CHOFER = chofer.MOVIM_CHOFER;
        nuevoMovimiento.MOVIM_CHOFER_LISTA = chofer.LISTAS_ID_LISTA;
        nuevoMovimiento.MOVIM_CHOFER_DESCRIPCION = chofer.DESCRIPCION;
        nuevoMovimiento.MOVIM_ENTRADA_SALIDA = entradaSalida.MOVIM_ENTRADA_SALIDA;
        nuevoMovimiento.MOVIM_ENTRADA_SALIDA_LISTA = entradaSalida.LISTAS_ID_LISTA;
        nuevoMovimiento.MOVIM_ENTRADA_SALIDA_DESCRIPCION = entradaSalida.DESCRIPCION;
        nuevoMovimiento.MOVIM_ALMACEN = almacen.MOVIM_ALMACEN;
        nuevoMovimiento.MOVIM_ALMACEN_LISTA = almacen.LISTAS_ID_LISTA;
        nuevoMovimiento.MOVIM_ALMACEN_DESCRIPCION = almacen.DESCRIPCION;
        nuevoMovimiento.MOVIM_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);
        nuevoMovimiento.MOVIM_UBICACION_ORIGEN = ubicacionOrigen.MOVIM_ALMACEN;
        nuevoMovimiento.MOVIM_UBICACION_ORIGEN_DESCRIPCION = ubicacionOrigen.DESCRIPCION;
        nuevoMovimiento.MOVIM_UBICACION_DESTINO = ubicacionDestino.MOVIM_ALMACEN;
        nuevoMovimiento.MOVIM_UBICACION_DESTINO_DESCRIPCION = ubicacionDestino.DESCRIPCION;

        await ViewModel.AgregarMovimiento(nuevoMovimiento);

        MovimientosGrid.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);
    }

    private async Task ModificarMovimiento()
    {
        var movimiento = Dialog.DataContext as MovimDTO;

        //Obtengo el item seleccionado de cada combo
        var objeto = ComboObjetos.SelectedItem as ObjetosDTO;
        var tipoMovimiento = ComboTiposMovimiento.SelectedItem as TiposMovimientoDTO;
        var peso = ComboPesos.SelectedItem as PesosDTO;
        var transportista = ComboTransportistas.SelectedItem as TransportistasDTO;
        var cliente = ComboClientes.SelectedItem as ClientesDTO;
        var chofer = ComboChoferes.SelectedItem as ChoferesDTO;
        var entradaSalida = ComboEntradaSalida.SelectedItem as EntradaSalidaDTO;
        var almacen = ComboAlmacenes.SelectedItem as AlmacenesDTO;
        var dispositivo = cmbDispositivos.SelectedItem as DispositivosDTO;
        var ubicacionOrigen = cmbUbicacionOrigen.SelectedItem as AlmacenesDTO;
        var ubicacionDestino = cmbUbicacionDestino.SelectedItem as AlmacenesDTO;

        //asigno los valores al objeto que voy a grabar
        TimeSpan? time = tpkHora.SelectedTime;
        var fechaHora = ViewModel.FormViewModel.Fecha!.Value.Date.Add(time!.Value);

        movimiento.MOVIM_ID_DISPOSITIVO = dispositivo.MOVIM_ID_DISPOSITIVO;
        movimiento.MOVIM_DISPOSITIVO_DESCRIPCION = dispositivo.DESCRIPCION;
        movimiento.MOVIM_ID_REG_MOBILE = 0;
        movimiento.MOVIM_FECHA = FormatoFecha.FechaEstandar(fechaHora);
        movimiento.MOVIM_ID_OBJETO = objeto.MOVIM_ID_OBJETO;
        movimiento.MOVIM_MATRICULA_OBJ = objeto.DESCRIPCION;
        movimiento.MOVIM_FECHA = FormatoFecha.FechaEstandar(txtFecha.Date!.Value.Date);
        movimiento.MOVIM_TIPO_MOVIM = tipoMovimiento.MOVIM_TIPO_MOVIM;
        movimiento.MOVIM_TIPO_MOVIM_LISTA = tipoMovimiento.LISTAS_ID_LISTA;
        movimiento.MOVIM_TIPO_MOVIM_DESCRIPCION = tipoMovimiento.DESCRIPCION;
        movimiento.MOVIM_PESO = peso.MOVIM_PESO;
        movimiento.MOVIM_PESO_LISTA = peso.LISTAS_ID_LISTA;
        movimiento.MOVIM_PESO_DESCRIPCION = peso.DESCRIPCION;
        movimiento.MOVIM_TRANSPORTISTA = transportista.MOVIM_TRANSPORTISTA;
        movimiento.MOVIM_TRANSPORTISTA_LISTA = transportista.LISTAS_ID_LISTA;
        movimiento.MOVIM_TRANSPORTISTA_DESCRIPCION = transportista.DESCRIPCION;
        movimiento.MOVIM_CLIENTE = cliente.MOVIM_CLIENTE;
        movimiento.MOVIM_CLIENTE_LISTA = cliente.LISTAS_ID_LISTA;
        movimiento.MOVIM_CLIENTE_DESCRIPCION = cliente.DESCRIPCION;
        movimiento.MOVIM_CHOFER = chofer.MOVIM_CHOFER;
        movimiento.MOVIM_CHOFER_LISTA = chofer.LISTAS_ID_LISTA;
        movimiento.MOVIM_CHOFER_DESCRIPCION = chofer.DESCRIPCION;
        movimiento.MOVIM_ENTRADA_SALIDA = entradaSalida.MOVIM_ENTRADA_SALIDA;
        movimiento.MOVIM_ENTRADA_SALIDA_LISTA = entradaSalida.LISTAS_ID_LISTA;
        movimiento.MOVIM_ENTRADA_SALIDA_DESCRIPCION = entradaSalida.DESCRIPCION;
        movimiento.MOVIM_ALMACEN = almacen.MOVIM_ALMACEN;
        movimiento.MOVIM_ALMACEN_LISTA = almacen.LISTAS_ID_LISTA;
        movimiento.MOVIM_ALMACEN_DESCRIPCION = almacen.DESCRIPCION;
        movimiento.MOVIM_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);
        movimiento.MOVIM_PDF = string.Empty;        
        movimiento.MOVIM_UBICACION_ORIGEN = ubicacionOrigen.MOVIM_ALMACEN;
        movimiento.MOVIM_UBICACION_ORIGEN_DESCRIPCION = ubicacionOrigen.DESCRIPCION;
        movimiento.MOVIM_UBICACION_DESTINO = ubicacionDestino.MOVIM_ALMACEN;
        movimiento.MOVIM_UBICACION_DESTINO_DESCRIPCION = ubicacionDestino.DESCRIPCION;

        await ViewModel.ActualizarMovimiento(movimiento);

        MovimientosGrid.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);
    }

    private async Task BorrarRecuperarCommand_Execute()
    {
        var pregunta = ViewModel.EstadoActivo ? Constantes.W_DarBajaRegistro.GetLocalized() : Constantes.W_RecuperarRegistro.GetLocalized();        
        ContentDialogResult result = await Dialogs.Pregunta(this.XamlRoot, pregunta);

        if (result == ContentDialogResult.Primary)
        {
            await ViewModel.BorrarRecuperarRegistro();

            MovimientosGrid.ItemsSource = ViewModel.ApplyFilter(null, chkMostrarTodos.IsChecked ?? false);
            //LimpiarIndicadorOden();
        }
    }

    #endregion

    private void Volver()
    {
        Frame.GoBack();
    }

    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {
        MovimientosGrid.ItemsSource = ViewModel.ApplyFilter(null, true);
    }

    private void chkMostrarTodos_Unchecked(object sender, RoutedEventArgs e)
    {
        MovimientosGrid.ItemsSource = ViewModel.ApplyFilter(null, false);
    }

    private void MovimientosGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        // Add sorting indicator, and sort
        var isAscending = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending;
        MovimientosGrid.ItemsSource = ViewModel.SortData(e.Column.Tag.ToString(), isAscending);
        e.Column.SortDirection = isAscending
            ? DataGridSortDirection.Ascending
            : DataGridSortDirection.Descending;

        // Remove sorting indicators from other columns
        foreach (var column in MovimientosGrid.Columns)
        {
            if (column.Tag != null && column.Tag.ToString() != e.Column.Tag.ToString())
            {
                column.SortDirection = null;
            }
        }
    }
}
