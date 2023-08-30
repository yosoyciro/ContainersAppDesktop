using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Logic.Contracts;
using CoreDesktop.Logic.Contracts;

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
    public bool EstadoActivo => current?.LISTAS_ID_ESTADO_REG == "A" ? true : false;
    public bool EstadoBaja => current?.LISTAS_ID_ESTADO_REG == "B" ? true : false;

    public ClaList claLista = new();
    private readonly IServiciosRepositorios<Lista> _listasServicio;
    private string _cachedSortedColumn = string.Empty;

    public ListaPorTipoViewModel(IServiciosRepositorios<Lista> listasServicio)
    {
        _listasServicio = listasServicio;
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

        var data = await _listasServicio.GetAsync();

        foreach (var item in data
            .Where(x => x.LISTAS_ID_LISTA == claLista!.ID)
            .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN))
        {
            Source.Add(item);
        }
    }

    public async Task BorrarRecuperarLista()
    {
        var accion = EstadoActivo ? "B" : "A";
        SelectedLista.LISTAS_ID_ESTADO_REG = accion;
        SelectedLista.LISTAS_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);
        await _listasServicio.DeleteRecover(SelectedLista);

        //Actualizo Source
        var i = Source.IndexOf(SelectedLista);              
        Source[i] = SelectedLista;
        Source[i].LISTAS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(SelectedLista.LISTAS_FECHA_ACTUALIZACION);
    }

    public async Task AgregarLista(Lista lista)
    {
        lista.LISTAS_ID_LISTA = claLista.ID;
        lista.ID = await _listasServicio.AddAsync(lista);
        if (lista.ID > 0)
        {
            Source.Add(lista);
        }
    }

    public async Task ActualizarLista(Lista lista)
    {
        await _listasServicio.UpdateAsync(lista);
        var i = Source.IndexOf(lista);
        Source[i] = lista;
    }

    #region Filtros y ordenamiento
    public ObservableCollection<Lista> AplicarFiltro(string? filter, bool verTodos)
    {
        return new ObservableCollection<Lista>(Source.Where(x =>
            (string.IsNullOrEmpty(filter) || x.LISTAS_ID_LISTA_DESCRIP.Contains(filter, StringComparison.OrdinalIgnoreCase)) &&
            (verTodos || x.LISTAS_ID_ESTADO_REG == "A")
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
