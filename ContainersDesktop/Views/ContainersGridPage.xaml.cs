using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using CommunityToolkit.WinUI.UI.Controls;
using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace ContainersDesktop.Views;

public sealed partial class ContainersGridPage : Page
{
    private Objetos objetoAgregar = new();
    //private DateTime FechaInicial;
    private CalendarDatePicker FechaInspecDatePicker = new();
    public ContainersGridViewModel ViewModel
    {
        get;
    }

    public ContainersGridPage()
    {
        ViewModel = App.GetService<ContainersGridViewModel>();
        InitializeComponent();
    }

    //private void btnAgregar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{
    //    //ViewModel.CrearNuevoObjeto();
    //    var nuevoRegistro = new Objetos()
    //    {
    //        OBJ_MATRICULA = "NUEVO CONTAINER",
    //        OBJ_ID_ESTADO_REG = "A",
    //        OBJ_SIGLAS_LISTA = 1000,
    //        OBJ_SIGLAS = 1,
    //        OBJ_MODELO_LISTA = 1100,
    //        OBJ_MODELO = 1,
    //        OBJ_ID_OBJETO = 1198,
    //        OBJ_VARIANTE_LISTA = 1200,
    //        OBJ_VARIANTE = 1,
    //        OBJ_TIPO_LISTA = 1300,
    //        OBJ_TIPO = 1,
    //        OBJ_INSPEC_CSC = "",
    //        OBJ_PROPIETARIO_LISTA = 1400,
    //        OBJ_PROPIETARIO = 1,
    //        OBJ_TARA_LISTA = 1500,
    //        OBJ_TARA = 1,
    //        OBJ_PMP_LISTA = 1600,
    //        OBJ_PMP = 1,
    //        OBJ_CARGA_UTIL = 0,
    //        OBJ_ALTURA_EXTERIOR_LISTA = 1700,
    //        OBJ_ALTURA_EXTERIOR = 1,
    //        OBJ_CUELLO_CISNE_LISTA = 1800,
    //        OBJ_CUELLO_CISNE = 1,
    //        OBJ_BARRAS_LISTA = 1900,
    //        OBJ_BARRAS = 1,
    //        OBJ_CABLES_LISTA = 2000,
    //        OBJ_CABLES = 1,
    //        OBJ_LINEA_VIDA_LISTA = 2100,
    //        OBJ_LINEA_VIDA = 1,
    //        OBJ_OBSERVACIONES = ""
    //    };
    //    ViewModel.Source.Add(nuevoRegistro);
    //}       
    
    private async void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
        Console.WriteLine(e.Row.DataContext);
        try
        {
            //objetoAgregar.OBJ_INSPEC_CSC = Convert.ToDateTime(FechaInspecDatePicker.Date).ToShortDateString();
            await ViewModel.ActualizarObjeto(objetoAgregar);
        }
        catch (Exception ex)
        {
            //Alert.Show(ex.Message);
        }
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ContainersDataGrid.SelectedItem != null)
        {
            objetoAgregar = ContainersDataGrid!.SelectedItem as Objetos; //Entity Object
            FechaInspecDatePicker.Date = objetoAgregar!.OBJ_INSPEC_CSC == "" ? DateTime.Now.Date : Convert.ToDateTime(objetoAgregar.OBJ_INSPEC_CSC);

            ViewModel.CargarMovimientos(objetoAgregar);
            Console.WriteLine("Movims", ViewModel.Movims);
            var childGrids = FindVisualChildren<DataGrid>(ContainersDataGrid);
            foreach (DataGrid childGrid in childGrids)
            {
                //Console.WriteLine("childs", childGrid);
                var dataGridMovimientos = childGrid as DataGrid;
                dataGridMovimientos.ItemsSource = ViewModel.MovimsDTO;
            }
        }        
    }

    private void dpDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (args.NewDate != null)
        {
            objetoAgregar.OBJ_INSPEC_CSC = Convert.ToDateTime(args.NewDate!.Value.Date).ToShortDateString();
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
    public ICommand BorrarCommand => new AsyncRelayCommand(BorrarObjeto);
    public ICommand AgregarCommand => new AsyncRelayCommand(AgregarObjeto);

    private async Task OpenNewDialog()
    {
        AgregarDialog.Title = "Nuevo dispositivo";
        AgregarDialog.PrimaryButtonText = "Agregar";
        AgregarDialog.PrimaryButtonCommand = AgregarCommand;
        AgregarDialog.DataContext = new Dispositivos();
        await AgregarDialog.ShowAsync();
    }

    private async Task BorrarObjeto()
    {
        try
        {
            await ViewModel.BorrarObjeto();
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

    private async Task AgregarObjeto()
    {
        await ViewModel.CrearObjeto(AgregarDialog.DataContext as Objetos);
    }
}
