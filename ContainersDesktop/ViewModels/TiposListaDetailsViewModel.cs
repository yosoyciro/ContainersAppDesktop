using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;

namespace ContainersDesktop.ViewModels;
public partial class TiposListaDetailsViewModel : ObservableRecipient
{
    private readonly IAsyncRepository<ClaList> _claListRepo;
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

    public TiposListaDetailsViewModel(IAsyncRepository<ClaList> claListServicio)
    {
        _claListRepo = claListServicio;
    }

    public async Task CargarSource()
    {
        Items.Clear();

        var data = await _claListRepo.GetAsync();

        foreach (var item in data.OrderBy(x => x.CLALIST_DESCRIP).Where(x => x.Estado == "A" && !string.IsNullOrEmpty(x.CLALIST_DESCRIP)))
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

        await _claListRepo.UpdateAsync(SelectedClaList);
        Items.Remove(SelectedClaList);
    }

    public async Task AgregarLista(ClaList claList)
    {
        await _claListRepo.AddAsync(claList);
        Items.Add(claList);
    }
}
