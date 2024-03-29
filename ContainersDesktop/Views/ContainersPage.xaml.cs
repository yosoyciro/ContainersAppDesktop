﻿using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using CommunityToolkit.WinUI.UI.Controls;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.DTO;
using Windows.UI;
using ContainersDesktop.Helpers;

namespace ContainersDesktop.Views;

public sealed partial class ContainersPage : Page
{
    private CalendarDatePicker FechaInspecDatePicker = new();
    public ContainersViewModel ViewModel
    {
        get;
    }

    public ContainersPage()
    {
        ViewModel = App.GetService<ContainersViewModel>();
        InitializeComponent();
        this.Loaded += ContainersGridPage_Loaded;
    }

    private async void ContainersGridPage_Loaded(object sender, RoutedEventArgs e)
    {
        grdContainers.Background = new SolidColorBrush(ViewModel.GridColor);       
        try
        {
            await ViewModel.CargarListasSource();
            grdContainers.ItemsSource = ViewModel.ApplyFilter(null, false);
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }        
    }
    
    private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        if (depObj != null)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }

                foreach (T childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }
    }

    public ICommand NuevoCommand => new AsyncRelayCommand(OpenNewDialog);
    public ICommand ModificarCommand => new AsyncRelayCommand(OpenModificarDialog);
    public ICommand BorrarRecuperarCommand => new AsyncRelayCommand(BorrarRecuperarCommand_Executed);
    public ICommand MovimientosCommand => new RelayCommand(VerMovimientos);
    public ICommand AgregarCommand => new AsyncRelayCommand(AgregarObjeto);
    public ICommand ModificarRegistroCommand => new AsyncRelayCommand(ModificarObjeto);
    public ICommand ExportarCommand => new AsyncRelayCommand(ExportarCommand_Execute);
    public ICommand ImportarCommand => new AsyncRelayCommand(ImportarCommand_Execute);    

    private async Task OpenNewDialog()
    {
        AgregarDialog.PrimaryButtonCommand = AgregarCommand;
        AgregarDialog.IsPrimaryButtonEnabled = false;
        
        AgregarDialog.DataContext = new ObjetosListaDTO() 
        { 
            OBJ_ID_ESTADO_REG = "A", 
            OBJ_INSPEC_CSC = FormatoFecha.FechaEstandar(DateTime.Now.Date), 
            OBJ_OBSERVACIONES = string.Empty,
            OBJ_FECHA_ACTUALIZACION = DateTime.Now.ToString() 
        };

        //Valores x defecto        
        ViewModel.ObjetosViewModel.Matricula = string.Empty;
        TxtFechaCSC.Date = DateTime.Now.Date;
        ComboSiglas.SelectedItem = ViewModel.LstSiglasActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstSiglasActivos.FirstOrDefault();
        ComboModelo.SelectedItem = ViewModel.LstModelosActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstModelosActivos.FirstOrDefault();
        ComboVariante.SelectedItem = ViewModel.LstVariantesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstVariantesActivos.FirstOrDefault();
        ComboTipo.SelectedItem = ViewModel.LstTiposActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstTiposActivos.FirstOrDefault();
        ComboPropietario.SelectedItem = ViewModel.LstPropietariosActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstPropietariosActivos.FirstOrDefault();
        ComboTara.SelectedItem = ViewModel.LstTaraActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstTaraActivos.FirstOrDefault();
        ComboPMP.SelectedItem = ViewModel.LstPmpActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstPmpActivos.FirstOrDefault();
        ComboAlturaExterior.SelectedItem = ViewModel.LstAlturasExteriorActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstAlturasExteriorActivos.FirstOrDefault();
        ComboCuelloCisne.SelectedItem = ViewModel.LstCuellosCisneActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstCuellosCisneActivos.FirstOrDefault();
        ComboBarras.SelectedItem = ViewModel.LstBarrasActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstBarrasActivos.FirstOrDefault();
        ComboCables.SelectedItem = ViewModel.LstCablesActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstCablesActivos.FirstOrDefault();
        ComboLineasVida.SelectedItem = ViewModel.LstLineasVidaActivos.FirstOrDefault(x => x.LISTAS_ID_LISTA > 0) ?? ViewModel.LstLineasVidaActivos.FirstOrDefault();
        colorPicker.Color = Color.FromArgb(255,255,0,0);
        await AgregarDialog.ShowAsync();
    }

    private async Task OpenModificarDialog()
    {
        AgregarDialog.PrimaryButtonCommand = ModificarRegistroCommand;
        AgregarDialog.IsPrimaryButtonEnabled = true;
        AgregarDialog.DataContext = ViewModel.SelectedObjeto;

        //Valores x defecto
        TxtFechaCSC.Date = DateTime.Parse(ViewModel.SelectedObjeto.OBJ_INSPEC_CSC);
        ViewModel.ObjetosViewModel.Matricula = ViewModel.SelectedObjeto.OBJ_MATRICULA!;
        //ViewModel.ObjetosViewModel.IdObjeto = ViewModel.SelectedObjeto.OBJ_ID_OBJETO.ToString();
        ComboSiglas.SelectedItem = ViewModel.LstSiglasActivos.FirstOrDefault(x => x.OBJ_SIGLAS == ViewModel.SelectedObjeto.OBJ_SIGLAS) ?? ViewModel.LstSiglasActivos.FirstOrDefault();
        ComboModelo.SelectedItem = ViewModel.LstModelosActivos.FirstOrDefault(x => x.OBJ_MODELO == ViewModel.SelectedObjeto.OBJ_MODELO) ?? ViewModel.LstModelosActivos.FirstOrDefault();
        ComboVariante.SelectedItem = ViewModel.LstVariantesActivos.FirstOrDefault(x => x.OBJ_VARIANTE == ViewModel.SelectedObjeto.OBJ_VARIANTE) ?? ViewModel.LstVariantesActivos.FirstOrDefault();
        ComboTipo.SelectedItem = ViewModel.LstTiposActivos.FirstOrDefault(x => x.OBJ_TIPO == ViewModel.SelectedObjeto.OBJ_TIPO) ?? ViewModel.LstTiposActivos.FirstOrDefault();
        ComboPropietario.SelectedItem = ViewModel.LstPropietariosActivos.FirstOrDefault(x => x.OBJ_PROPIETARIO == ViewModel.SelectedObjeto.OBJ_PROPIETARIO) ?? ViewModel.LstPropietariosActivos.FirstOrDefault();
        ComboTara.SelectedItem = ViewModel.LstTaraActivos.FirstOrDefault(x => x.OBJ_TARA == ViewModel.SelectedObjeto.OBJ_TARA) ?? ViewModel.LstTaraActivos.FirstOrDefault();
        ComboPMP.SelectedItem = ViewModel.LstPmpActivos.FirstOrDefault(x => x.OBJ_PMP == ViewModel.SelectedObjeto.OBJ_PMP) ?? ViewModel.LstPmpActivos.FirstOrDefault();
        ComboAlturaExterior.SelectedItem = ViewModel.LstAlturasExteriorActivos.FirstOrDefault(x => x.OBJ_ALTURA_EXTERIOR == ViewModel.SelectedObjeto.OBJ_ALTURA_EXTERIOR) ?? ViewModel.LstAlturasExteriorActivos.FirstOrDefault();
        ComboCuelloCisne.SelectedItem = ViewModel.LstCuellosCisneActivos.FirstOrDefault(x => x.OBJ_CUELLO_CISNE == ViewModel.SelectedObjeto.OBJ_CUELLO_CISNE) ?? ViewModel.LstCuellosCisneActivos.FirstOrDefault();
        ComboBarras.SelectedItem = ViewModel.LstBarrasActivos.FirstOrDefault(x => x.OBJ_BARRAS == ViewModel.SelectedObjeto.OBJ_BARRAS) ?? ViewModel.LstBarrasActivos.FirstOrDefault();
        ComboCables.SelectedItem = ViewModel.LstCablesActivos.FirstOrDefault(x => x.OBJ_CABLES == ViewModel.SelectedObjeto.OBJ_CABLES) ?? ViewModel.LstCablesActivos.FirstOrDefault();
        ComboLineasVida.SelectedItem = ViewModel.LstLineasVidaActivos.FirstOrDefault(x => x.OBJ_LINEA_VIDA == ViewModel.SelectedObjeto.OBJ_LINEA_VIDA) ?? ViewModel.LstLineasVidaActivos.FirstOrDefault();
        colorPicker.Color = Colores.HexToColor(ViewModel.SelectedObjeto.OBJ_COLOR);
        await AgregarDialog.ShowAsync();
    }

    private async Task ExportarCommand_Execute()
    {
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "CONTAINERS.csv");

        try
        {
            await Exportar.GenerarDatos(ViewModel.Source, filePath, this.XamlRoot);           
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }
    }

    private async Task ImportarCommand_Execute()
    {
        //await ImportarDialog.ShowAsync();
    }      

    private async Task BorrarRecuperarCommand_Executed()
    {
        var pregunta = ViewModel.EstadoActivo ? Constantes.W_DarBajaRegistro.GetLocalized() : Constantes.W_RecuperarRegistro.GetLocalized();
        ContentDialogResult result = await Dialogs.Pregunta(this.XamlRoot, pregunta);
        if (result == ContentDialogResult.Primary)
        {
            try
            {
                await ViewModel.BorrarRecuperarRegistro();

                grdContainers.ItemsSource = ViewModel.ApplyFilter(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
            }
            catch (Exception ex)
            {
                await Dialogs.Error(XamlRoot, ex.Message);
            }
        }
    }

    private async Task ModificarObjeto()
    {
        var nuevoObjeto = AgregarDialog.DataContext as ObjetosListaDTO;
        //Obtengo el item seleccionado de cada combo
        var siglas = ComboSiglas.SelectedItem as SiglasDTO;
        var modelo = ComboModelo.SelectedItem as ModelosDTO;
        var variante = ComboVariante.SelectedItem as VariantesDTO;
        var tipo = ComboTipo.SelectedItem as TiposDTO;
        var propietario = ComboPropietario.SelectedItem as PropietariosDTO;
        var tara = ComboTara.SelectedItem as TaraDTO;
        var pmp = ComboPMP.SelectedItem as PmpDTO;
        var alturaExterior = ComboAlturaExterior.SelectedItem as AlturasExteriorDTO;
        var cuelloCisne = ComboCuelloCisne.SelectedItem as CuellosCisneDTO;
        var barras = ComboBarras.SelectedItem as BarrasDTO;
        var cables = ComboCables.SelectedItem as CablesDTO;
        var lineasVida = ComboLineasVida.SelectedItem as LineasVidaDTO;

        //asigno los valores al objeto que voy a grabar
        nuevoObjeto.OBJ_MATRICULA = ViewModel.ObjetosViewModel.Matricula;
        //nuevoObjeto.OBJ_ID_OBJETO = Convert.ToInt32(ViewModel.ObjetosViewModel.IdObjeto);
        nuevoObjeto.OBJ_SIGLAS = siglas.OBJ_SIGLAS;
        nuevoObjeto.OBJ_SIGLAS_DESCRIPCION = siglas.DESCRIPCION;
        nuevoObjeto.OBJ_MODELO = modelo.OBJ_MODELO;
        nuevoObjeto.OBJ_MODELO_DESCRIPCION = modelo.DESCRIPCION;
        nuevoObjeto.OBJ_VARIANTE = variante.OBJ_VARIANTE;
        nuevoObjeto.OBJ_VARIANTE_DESCRIPCION = variante.DESCRIPCION;
        nuevoObjeto.OBJ_TIPO = tipo.OBJ_TIPO;
        nuevoObjeto.OBJ_TIPO_DESCRIPCION = tipo.DESCRIPCION;
        nuevoObjeto.OBJ_PROPIETARIO = propietario.OBJ_PROPIETARIO;
        nuevoObjeto.OBJ_PROPIETARIO_DESCRIPCION = propietario.DESCRIPCION;
        nuevoObjeto.OBJ_TARA = tara.OBJ_TARA;
        nuevoObjeto.OBJ_TARA_DESCRIPCION = tara.DESCRIPCION;
        nuevoObjeto.OBJ_PMP = pmp.OBJ_PMP;
        nuevoObjeto.OBJ_PMP_DESCRIPCION = pmp.DESCRIPCION;
        nuevoObjeto.OBJ_ALTURA_EXTERIOR = alturaExterior.OBJ_ALTURA_EXTERIOR;
        nuevoObjeto.OBJ_ALTURA_EXTERIOR_DESCRIPCION = alturaExterior.DESCRIPCION;
        nuevoObjeto.OBJ_CUELLO_CISNE = cuelloCisne.OBJ_CUELLO_CISNE;
        nuevoObjeto.OBJ_CUELLO_CISNE_DESCRIPCION = cuelloCisne.DESCRIPCION;
        nuevoObjeto.OBJ_BARRAS = barras.OBJ_BARRAS;
        nuevoObjeto.OBJ_BARRAS_DESCRIPCION = barras.DESCRIPCION;
        nuevoObjeto.OBJ_CABLES = cables.OBJ_CABLES;
        nuevoObjeto.OBJ_CABLES_DESCRIPCION = cables.DESCRIPCION;
        nuevoObjeto.OBJ_LINEA_VIDA = lineasVida.OBJ_LINEA_VIDA;
        nuevoObjeto.OBJ_LINEA_VIDA_DESCRIPCION = lineasVida.DESCRIPCION;
        nuevoObjeto.OBJ_INSPEC_CSC = FormatoFecha.FechaEstandar(TxtFechaCSC.Date.Value.Date);
        nuevoObjeto.OBJ_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now.Date);
        nuevoObjeto.OBJ_COLOR = Colores.ColorToHex(colorPicker.Color);

        try
        {
            await ViewModel.ActualizarObjeto(nuevoObjeto);

            grdContainers.ItemsSource = ViewModel.ApplyFilter(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
        }
        catch (Exception ex)
        {
            await Dialogs.Error(this.XamlRoot, ex.Message);
        }        
    }

    private async Task AgregarObjeto()
    {
        var nuevoObjeto = AgregarDialog.DataContext as ObjetosListaDTO;

        //Obtengo el item seleccionado de cada combo
        var siglas = ComboSiglas.SelectedItem as SiglasDTO;
        var modelo = ComboModelo.SelectedItem as ModelosDTO;
        var variante = ComboVariante.SelectedItem as VariantesDTO;
        var tipo = ComboTipo.SelectedItem as TiposDTO;
        var propietario = ComboPropietario.SelectedItem as PropietariosDTO;
        var tara = ComboTara.SelectedItem as TaraDTO;
        var pmp = ComboPMP.SelectedItem as PmpDTO;
        var alturaExterior = ComboAlturaExterior.SelectedItem as AlturasExteriorDTO;
        var cuelloCisne = ComboCuelloCisne.SelectedItem as CuellosCisneDTO;
        var barras = ComboBarras.SelectedItem as BarrasDTO;
        var cables = ComboCables.SelectedItem as CablesDTO;
        var lineasVida = ComboLineasVida.SelectedItem as LineasVidaDTO;

        //asigno los valores al objeto que voy a grabar
        nuevoObjeto.OBJ_MATRICULA = ViewModel.ObjetosViewModel.Matricula;
        nuevoObjeto.OBJ_ID_OBJETO = 0; // Convert.ToInt32(ViewModel.ObjetosViewModel.IdObjeto);
        nuevoObjeto.OBJ_SIGLAS = siglas.OBJ_SIGLAS;
        nuevoObjeto.OBJ_SIGLAS_DESCRIPCION = siglas.DESCRIPCION;
        nuevoObjeto.OBJ_MODELO = modelo.OBJ_MODELO;
        nuevoObjeto.OBJ_MODELO_DESCRIPCION = modelo.DESCRIPCION;
        nuevoObjeto.OBJ_VARIANTE = variante.OBJ_VARIANTE;
        nuevoObjeto.OBJ_VARIANTE_DESCRIPCION = variante.DESCRIPCION;
        nuevoObjeto.OBJ_TIPO = tipo.OBJ_TIPO;
        nuevoObjeto.OBJ_TIPO_DESCRIPCION = tipo.DESCRIPCION;
        nuevoObjeto.OBJ_PROPIETARIO = propietario.OBJ_PROPIETARIO;
        nuevoObjeto.OBJ_PROPIETARIO_DESCRIPCION = propietario.DESCRIPCION;
        nuevoObjeto.OBJ_TARA = tara.OBJ_TARA;
        nuevoObjeto.OBJ_TARA_DESCRIPCION = tara.DESCRIPCION;
        nuevoObjeto.OBJ_PMP = pmp.OBJ_PMP;
        nuevoObjeto.OBJ_PMP_DESCRIPCION = pmp.DESCRIPCION;
        nuevoObjeto.OBJ_ALTURA_EXTERIOR = alturaExterior.OBJ_ALTURA_EXTERIOR;
        nuevoObjeto.OBJ_ALTURA_EXTERIOR_DESCRIPCION = alturaExterior.DESCRIPCION;
        nuevoObjeto.OBJ_CUELLO_CISNE = cuelloCisne.OBJ_CUELLO_CISNE;
        nuevoObjeto.OBJ_CUELLO_CISNE_DESCRIPCION = cuelloCisne.DESCRIPCION;
        nuevoObjeto.OBJ_BARRAS = barras.OBJ_BARRAS;
        nuevoObjeto.OBJ_BARRAS_DESCRIPCION = barras.DESCRIPCION;
        nuevoObjeto.OBJ_CABLES = cables.OBJ_CABLES;
        nuevoObjeto.OBJ_CABLES_DESCRIPCION = cables.DESCRIPCION;
        nuevoObjeto.OBJ_LINEA_VIDA = lineasVida.OBJ_LINEA_VIDA;
        nuevoObjeto.OBJ_LINEA_VIDA_DESCRIPCION = lineasVida.DESCRIPCION;
        nuevoObjeto.OBJ_INSPEC_CSC = FormatoFecha.FechaEstandar(TxtFechaCSC.Date.Value.Date);
        nuevoObjeto.OBJ_COLOR = Colores.ColorToHex(colorPicker.Color);

        await ViewModel.CrearObjeto(nuevoObjeto);

        grdContainers.ItemsSource = ViewModel.ApplyFilter(SearchBox.Text, chkMostrarTodos.IsChecked ?? false);
    }

    private void VerMovimientos()
    {
        Frame.Navigate(typeof(MovimientosContainerPage), ViewModel.SelectedObjeto);
    }

    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.QueryText != "")
        {
            grdContainers.ItemsSource = ViewModel.ApplyFilter(args.QueryText, chkMostrarTodos.IsChecked ?? false);
        }        
    }    

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            if (sender.Text == "")
            {
                grdContainers.ItemsSource = ViewModel.ApplyFilter(sender.Text, chkMostrarTodos.IsChecked ?? false);
            }
        }
    }

    private void ContainersDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        // Add sorting indicator, and sort
        var isAscending = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending;
        grdContainers.ItemsSource = ViewModel.SortData(e.Column.Tag.ToString(), isAscending);
        e.Column.SortDirection = isAscending
            ? DataGridSortDirection.Ascending
            : DataGridSortDirection.Descending;

        // Remove sorting indicators from other columns
        foreach (var column in grdContainers.Columns)
        {
            if (column.Tag != null && column.Tag.ToString() != e.Column.Tag.ToString())
            {
                column.SortDirection = null;
            }
        }
    }

    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {
        grdContainers.ItemsSource = ViewModel.ApplyFilter(SearchBox.Text, true);
    }

    private void chkMostrarTodos_Unchecked(object sender, RoutedEventArgs e)
    {
        grdContainers.ItemsSource = ViewModel.ApplyFilter(SearchBox.Text, false);
    }

    private void colorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
    {
        colorPicker.Color = sender.Color;
    }
}
