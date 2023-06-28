using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace ContainersDesktop.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel
    {
        get;
    }
    
    public LoginPage()
    {
        ViewModel = App.GetService<LoginViewModel>();
        this.InitializeComponent();        
    }

    private async void PassportSignInButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorMessage.Text = "";

        var result = await ViewModel.Login(txtUsuario.Text, txtPassword.Text);
        if (result)
        {
            ShellPage.Current.ShowPane();
            Frame.Navigate(typeof(MainPage));
        }
    }
    private void RegisterButtonTextBlock_OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ErrorMessage.Text = "";
    }
}
