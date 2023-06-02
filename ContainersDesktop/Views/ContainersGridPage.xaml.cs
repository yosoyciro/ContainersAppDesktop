using System.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class ContainersGridPage : Page
{
    private Objetos objetoAgregar = new();
    private DateTime FechaInicial;
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

    private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
    {
        //if (e.Column is DataGridTemplateColumn column && (string)column?.Header == "Fecha Inspec" &&
        //    e.EditingElement is CalendarDatePicker calendar)
        //{
        //    calendar.IsCalendarOpen = true;
        //}
    }

    private void btnAgregar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.CrearNuevoObjeto();
    }       

    private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
        Console.WriteLine("Begins edit");
    }

    private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        
    }

    private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
        Console.WriteLine(e.Row.DataContext);
        try
        {
            //objetoAgregar.OBJ_INSPEC_CSC = Convert.ToDateTime(FechaInspecDatePicker.Date).ToShortDateString();
            ViewModel.ActualizarObjeto(objetoAgregar);
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
        }        
    }

    private void dpDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (args.NewDate != null)
        {
            objetoAgregar.OBJ_INSPEC_CSC = Convert.ToDateTime(args.NewDate!.Value.Date).ToShortDateString();
        }
        
    }
}
