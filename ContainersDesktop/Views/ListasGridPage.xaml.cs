using ContainersDesktop.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class ListasGridPage : Page
{
    public ListasGridViewModel ViewModel
    {
        get;
    }

    public ListasGridPage()
    {
        ViewModel = App.GetService<ListasGridViewModel>();
        InitializeComponent();
    }
}
