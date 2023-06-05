using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Services;
using ContainersDesktop.DTO;

namespace ContainersDesktop.ViewModels;

public partial class ListasGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IListasServicio _listasServicio;
    private readonly IClaListServicio _claListServicio;

    public ObservableCollection<Listas> Source { get; } = new();
    public List<ClaList> LstClaList { get; } = new();
    public ObservableCollection<ClaListDTO> LstClaListDTO { get; } = new();

    public ListasGridViewModel(IListasServicio listasServicio, IClaListServicio claListServicio)
    {
        _listasServicio = listasServicio;
        _claListServicio = claListServicio;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();
        //ClaList
        LstClaList.Clear();
        var claListListas = await _claListServicio.ObtenerClaListas();
        if (claListListas.Any())
        {
            foreach (var item in claListListas)
            {
                LstClaList.Add(item);
            }
        }

        foreach (var item in LstClaList)
        {
            LstClaListDTO.Add(new ClaListDTO() { LISTAS_ID_LISTA = item.CLALIST_ID_REG, DESCRIPCION = item.CLALIST_DESCRIP });
        }

        await LlenarSource(false);
    }

    public void OnNavigatedFrom()
    {
    }

    public async void GuardarLista(Listas lista)
    {
        if (lista != null && lista.LISTAS_ID_REG == 0)
        //var result = await _objetosServicio.ObtenerObjetoPorId(objeto.OBJ_ID_REG);
        {
            await _listasServicio.CrearLista(lista);
        }
        else
        {
            await _listasServicio.ActualizarLista(lista);

        }

        //await LlenarSource(false);
    }

    public async void BorrarLista(Listas lista)
    {
        await _listasServicio.BorrarLista(lista.LISTAS_ID_REG);
        Source.Remove(lista);
    }

    public async Task LlenarSource(bool verTodos)
    {        
        var data = await _listasServicio.ObtenerListas(verTodos);

        foreach (var item in data.OrderBy(x => x.LISTAS_ID_LISTA).ThenBy(x => x.LISTAS_ID_LISTA_ORDEN))
        {
            Source.Add(item);
        }
    }
}
