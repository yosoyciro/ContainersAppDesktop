using System;
using System.Net.Mail;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace ContainersDesktop.Views;

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
        //txtUsuario.Text = "cdaniele@unicom.es";
        //txtPassword.Password = "123abcZz12+";
    }

    private async void PassportSignInButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorMessage.Text = "";

        var result = await ViewModel.Login(txtUsuario.Text, txtPassword.Password);
        //var result = await ViewModel.Login("cdaniele@unicom.es", "123abcZz12+");
        if (result)
        {
            //ShellPage.Current.ShowPane();
            Frame.Navigate(typeof(MainPage));
        }
        else
        {
            ErrorMessage.Text = "Usuario/password incorrecto";
        }        
    }

    private void RegisterButtonTextBlock_OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ErrorMessage.Text = "";
    }

    public bool EmailEsValido(string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        else { return false; }

    }

    private void txtUsuario_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {        
        if (string.IsNullOrEmpty(txtPassword.Password) || !EmailEsValido(sender.Text))
        {
            btnLogin.IsEnabled = false;
        }
        else
        {
            btnLogin.IsEnabled = true;
        }
    }

    private void txtPassword_PasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
    {
        if (!EmailEsValido(txtUsuario.Text) || string.IsNullOrEmpty(sender.Password))
        {
            btnLogin.IsEnabled = false;
        }
        else
        {
            btnLogin.IsEnabled = true;
        }
    }
}
