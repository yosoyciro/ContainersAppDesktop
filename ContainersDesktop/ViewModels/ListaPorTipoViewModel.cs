using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Services;

namespace ContainersDesktop.ViewModels;

public partial class ListaPorTipoViewModel : ObservableRecipient, INavigationAware
{    
    public ObservableCollection<Listas> Source {
        get;
    } = new ();
    private Listas current;
    public Listas SelectedLista
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }
    public bool HasCurrent => current is not null;

    private ClaList claLista = new();
    private readonly IListasServicio _listasServicio;
    public ListaPorTipoViewModel(IListasServicio listasServicio)
    {
        _listasServicio = listasServicio;
    }

    public void OnNavigatedFrom()
    {
    
    }

    public async void OnNavigatedTo(object parameter)
    {
        Console.WriteLine("parameter", parameter);
        claLista = parameter as ClaList;
        Source.Clear();

        var data = await _listasServicio.ObtenerListas();

        foreach (var item in data.Where(x => x.LISTAS_ID_LISTA == claLista!.CLALIST_ID_REG).OrderBy(x => x.LISTAS_ID_LISTA_ORDEN))
        {
            Source.Add(item);
        }
    }

    public async Task BorrarLista()
    {
        await _listasServicio.BorrarLista(SelectedLista.LISTAS_ID_REG);
        Source.Remove(SelectedLista);
    }

    public async Task AgregarLista(Listas lista)
    {
        lista.LISTAS_ID_LISTA = claLista.CLALIST_ID_REG;
        await _listasServicio.CrearLista(lista);
        Source.Add(lista);
    }

    public async Task ActualizarLista(Listas lista)
    {        
        await _listasServicio.ActualizarLista(lista);
        var i = Source.IndexOf(lista);
        Source[i].LISTAS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaCorta(DateTime.Now.Date.ToShortDateString());
    }
}
