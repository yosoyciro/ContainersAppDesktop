using System.Collections.ObjectModel;
using Azure;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Contracts.Services;
using ContainersDesktop.Logica.Services;

namespace ContainersDesktop.ViewModels;
public partial class SincronizacionesViewModel : ObservableRecipient
{
    private readonly ISincronizacionServicio _sincronizacionServicio; 
    private readonly SincronizarServicio _sincronizarServicio;

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
    public ObservableCollection<Sincronizaciones> Source { get; } = new();
    private string _cachedSortedColumn = string.Empty;
    [ObservableProperty]
    public bool isBusy = false;

    public SincronizacionesViewModel(ISincronizacionServicio sincronizacionServicio, SincronizarServicio sincronizarServicio)
    {
        _sincronizacionServicio = sincronizacionServicio;
        _sincronizarServicio = sincronizarServicio;
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
}
