using System.Collections.ObjectModel;
using Azure;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Helpers;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Mensajeria;
using ContainersDesktop.Logica.Services;
using Windows.UI;

namespace ContainersDesktop.ViewModels;
public partial class SincronizacionesViewModel : ObservableRecipient
{
    private readonly IAsyncRepository<Sincronizacion> _sincronizacionRepo; 
    private readonly SincronizarServicio _sincronizarServicio;
    private readonly IConfigRepository<UI_Config> _configRepository;
    private readonly MensajesNotificacionesViewModel _mensajesNotificacionesViewModel;

    //Estilos
    private Color _gridColor;
    private Color _comboColor;

    private Sincronizacion current;
    public Sincronizacion Current
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }
    public bool HasCurrent => current is not null;
    public Color GridColor => _gridColor;
    public Color ComboColor => _comboColor;

    public ObservableCollection<Sincronizacion> Source { get; } = new();
    private string _cachedSortedColumn = string.Empty;

    [ObservableProperty]
    public bool isBusy = false;

    public SincronizacionesViewModel(
        IAsyncRepository<Sincronizacion> sincronizacionServicio, 
        SincronizarServicio sincronizarServicio, 
        IConfigRepository<UI_Config> configRepository
        )
    {
        _sincronizacionRepo = sincronizacionServicio;
        _sincronizarServicio = sincronizarServicio;
        _configRepository = configRepository;
        _mensajesNotificacionesViewModel = ServiceHelper.GetService<MensajesNotificacionesViewModel>();

        CargarConfiguracion().Wait();
    }

    public async Task CargarSource()
    {
        Source.Clear();
        var data = await _sincronizacionRepo.GetAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public async Task<bool> SincronizarInformacion()
    {
        try
        {
            IsBusy = true;
            var sinProcesar = await _sincronizarServicio.Sincronizar();
            _mensajesNotificacionesViewModel.SetMensajesNoLeidos(sinProcesar);
            //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            //() =>
            //    { 
            //        MensajesNotificaciones.NumeroMensajesNoProcesados;
            //    }
            //);

            return true;
        }
        catch (RequestFailedException)
        {
            throw;
        }
        catch (SystemException)
        {
            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }

    #region Configs

    private async Task CargarConfiguracion()
    {
        var gridColor = await _configRepository.Leer("GridColor");
        _gridColor = Colores.HexToColor(gridColor.Valor!);

        var comboColor = await _configRepository.Leer("ComboColor");
        _comboColor = Colores.HexToColor(comboColor.Valor!);
    }

    #endregion
}
