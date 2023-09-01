using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Contracts;
using CoreDesktop.Logica.Contracts;

namespace ContainersDesktop.ViewModels;

public partial class ListaPorTipoViewModel : ObservableRecipient, INavigationAware
{
    private ListasPorTipoFormViewModel _formViewModel = new();
    public ListasPorTipoFormViewModel FormViewModel => _formViewModel;
    public ObservableCollection<Lista> Source {
        get;
    } = new();
    private Lista current;
    public Lista SelectedLista
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
            OnPropertyChanged(nameof(EstadoActivo));
            OnPropertyChanged(nameof(EstadoBaja));
        }
    }
    public bool HasCurrent => current is not null;
    public bool EstadoActivo => current?.Estado == "A" ? true : false;
    public bool EstadoBaja => current?.Estado == "B" ? true : false;

    public ClaList claLista = new();
    private readonly IAsyncRepository<Lista> _listasRepo;
    private string _cachedSortedColumn = string.Empty;

    public ListaPorTipoViewModel(IAsyncRepository<Lista> listasServicio)
    {
        _listasRepo = listasServicio;
    }

    public void OnNavigatedFrom()
    {

    }

    public void OnNavigatedTo(object parameter)
    {
        Console.WriteLine("parameter", parameter);
        claLista = parameter as ClaList;
    }

    public async Task CargarSource()
    {
        Source.Clear();

        var data = await _listasRepo.GetAsync();

        foreach (var item in data
            .Where(x => x.LISTAS_ID_LISTA == claLista!.ID)
            .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN))
        {
            item.FechaActualizacion = FormatoFecha.ConvertirAFechaHora(item.FechaActualizacion!);
            Source.Add(item);
        }
    }

    public async Task BorrarRecuperarLista()
    {
        var accion = EstadoActivo ? "B" : "A";
        SelectedLista.Estado = accion;
        SelectedLista.FechaActualizacion = FormatoFecha.FechaEstandar(DateTime.Now);
        await _listasRepo.UpdateAsync(SelectedLista);

        //Actualizo Source
        var i = Source.IndexOf(SelectedLista);              
        Source[i] = SelectedLista;
        Source[i].FechaActualizacion = FormatoFecha.ConvertirAFechaHora(SelectedLista.FechaActualizacion);
    }

    public async Task AgregarLista(Lista lista)
    {
        lista.LISTAS_ID_LISTA = claLista.ID;
        lista.ID = await _listasRepo.AddAsync(lista);
        if (lista.ID > 0)
        {
            Source.Add(lista);
        }
    }

    public async Task ActualizarLista(Lista lista)
    {
        await _listasRepo.UpdateAsync(lista);
        var i = Source.IndexOf(lista);
        Source[i] = lista;
    }

    #region Filtros y ordenamiento
    public ObservableCollection<Lista> AplicarFiltro(string? filter, bool verTodos)
    {
        return new ObservableCollection<Lista>(Source.Where(x =>
            (string.IsNullOrEmpty(filter) || x.LISTAS_ID_LISTA_DESCRIP.Contains(filter, StringComparison.OrdinalIgnoreCase)) &&
            (verTodos || x.Estado == "A")
        ));
    }

    public string CachedSortedColumn
    {
        get
        {
            return _cachedSortedColumn;
        }

        set
        {
            _cachedSortedColumn = value;
        }
    }

    public ObservableCollection<Lista> SortData(string sortBy, bool ascending)
    {
        _cachedSortedColumn = sortBy;
        switch (sortBy)
        {
            case "Id":
                if (ascending)
                {
                    return new ObservableCollection<Lista>(from item in Source
                                                             orderby item.ID ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Lista>(from item in Source
                                                             orderby item.ID descending
                                                             select item);
                }

            case "Orden":
                if (ascending)
                {
                    return new ObservableCollection<Lista>(from item in Source
                                                             orderby item.LISTAS_ID_LISTA_ORDEN ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Lista>(from item in Source
                                                             orderby item.LISTAS_ID_LISTA_ORDEN descending
                                                             select item);
                }

            case "Descripcion":
                if (ascending)
                {
                    return new ObservableCollection<Lista>(from item in Source
                                                             orderby item.LISTAS_ID_LISTA_DESCRIP ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Lista>(from item in Source
                                                             orderby item.LISTAS_ID_LISTA_DESCRIP descending
                                                             select item);
                }            
        }

        return Source;
    }
    #endregion
}
