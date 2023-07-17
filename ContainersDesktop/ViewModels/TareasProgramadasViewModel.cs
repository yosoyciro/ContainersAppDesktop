using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using ContainersDesktop.DTO;

namespace ContainersDesktop.ViewModels;
public partial class TareasProgramadasViewModel : ObservableRecipient, INavigationAware
{
    private readonly ITareasProgramadasServicio _tareasProgramadasServicio;
    private readonly IListasServicio _listasServicio;
    private readonly IDispositivosServicio _dispositivosServicio;
    private readonly IObjetosServicio _objetosServicio;
    private string _cachedSortedColumn = string.Empty;

    private TareaProgramadaDTO current;
    public TareaProgramadaDTO Current
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }
    public bool HasCurrent => current is not null;

    [ObservableProperty]
    public ObjetosDTO objeto = null;

    #region Observable Collections
    public ObservableCollection<TareaProgramadaDTO> Items
    {
        get;
    } = new();
    public ObservableCollection<Listas> LstListas { get; } = new();
    public ObservableCollection<DispositivosDTO> LstDispositivos { get; } = new();
    public ObservableCollection<DispositivosDTO> LstDispositivosActivos { get; } = new();
    public ObservableCollection<ObjetosDTO> LstObjetos { get; } = new();
    public ObservableCollection<ObjetosDTO> LstObjetosActivos { get; } = new();
    public ObservableCollection<AlmacenesDTO> LstAlmacenes { get; } = new();
    public ObservableCollection<AlmacenesDTO> LstAlmacenesActivos { get; } = new();
    #endregion

    public TareasProgramadasViewModel(ITareasProgramadasServicio tareasProgramadasServicio, IListasServicio listasServicio, IDispositivosServicio dispositivosServicio, IObjetosServicio objetosServicio)
    {
        _tareasProgramadasServicio = tareasProgramadasServicio;
        _listasServicio = listasServicio;
        _dispositivosServicio = dispositivosServicio;
        _objetosServicio = objetosServicio;
    }
    public void OnNavigatedFrom()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        await CargarListas();
        await CargarSource(parameter as ObjetosListaDTO);
    }

    #region Listas y Source
    private async Task CargarSource(ObjetosListaDTO objeto)
    {
        //Objeto
        if (objeto is not null)
        {
            Objeto = new ObjetosDTO()
            {
                MOVIM_ID_OBJETO = objeto.OBJ_ID_REG,
                DESCRIPCION = objeto.OBJ_MATRICULA,
            };
        }

        Items.Clear();
        var data = Objeto != null ? await _tareasProgramadasServicio.ObtenerPorObjeto(objeto.OBJ_ID_REG) : await _tareasProgramadasServicio.ObtenerTodos();
        if (data.Any())
        {
            foreach (var item in data)
            {
                var dto = MapSourceToDTO(item);
                Items.Add(dto);
            }
        }
    }

    private async Task CargarListas()
    {
        //Cargo Listas
        LstListas.Clear();
        var listas = await _listasServicio.ObtenerListas();
        if (listas.Any())
        {
            foreach (var item in listas)
            {
                LstListas.Add(item);
            }
        }

        //Dispositivos
        LstDispositivos.Clear();
        var dispositivos = await _dispositivosServicio.ObtenerDispositivos();
        if (dispositivos.Any())
        {
            foreach (var item in dispositivos)
            {
                LstDispositivos.Add(new DispositivosDTO() { MOVIM_ID_DISPOSITIVO = item.DISPOSITIVOS_ID_REG, DESCRIPCION = item.DISPOSITIVOS_DESCRIP });
                if (item.DISPOSITIVOS_ID_ESTADO_REG == "A")
                {
                    LstDispositivosActivos.Add(new DispositivosDTO() { MOVIM_ID_DISPOSITIVO = item.DISPOSITIVOS_ID_REG, DESCRIPCION = item.DISPOSITIVOS_DESCRIP });
                }
            }
        }

        //Objetos
        LstObjetos.Clear();
        var objetos = await _objetosServicio.ObtenerObjetos();
        if (objetos.Any())
        {
            foreach (var item in objetos)
            {
                LstObjetos.Add(new ObjetosDTO() { MOVIM_ID_OBJETO = item.OBJ_ID_REG, DESCRIPCION = item.OBJ_MATRICULA });
                if (item.OBJ_ID_ESTADO_REG == "A")
                {
                    LstObjetosActivos.Add(new ObjetosDTO() { MOVIM_ID_OBJETO = item.OBJ_ID_REG, DESCRIPCION = item.OBJ_MATRICULA });
                }
            }
        }

        //Ubicaciones
        var lstUbicaciones = LstListas.Where(x => x.LISTAS_ID_LISTA == 3600 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstUbicaciones)
        {
            LstAlmacenes.Add(new AlmacenesDTO() { MOVIM_ALMACEN = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            if (item.LISTAS_ID_ESTADO_REG == "A")
            {
                LstAlmacenesActivos.Add(new AlmacenesDTO() { MOVIM_ALMACEN = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            }
        }        
    }

    #endregion

    #region Mapeos
    private TareaProgramadaDTO MapSourceToDTO(TareaProgramada item)
    {
        var objeto = LstObjetos.Where(x => x.MOVIM_ID_OBJETO == item.TAREAS_PROGRAMADAS_OBJETO_ID_REG).FirstOrDefault();        
        var dispositivo = LstDispositivos.Where(x => x.MOVIM_ID_DISPOSITIVO == item.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG).FirstOrDefault();
        var ubicacionOrigen = LstAlmacenes.FirstOrDefault(x => x.MOVIM_ALMACEN == item.TAREAS_PROGRAMADAS_UBICACION_ORIGEN);
        var ubicacionDestino = LstAlmacenes.FirstOrDefault(x => x.MOVIM_ALMACEN == item.TAREAS_PROGRAMADAS_UBICACION_DESTINO);

        return new TareaProgramadaDTO()
        {
            TAREAS_PROGRAMADAS_ID_REG = item.TAREAS_PROGRAMADAS_ID_REG,
            TAREAS_PROGRAMADAS_OBJETO_ID_REG = item.TAREAS_PROGRAMADAS_OBJETO_ID_REG,
            TAREAS_PROGRAMADAS_OBJETO_MATRICULA = objeto.DESCRIPCION,
            TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = item.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA,
            TAREAS_PROGRAMADAS_FECHA_COMPLETADA = item.TAREAS_PROGRAMADAS_FECHA_COMPLETADA,
            TAREAS_PROGRAMADAS_UBICACION_ORIGEN = item.TAREAS_PROGRAMADAS_UBICACION_ORIGEN,
            TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION = ubicacionOrigen.DESCRIPCION,
            TAREAS_PROGRAMADAS_UBICACION_DESTINO = item.TAREAS_PROGRAMADAS_UBICACION_DESTINO,
            TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION = ubicacionDestino.DESCRIPCION,
            TAREAS_PROGRAMADAS_ORDENADO = item.TAREAS_PROGRAMADAS_ORDENADO,
            TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = item.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG,
            TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION = dispositivo.DESCRIPCION,            
        };
    }

    private TareaProgramada MapDTOToSource(TareaProgramadaDTO tareaProgramadaDTO)
    {
        return new TareaProgramada()
        {
            TAREAS_PROGRAMADAS_ID_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_ID_REG,
            TAREAS_PROGRAMADAS_OBJETO_ID_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_OBJETO_ID_REG,
            TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA,
            TAREAS_PROGRAMADAS_FECHA_COMPLETADA = tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_COMPLETADA,
            TAREAS_PROGRAMADAS_UBICACION_ORIGEN = tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_ORIGEN,
            TAREAS_PROGRAMADAS_UBICACION_DESTINO = tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_DESTINO,
            TAREAS_PROGRAMADAS_ORDENADO = tareaProgramadaDTO.TAREAS_PROGRAMADAS_ORDENADO,
            TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG,
        };
    }
    #endregion

    #region Ordenamiento y filtro
    //public ObservableCollection<TareaProgramadaDTO> ApplyFilter(string? filter, bool verTodos)
    //{
    //    return new ObservableCollection<MovimDTO>(Items.Where(x =>
    //        (verTodos || x.MOVIM_ID_ESTADO_REG == "A")
    //    ));
    //}

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

    public ObservableCollection<TareaProgramadaDTO> SortData(string sortBy, bool ascending)
    {
        _cachedSortedColumn = sortBy;
        switch (sortBy)
        {
            case "Container":
                if (ascending)
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_OBJETO_MATRICULA ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_OBJETO_MATRICULA descending
                                                              select item);
                }

            case "FechaProgramada":
                if (ascending)
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA descending
                                                              select item);
                }

            case "FechaCompletada":
                if (ascending)
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_FECHA_COMPLETADA ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_FECHA_COMPLETADA descending
                                                              select item);
                }

            case "UbicacionOrigen":
                if (ascending)
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION descending
                                                              select item);
                }

            case "UbicacionDestino":
                if (ascending)
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION descending
                                                              select item);
                }

            case "Dispositivo":
                if (ascending)
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<TareaProgramadaDTO>(from item in Items
                                                              orderby item.TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION descending
                                                              select item);
                }            
        }

        return Items;
    }
    #endregion
}
