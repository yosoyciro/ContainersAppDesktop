using System.Windows.Forms.Design.Behavior;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Contracts.Services;
using ContainersDesktop.Dominio.Models.Menu;
using ContainersDesktop.Helpers;
using ContainersDesktop.Logica.Services.ModelosStorage;
using ContainersDesktop.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

using Windows.System;

namespace ContainersDesktop.Views;

// TODO: Update NavigationViewItem titles and icons in ShellPage.xaml.
public sealed partial class ShellPage : Page
{
    private List<Menu> Menues = new();
    public ShellViewModel ViewModel
    {
        get;
    }
    public static ShellPage Current;

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
        Current = this;
    }

    private void OnLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));        
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        var resource = args.WindowActivationState == WindowActivationState.Deactivated ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

        AppTitleBarText.Foreground = (SolidColorBrush)App.Current.Resources[resource];
        App.AppTitlebar = AppTitleBarText as UIElement;
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    public void ShowPane()
    {
        NavigationViewControl.IsPaneVisible = true;
    }

    public void RefrescarItems()
    {
        //var item = NavigationViewControl.MenuItems.Where(x => x.)
        Console.WriteLine(NavigationViewControl.MenuItems);
    }

    private async void NavigationViewControl_Loaded(object sender, RoutedEventArgs e)
    {
        // Armo el menu
        NavigationViewControl.MenuItems.Add(new NavigationViewItem()
        {
            Content = Constantes.Menu_Inicio.GetLocalized(),
            Icon = new SymbolIcon(Symbol.Home),
            Tag = "Inicio"
        });

        if (!ViewModel.SubModulos.Any())
        {
            var modulos = await ViewModel._azureTableStorage.LeerTableStorage<Modulos>(nameof(Modulos));
            var moduloContenedores = modulos.FirstOrDefault(x => x.Nombre == "Contenedores Marítimos");
            ViewModel.SubModulos = await ViewModel._azureTableStorage.LeerTableStorage<SubModulos>(nameof(SubModulos), "PartitionKey", moduloContenedores!.RowKey);
        }

        foreach (var item in ViewModel.SubModulos.OrderBy(x => x.Orden))
        {
            // Armo el menu
            var rutas = item.RutaSubModulos.Split(new[] { "\\#\\" }, StringSplitOptions.None);

            for (int i = 0; i < rutas.Length; i++)
            {
                if (i == 0)
                {
                    var items = GetNavigationViewItems().FirstOrDefault(x => x.Content.ToString() == rutas[0]);
                    if (items == null)
                    {
                        var glyph = i == rutas.Length-1 ? item.Icono : "e70d";
                        var icon = (char)int.Parse(glyph, System.Globalization.NumberStyles.HexNumber);
                        var fontIcon = new FontIcon() { Glyph = icon.ToString(), FontFamily = new FontFamily("Segoe MDL2 Assets") };
                        NavigationViewControl.MenuItems.Add(new NavigationViewItem()
                        {
                            Content = rutas[0],
                            Icon = fontIcon,
                            Tag = rutas[0]
                        });
                    }                    
                }
                if (i == 1)
                {
                    var item1 = GetNavigationViewItems().FirstOrDefault(x => x.Content.ToString() == rutas[0]);
                    if (item1 != null)
                    {
                        var glyph = i == rutas.Length-1 ? item.Icono : "E70D";
                        var icon = (char)int.Parse(glyph, System.Globalization.NumberStyles.HexNumber);
                        var fontIcon = new FontIcon() { Glyph = icon.ToString(), FontFamily = new FontFamily("Segoe MDL2 Assets") };

                        item1.MenuItems.Add(new NavigationViewItem()
                        {
                            Content = rutas[1],
                            Icon = fontIcon,
                            Tag = rutas[1]
                        });
                    }
                }

                if (i == 2)
                {
                    var item2 = GetNavigationViewItems().FirstOrDefault(x => x.Content.ToString() == rutas[1]);
                    if (item2 != null)
                    {
                        item2.MenuItems.Add(new NavigationViewItem()
                        {
                            Content = rutas[2],
                            //Icon = new SymbolIcon(Symbol.Previous),
                            Tag = rutas[2]
                        });
                    }
                }

                if (i == 3)
                {
                    var item3 = GetNavigationViewItems().FirstOrDefault(x => x.Content.ToString() == rutas[2]);
                    if (item3 != null)
                    {
                        item3.MenuItems.Add(new NavigationViewItem()
                        {
                            Content = rutas[3],
                            //Icon = new SymbolIcon(Symbol.Previous),
                            Tag = rutas[3]
                        });
                    }
                }
            }
        }
        
        // set the initial SelectedItem 
        foreach (NavigationViewItemBase item in NavigationViewControl.MenuItems)
        {
            if (item is NavigationViewItem && item.Tag.ToString() == "Inicio")
            {
                NavigationViewControl.SelectedItem = item;
                break;
            }
        }
    }

    private void NavigationViewControl_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            NavigationFrame.Navigate(typeof(SettingsPage));
        }
        else
        {
            // find NavigationViewItem with Content that equals InvokedItem
            var item = sender.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(x => (string)x.Content == (string)args.InvokedItem);
            if (item == null) 
            {
                // 2do nivel
                foreach (var subMenu in NavigationViewControl.MenuItems.OfType<NavigationViewItem>())
                {
                    item = subMenu.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(x => (string)x.Content == (string)args.InvokedItem);
                    if (item != null)
                    {
                        break;
                    }
                }
            }
            NavigationViewControl_Navigate(item as NavigationViewItem);
        }
    }

    private void NavigationViewControl_Navigate(NavigationViewItem item)
    {
        switch (item.Tag)
        {
            case "Inicio":
                NavigationFrame.Navigate(typeof(MainPage));
                break;

            case "Historial":
                NavigationFrame.Navigate(typeof(MovimientosPage));
                break;

            case "Programar":
                NavigationFrame.Navigate(typeof(TareasProgramadasPage));
                break;

            case "data2movie":
                NavigationFrame.Navigate(typeof(Data2MoviePage));
                break;

            case "Listas":
                NavigationFrame.Navigate(typeof(TiposListaDetailsPage));
                break;

            case "Dispositivos":
                NavigationFrame.Navigate(typeof(DispositivosPage));
                break;

            case "Containers":
                NavigationFrame.Navigate(typeof(ContainersGridPage));
                break;

            case "Sincronizaciones":
                NavigationFrame.Navigate(typeof(SincronizacionesPage));
                break;
        }
    }

    private List<NavigationViewItem> GetNavigationViewItems()
    {
        var result = new List<NavigationViewItem>();
        var items = NavigationViewControl.MenuItems.Select(i => (NavigationViewItem)i).ToList();
        items.AddRange(NavigationViewControl.FooterMenuItems.Select(i => (NavigationViewItem)i));
        result.AddRange(items);

        foreach (NavigationViewItem mainItem in items)
        {
            result.AddRange(mainItem.MenuItems.Select(i => (NavigationViewItem)i));
        }

        return result;
    }
}
