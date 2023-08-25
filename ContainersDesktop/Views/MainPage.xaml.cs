using ContainersDesktop.Helpers;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;

public sealed partial class MainPage : Page
{    
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
        Loaded += MainPage_Loaded;
        
    }

    private async void MainPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var login = ViewModel.Login;
        txtBienvenido.Text = "Bienvenido " + login.Usuario;

        //var result = await ViewModel.RecibirMensajes();
        //if (!string.IsNullOrEmpty(result))
        //{
        //    await Dialogs.Aviso(this.XamlRoot, result);
        //};

        await ViewModel.VerificarConfiguracion();
    }
}
