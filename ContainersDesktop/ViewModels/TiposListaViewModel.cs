using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;

namespace ContainersDesktop.ViewModels;
public partial class TiposListaViewModel : BaseViewModel<ClaList>
{
    private readonly IAsyncRepositoryObservable<ClaList> _claListRepo;
    //[ObservableProperty]
    //public string filter;
    //public ObservableCollection<ClaList> Items { get; set; } = new();

    //private ClaList current;
    //public ClaList SelectedClaList
    //{
    //    get => current;
    //    set
    //    {
    //        SetProperty(ref current, value);
    //        OnPropertyChanged(nameof(HasCurrent));
    //    }
    //}
    //public bool HasCurrent => current is not null;

    public override bool ApplyFilter(ClaList item, string filter)
    {
        return item.ApplyFilter(filter);
    }

    public TiposListaViewModel(IAsyncRepositoryObservable<ClaList> claListServicio)
    {
        _claListRepo = claListServicio;
    }

    public async Task CargarSource()
    {
        Items.Clear();

        var data = await _claListRepo.GetAsync();

        foreach (var item in data.OrderBy(x => x.ClaList_Descrip).Where(x => x.Estado == "A" && !string.IsNullOrEmpty(x.ClaList_Descrip)))
        {
            Items.Add(item);
        }
    }

    //public ObservableCollection<ClaList> AplicarFiltro(string? filter)
    //{
    //    return new ObservableCollection<ClaList>(Items.Where(x => 
    //        (string.IsNullOrEmpty(filter) || x.CLALIST_DESCRIP.Contains(filter, StringComparison.OrdinalIgnoreCase))
    //    ));
    //}

    //public bool ApplyFilterViewModel(string filter)
    //{
    //    //return !string.IsNullOrEmpty(filter);
    //    return ;
    //}

    //public async Task BorrarLista()
    //{

    //    await _claListRepo.UpdateAsync(Current);
    //    DeleteItem(Current);
    //}

    //public async Task AgregarLista(ClaList claList)
    //{
    //    await _claListRepo.AddAsync(claList);
    //    AddItem(claList);
    //}
}
