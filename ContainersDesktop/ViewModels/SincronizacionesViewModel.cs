using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;

namespace ContainersDesktop.ViewModels;
public class SincronizacionesViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISincronizacionServicio _sincronizacionServicio;    

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

    public SincronizacionesViewModel(ISincronizacionServicio sincronizacionServicio)
    {
        _sincronizacionServicio = sincronizacionServicio;
    }

    public void OnNavigatedFrom()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        await CargarSource();
    }

    private async Task CargarSource()
    {
        Source.Clear();
        var data = await _sincronizacionServicio.ObtenerSincronizaciones();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }
}
