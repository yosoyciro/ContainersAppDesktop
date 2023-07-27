using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;
public sealed partial class Data2MoviePage : Page
{
    public Data2MovieViewModel ViewModel
    {
        get;
    }
    
    public Data2MoviePage()
    {
        ViewModel = App.GetService<Data2MovieViewModel>();
        InitializeComponent();
        Loaded += Data2MoviePage_Loaded;        
    }

    private async void Data2MoviePage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var login = await ViewModel.ObtenerLogin();
        //var password = ViewModel.ObtenerPassword();
        string[] args = { login.Usuario, login.Password };
        var pathProyecto = ViewModel.ObtenerPathProyecto();

        System.Diagnostics.Process.Start(pathProyecto, args);

        Frame.GoBack();
    }
}
