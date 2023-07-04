using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;

public sealed partial class SincronizacionesPage : Page
{
    public SincronizacionesViewModel ViewModel
    {
        get;
    }
    public SincronizacionesPage()
    {
        ViewModel = App.GetService<SincronizacionesViewModel>();
        InitializeComponent();
    }

    private void DispositivosGrid_Sorting(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridColumnEventArgs e)
    {

    }
}
