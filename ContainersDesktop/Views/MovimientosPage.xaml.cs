using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.Core.Models;
using Microsoft.UI.Xaml;
using ContainersDesktop.Core.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContainersDesktop.Views;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
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
    }

    #region Commands
    public ICommand NuevoCommand => new AsyncRelayCommand(OpenNewDialog);
    public ICommand BorrarCommand => new AsyncRelayCommand(BorrarMovimiento);
    public ICommand VerCommand => new AsyncRelayCommand(OpenVerDialog);
    public ICommand VolverCommand => new RelayCommand(Volver);
    public ICommand AgregarCommand => new AsyncRelayCommand(AgregarMovimiento);
    public ICommand MostrarTodosCommand => new RelayCommand(MostrarTodos);

    private async Task OpenNewDialog()
    {
        AgregarDialog.Title = "Agregar tarea";
        AgregarDialog.PrimaryButtonCommand = AgregarCommand;
        AgregarDialog.DataContext = new Movim();
        await AgregarDialog.ShowAsync();
    }

    private async Task OpenVerDialog()
    {
        AgregarDialog.Title = "Ver tarea";
        AgregarDialog.DataContext = ViewModel.Current;
        await AgregarDialog.ShowAsync();
    }

    private async Task BorrarMovimiento()
    {
        try
        {
            await ViewModel.BorrarMovimiento();
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

    private async Task AgregarMovimiento()
    {
        await ViewModel.AgregarMovimiento(AgregarDialog.DataContext as Movim);
    }
    

    private void Volver()
    {
        Frame.GoBack();
    }

    private void MostrarTodos()
    {

    }
    #endregion

    #region Operaciones sobre el grid y content dialog
    private void MovimientosGrid_RowEditEnding(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEditEndingEventArgs e)
    {

    }
    private void dpDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        var movim = AgregarDialog.DataContext as Movim;
        if (args.NewDate != null && movim != null)
        {
            movim.MOVIM_FECHA = Convert.ToDateTime(args.NewDate!.Value.Date).ToShortDateString();
        }
    }
    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {

    }
    #endregion


}
