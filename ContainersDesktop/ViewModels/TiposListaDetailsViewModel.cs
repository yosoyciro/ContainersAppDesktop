using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;

namespace ContainersDesktop.ViewModels;
public partial class TiposListaDetailsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IClaListServicio _claListServicio;
    [ObservableProperty]
    public string filter;
    public ObservableCollection<ClaList> Items { get; set; } = new();

    private ClaList current;
    public ClaList SelectedClaList
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }
    public bool HasCurrent => current is not null;

    public TiposListaDetailsViewModel(IClaListServicio claListServicio)
    {
        _claListServicio = claListServicio;
    }

    public void OnNavigatedFrom()
    {        
    }

    public async void OnNavigatedTo(object parameter)
    {
        Items.Clear();

        // TODO: Replace with real data.
        var data = await _claListServicio.ObtenerClaListas();

        foreach (var item in data.OrderBy(x => x.CLALIST_DESCRIP).Where(x => x.CLALIST_ID_ESTADO_REG == "A" && !string.IsNullOrEmpty(x.CLALIST_DESCRIP)))
        {
            Items.Add(item);
        }
    }

    public ObservableCollection<ClaList> AplicarFiltro(string? filter)
    {
        return new ObservableCollection<ClaList>(Items.Where(x => 
            (string.IsNullOrEmpty(filter) || x.CLALIST_DESCRIP.Contains(filter, StringComparison.OrdinalIgnoreCase))
        ));
    }

    public async Task BorrarLista()
    {
        await _claListServicio.BorrarClaLista(SelectedClaList.CLALIST_ID_REG);
        Items.Remove(SelectedClaList);
    }

    public async Task AgregarLista(ClaList claList)
    {
        await _claListServicio.CrearClaLista(claList);
        Items.Add(claList);
    }
}
