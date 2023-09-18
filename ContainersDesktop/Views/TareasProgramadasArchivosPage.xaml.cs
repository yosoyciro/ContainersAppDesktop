using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.Views;

public sealed partial class TareasProgramadasArchivosPage : Page
{
    public TareasProgramadasArchivosViewModel ViewModel { get; }
    public TareasProgramadasArchivosPage()
    {
        ViewModel = App.GetService<TareasProgramadasArchivosViewModel>();
        this.InitializeComponent();
        Loaded += TareasProgramadasArchivosPage_Loaded;
    }

    private async void TareasProgramadasArchivosPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.CargarSource();
        grdTareasProgramadasArchivos.ItemsSource = ViewModel.AplicarFiltro();
    }

    #region AppBar commands buttons
    public ICommand VolverCommand => new RelayCommand(VolverCommand_Executed);
    private void VolverCommand_Executed()
    {
        Frame.GoBack();
    }


    #endregion
}
