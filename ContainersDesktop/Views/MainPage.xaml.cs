﻿using ContainersDesktop.ViewModels;
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
        var login = await ViewModel.GetUsuarioLogueado();
        txtBienvenido.Text = "Bienvenido " + login.Usuario;
    }
}
