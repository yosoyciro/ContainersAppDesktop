using System.Collections.ObjectModel;
using Azure;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Contracts.Services;
using ContainersDesktop.Infraestructura.Contracts.Services.Config;
using ContainersDesktop.Logica.Services;
using Windows.UI;

namespace ContainersDesktop.ViewModels;
public partial class SincronizacionesViewModel : ObservableRecipient
{
    private readonly ISincronizacionServicio _sincronizacionServicio; 
    private readonly SincronizarServicio _sincronizarServicio;
    private readonly IConfigRepository<UI_Config> _configRepository;

    //Estilos
    private Color _gridColor;
    private Color _comboColor;

    private Sincronizaciones current;
    public Sincronizaciones Current
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

    public ObservableCollection<Sincronizaciones> Source { get; } = new();
    private string _cachedSortedColumn = string.Empty;
    [ObservableProperty]
    public bool isBusy = false;

    public SincronizacionesViewModel(ISincronizacionServicio sincronizacionServicio, SincronizarServicio sincronizarServicio, IConfigRepository<UI_Config> configRepository)
    {
        _sincronizacionServicio = sincronizacionServicio;
        _sincronizarServicio = sincronizarServicio;
        _configRepository = configRepository;

        CargarConfiguracion().Wait();
    }

    public async Task CargarSource()
    {
        Source.Clear();
        var data = await _sincronizacionServicio.ObtenerSincronizaciones();

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
            await _sincronizarServicio.Sincronizar();

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
