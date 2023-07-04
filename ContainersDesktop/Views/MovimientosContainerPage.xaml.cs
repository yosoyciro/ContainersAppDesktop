using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Core.Models;
using ContainersDesktop.ViewModels;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ContainersDesktop.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MovimientosContainerPage : Page
{
    public MovimientosViewModel ViewModel
    {
        get;
    }

    public MovimientosContainerPage()
    {
        ViewModel = App.GetService<MovimientosViewModel>();
        this.InitializeComponent();
        Loaded += MovimientosContainerPage_Loaded;
    }

    private void MovimientosContainerPage_Loaded(object sender, RoutedEventArgs e)
    {
    
    }

    public ICommand VerCommand => new AsyncRelayCommand(OpenNewDialog);
    public ICommand VolverCommand => new RelayCommand(Volver);

    private async Task OpenNewDialog()
    {
        VerDialog.Title = "Ver movimiento";
        VerDialog.DataContext = new Movim();
        await VerDialog.ShowAsync();
    }

    private void Volver()
    {
        Frame.GoBack();
    }

    private void chkMostrarTodos_Checked(object sender, RoutedEventArgs e)
    {

    }
}
