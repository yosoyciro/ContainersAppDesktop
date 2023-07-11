﻿using System.Collections.ObjectModel;
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
    } = new();
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
    private string _cachedSortedColumn = string.Empty;

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
        await CargarSource();
    }

    private async Task CargarSource()
    {
        Source.Clear();

        var data = await _listasServicio.ObtenerListas();

        foreach (var item in data
            .Where(x => x.LISTAS_ID_LISTA == claLista!.CLALIST_ID_REG)
            .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN))
        {
            Source.Add(item);
        }
    }

    public async Task BorrarLista()
    {
        await _listasServicio.BorrarLista(SelectedLista.LISTAS_ID_REG);
        await CargarSource();
    }

    public async Task AgregarLista(Listas lista)
    {
        lista.LISTAS_ID_LISTA = claLista.CLALIST_ID_REG;
        var result = await _listasServicio.CrearLista(lista);
        if (result > 0)
        {
            await CargarSource();
        }

    }

    public async Task ActualizarLista(Listas lista)
    {
        await _listasServicio.ActualizarLista(lista);
        await CargarSource();
    }

    #region Filtros y ordenamiento
    public ObservableCollection<Listas> AplicarFiltro(string? filter, bool verTodos)
    {
        return new ObservableCollection<Listas>(Source.Where(x =>
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

    public ObservableCollection<Listas> SortData(string sortBy, bool ascending)
    {
        _cachedSortedColumn = sortBy;
        switch (sortBy)
        {
            case "Id":
                if (ascending)
                {
                    return new ObservableCollection<Listas>(from item in Source
                                                             orderby item.LISTAS_ID_REG ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Listas>(from item in Source
                                                             orderby item.LISTAS_ID_REG descending
                                                             select item);
                }

            case "Orden":
                if (ascending)
                {
                    return new ObservableCollection<Listas>(from item in Source
                                                             orderby item.LISTAS_ID_LISTA_ORDEN ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Listas>(from item in Source
                                                             orderby item.LISTAS_ID_LISTA_ORDEN descending
                                                             select item);
                }

            case "Descripcion":
                if (ascending)
                {
                    return new ObservableCollection<Listas>(from item in Source
                                                             orderby item.LISTAS_ID_LISTA_DESCRIP ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Listas>(from item in Source
                                                             orderby item.LISTAS_ID_LISTA_DESCRIP descending
                                                             select item);
                }            
        }

        return Source;
    }
    #endregion
}