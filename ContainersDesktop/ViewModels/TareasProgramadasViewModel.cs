using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Services;
using ContainersDesktop.DTO;

namespace ContainersDesktop.ViewModels;
public partial class TareasProgramadasViewModel : ObservableRecipient, INavigationAware
{
    private readonly TareasProgramadasFormViewModel _formViewModel = new();
    public TareasProgramadasFormViewModel FormViewModel => _formViewModel;

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

    private ObjetosListaDTO _objetosListaDTO;
    [ObservableProperty]
    public ObjetosDTO objeto = null;

    #region Observable Collections
    private readonly ObservableCollection<TareaProgramadaDTO> _items = new();
    public ObservableCollection<TareaProgramadaDTO> Items => _items;    
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

    public void OnNavigatedTo(object parameter)
    {
        _objetosListaDTO = parameter as ObjetosListaDTO;
    }

    #region Listas y Source
    public async Task CargarListasSource()
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
                //LstUbicacionesOrigen.Add(new UbicacionOrigenDTO() { MOVIM_ALMACEN = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                //LstUbicacionesDestino.Add(new UbicacionDestinoDTO() { MOVIM_ALMACEN = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            }
        }

        //Objeto
        if (_objetosListaDTO is not null)
        {
            Objeto = new ObjetosDTO()
            {
                MOVIM_ID_OBJETO = _objetosListaDTO.OBJ_ID_REG,
                DESCRIPCION = _objetosListaDTO.OBJ_MATRICULA,
            };
        }

        _items.Clear();
        var data = Objeto != null ? await _tareasProgramadasServicio.ObtenerPorObjeto(Objeto.MOVIM_ID_OBJETO) : await _tareasProgramadasServicio.ObtenerTodos();
        if (data.Any())
        {
            foreach (var item in data)
            {
                var dto = MapSourceToDTO(item);
                _items.Add(dto);
            }
        }
    }

    #endregion

    #region CRUD
    public async Task AgregarTareaProgramada(TareaProgramadaDTO dto)
    {
        try
        {
            var tareaProgramada = MapDTOToSource(dto);
            dto.TAREAS_PROGRAMADAS_ID_REG = await _tareasProgramadasServicio.Agregar(tareaProgramada);
            dto.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaHora(dto.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA!);
            _items.Add(dto);

            OnPropertyChanged(nameof(Items));
        }
        catch (Exception)
        {
            throw;
        }        
    }

    public async Task ActualizarTareaProgramada(TareaProgramadaDTO dto)
    {
        try
        {            
            var tareaProgramada = MapDTOToSource(dto);
            await _tareasProgramadasServicio.Modificar(tareaProgramada);

            var i = _items.IndexOf(Current);
            dto.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaCorta(dto.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA!);

            _items[i].TAREAS_PROGRAMADAS_OBJETO_ID_REG = dto.TAREAS_PROGRAMADAS_OBJETO_ID_REG;
            _items[i].TAREAS_PROGRAMADAS_OBJETO_MATRICULA = dto.TAREAS_PROGRAMADAS_OBJETO_MATRICULA;
            _items[i].TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = dto.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA;
            _items[i].TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION = dto.TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION;
            _items[i].TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION = dto.TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION;
            _items[i].TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION = dto.TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION;
            _items[i].TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = dto.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD;
            _items[i].TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = dto.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD;

            OnPropertyChanged(nameof(Items));
        }
        catch (Exception)
        {
            throw;
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
            TAREAS_PROGRAMADAS_ID_ESTADO_REG = item.TAREAS_PROGRAMADAS_ID_ESTADO_REG,
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
            TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = item.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD,
            TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = item.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD,
        };
    }

    private TareaProgramada MapDTOToSource(TareaProgramadaDTO tareaProgramadaDTO)
    {
        return new TareaProgramada()
        {
            TAREAS_PROGRAMADAS_ID_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_ID_REG,
            TAREAS_PROGRAMADAS_ID_ESTADO_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_ID_ESTADO_REG,
            TAREAS_PROGRAMADAS_OBJETO_ID_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_OBJETO_ID_REG,
            TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA,
            TAREAS_PROGRAMADAS_FECHA_COMPLETADA = tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_COMPLETADA,
            TAREAS_PROGRAMADAS_UBICACION_ORIGEN = tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_ORIGEN,
            TAREAS_PROGRAMADAS_UBICACION_DESTINO = tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_DESTINO,
            TAREAS_PROGRAMADAS_ORDENADO = tareaProgramadaDTO.TAREAS_PROGRAMADAS_ORDENADO,
            TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG,
            TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = tareaProgramadaDTO.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD,
            TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = tareaProgramadaDTO.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD,
        };
    }
    #endregion

    #region Ordenamiento y filtro
    public ObservableCollection<TareaProgramadaDTO> AplicarFiltro(string? filter, bool verTodos)
    {
        return new ObservableCollection<TareaProgramadaDTO>(Items);
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
