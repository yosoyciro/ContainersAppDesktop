﻿using CommunityToolkit.Mvvm.ComponentModel;

using ContainersDesktop.Contracts.Services;
using ContainersDesktop.Views;

using Microsoft.UI.Xaml.Navigation;

namespace ContainersDesktop.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }
    public LoginViewModel _loginViewModel;

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService, LoginViewModel loginViewModel)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        _loginViewModel = loginViewModel;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}
