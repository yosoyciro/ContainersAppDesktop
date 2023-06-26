using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.Core.Models;

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

    public ICommand VerCommand => new AsyncRelayCommand(OpenNewDialog);

    private async Task OpenNewDialog()
    {
        VerDialog.Title = "Ver movimiento";
        VerDialog.DataContext = new Movim();
        await VerDialog.ShowAsync();
    }
}
