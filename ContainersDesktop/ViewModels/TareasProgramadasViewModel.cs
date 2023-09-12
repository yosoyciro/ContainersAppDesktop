using System.Collections.ObjectModel;
using System.Data;
using System.Runtime.CompilerServices;
using AutoMapper;
using Azure;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.DTO;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Contracts;
using ContainersDesktop.Logica.Mensajeria.Messages;
using ContainersDesktop.Logica.Services;
using ContainersDesktop.Logica.Specification.Implementaciones;
using CoreDesktop.Dominio.Models;
using CoreDesktop.Logica.Mensajeria.Services;
using Windows.UI;

namespace ContainersDesktop.ViewModels;
public partial class TareasProgramadasViewModel : ObservableRecipient, INavigationAware
{
    private readonly TareasProgramadasFormViewModel _formViewModel = new();
    public TareasProgramadasFormViewModel FormViewModel => _formViewModel;

    private readonly IAsyncRepository<TareaProgramada> _tareasProgramadasRepo;
    private readonly IAsyncRepository<Lista> _listasRepo;
    private readonly IAsyncRepository<Dispositivo> _dispositivosRepo;
    private readonly IAsyncRepository<Objeto> _objetosRepo;
    private readonly IAsyncRepository<DispCalendar> _dispCalendarRepo;
    private readonly SincronizarServicio _sincronizarRepo;
    private readonly IConfigRepository<UI_Config> _configRepository;
    private readonly IMapper _mapper;
    private readonly AzureServiceBus _azureBus;

    private string _cachedSortedColumn = string.Empty;
    //Estilos
    private Color _gridColor;
    private Color _comboColor;

    private TareaProgramadaDTO current;

    public TareaProgramadaDTO Current
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
    public bool EstadoActivo => current?.TAREAS_PROGRAMADAS_ID_ESTADO_REG == "A" ? true : false;
    public bool EstadoBaja => current?.TAREAS_PROGRAMADAS_ID_ESTADO_REG == "B" ? true : false;
    public Color GridColor => _gridColor;
    public Color ComboColor => _comboColor;

    private ObjetosListaDTO _objetosListaDTO;
    [ObservableProperty]
    public ObjetosDTO objeto = null;
    [ObservableProperty]
    public bool isBusy = false;

    #region Observable Collections
    private readonly ObservableCollection<TareaProgramadaDTO> _items = new();
    public ObservableCollection<TareaProgramadaDTO> Items => _items;    
    public ObservableCollection<Lista> LstListas { get; } = new();
    public ObservableCollection<DispositivosDTO> LstDispositivos { get; } = new();
    public ObservableCollection<DispositivosDTO> LstDispositivosActivos { get; } = new();
    public ObservableCollection<ObjetosDTO> LstObjetos { get; } = new();
    public ObservableCollection<ObjetosDTO> LstObjetosActivos { get; } = new();
    public ObservableCollection<AlmacenesDTO> LstAlmacenes { get; } = new();
    public ObservableCollection<AlmacenesDTO> LstAlmacenesActivos { get; } = new();
    //private readonly List<int> _lstHoras = new();
    public List<int> LstHoras { get; } = new();
    public SafeObservableCollection<DateTimeOffset> LstFechasDisponibles { get; } = new();
    #endregion    

    public TareasProgramadasViewModel(
        SincronizarServicio sincronizarServicio,
        IConfigRepository<UI_Config> configRepository,
        IAsyncRepository<TareaProgramada> tareasProgramadasServicio,
        IAsyncRepository<Lista> listasServicio,
        IAsyncRepository<Dispositivo> dispositivosServicio,
        IAsyncRepository<Objeto> objetosServicio,
        IMapper mapper
,
        AzureServiceBus azureBus,
        IAsyncRepository<DispCalendar> dispCalendarRepo)
    {
        _tareasProgramadasRepo = tareasProgramadasServicio;
        _sincronizarRepo = sincronizarServicio;
        _configRepository = configRepository;
        _listasRepo = listasServicio;
        _dispositivosRepo = dispositivosServicio;
        _objetosRepo = objetosServicio;
        _mapper = mapper;
        _azureBus = azureBus;
        _dispCalendarRepo = dispCalendarRepo;

        CargarConfiguracion().Wait();        
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
        var listas = await _listasRepo.GetAsync();
        if (listas.Any())
        {
            foreach (var item in listas)
            {
                LstListas.Add(item);
            }
        }

        //Dispositivos
        LstDispositivos.Clear();
        var dispositivos = await _dispositivosRepo.GetAsync();
        if (dispositivos.Any())
        {
            foreach (var item in dispositivos)
            {
                LstDispositivos.Add(new DispositivosDTO() { MOVIM_ID_DISPOSITIVO = item.ID, DESCRIPCION = item.DISPOSITIVOS_DESCRIP });
                if (item.Estado == "A")
                {
                    LstDispositivosActivos.Add(new DispositivosDTO() { MOVIM_ID_DISPOSITIVO = item.ID, DESCRIPCION = item.DISPOSITIVOS_DESCRIP });
                }
            }
        }

        //Objetos
        LstObjetos.Clear();
        var objetos = await _objetosRepo.GetAsync();
        if (objetos.Any())
        {
            foreach (var item in objetos)
            {
                LstObjetos.Add(new ObjetosDTO() { MOVIM_ID_OBJETO = item.ID, DESCRIPCION = item.OBJ_MATRICULA });
                if (item.Estado == "A")
                {
                    LstObjetosActivos.Add(new ObjetosDTO() { MOVIM_ID_OBJETO = item.ID, DESCRIPCION = item.OBJ_MATRICULA });
                }
            }
        }

        //Ubicaciones
        var lstUbicaciones = LstListas.Where(x => x.LISTAS_ID_LISTA == 3600 || x.ID == 1).ToList();
        foreach (var item in lstUbicaciones)
        {
            LstAlmacenes.Add(new AlmacenesDTO() { MOVIM_ALMACEN = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            if (item.Estado == "A")
            {
                LstAlmacenesActivos.Add(new AlmacenesDTO() { MOVIM_ALMACEN = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
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

        var tareasProgramadas = await _tareasProgramadasRepo.GetAsync();

        if (Objeto is not null)
        {
            tareasProgramadas = tareasProgramadas.Where(x => x.TAREAS_PROGRAMADAS_OBJETO_ID_REG == Objeto.MOVIM_ID_OBJETO).ToList();
        }

        var data = tareasProgramadas;
        if (data.Any())
        {
            foreach (var item in data)
            {
                var tareaProgramadaDTO = _mapper.Map<TareaProgramadaDTO>(item);
                CargarDescripciones(tareaProgramadaDTO);
                _items.Add(tareaProgramadaDTO);
            }
        }
    }

    #endregion

    #region CRUD
    public async Task AgregarTareaProgramada(TareaProgramadaDTO dto)
    {
        try
        {
            var tareaProgramada = _mapper.Map<TareaProgramada>(dto);
            dto.TAREAS_PROGRAMADAS_ID_REG = await _tareasProgramadasRepo.AddAsync(tareaProgramada);
            dto.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaHora(dto.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA!);
            _items.Add(dto);

            var mensaje = _mapper.Map<TareaProgramadaCreada>(tareaProgramada);
            await _azureBus.EnviarMensaje(mensaje);
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
            var tareaProgramada = (TareaProgramada)_mapper.Map(dto, typeof(TareaProgramadaDTO), typeof(TareaProgramada));
            await _tareasProgramadasRepo.UpdateAsync(tareaProgramada);

            var i = _items.IndexOf(Current);
            Current.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaHora(Current.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA!);

            _items[i].TAREAS_PROGRAMADAS_OBJETO_ID_REG = dto.TAREAS_PROGRAMADAS_OBJETO_ID_REG;
            _items[i].TAREAS_PROGRAMADAS_OBJETO_MATRICULA = dto.TAREAS_PROGRAMADAS_OBJETO_MATRICULA;
            _items[i].TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = dto.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA;
            _items[i].TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION = dto.TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION;
            _items[i].TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION = dto.TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION;
            _items[i].TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION = dto.TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION;
            _items[i].TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = dto.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD;
            _items[i].TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = dto.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD;

            var mensaje = _mapper.Map<TareaProgramadaModificada>(tareaProgramada);
            await _azureBus.EnviarMensaje(mensaje);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task BorrarRecuperarRegistro()
    {
        var accion = EstadoActivo ? "B" : "A";
        Current.TAREAS_PROGRAMADAS_ID_ESTADO_REG = accion;
        Current.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);

        try
        {
            var tareaProgramada = (TareaProgramada)_mapper.Map(Current, typeof(TareaProgramadaDTO), typeof(TareaProgramada));
            await _tareasProgramadasRepo.UpdateAsync(tareaProgramada);

            //Actualizo Source
            var i = Items.IndexOf(Current);
            Items[i].TAREAS_PROGRAMADAS_ID_ESTADO_REG = accion;
            Items[i].TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(Current.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION);

            var mensaje = _mapper.Map<TareaProgramadaModificada>(tareaProgramada);
            await _azureBus.EnviarMensaje(mensaje);
        }
        catch (Exception)
        {
            throw;
        }
        
    }

    #endregion

    #region Mapeos

    private void CargarDescripciones(TareaProgramadaDTO tareaProgramadaDTO)
    {
        tareaProgramadaDTO.TAREAS_PROGRAMADAS_OBJETO_MATRICULA = LstObjetos.FirstOrDefault(x => x.MOVIM_ID_OBJETO == tareaProgramadaDTO.TAREAS_PROGRAMADAS_OBJETO_ID_REG).DESCRIPCION;
        tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaHora(tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA);
        tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_COMPLETADA = FormatoFecha.ConvertirAFechaHora(tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_COMPLETADA);
        tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION = LstAlmacenes.FirstOrDefault(x => x.MOVIM_ALMACEN == tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_ORIGEN).DESCRIPCION;
        tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION = LstAlmacenes.FirstOrDefault(x => x.MOVIM_ALMACEN == tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_DESTINO).DESCRIPCION;
        tareaProgramadaDTO.TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION = LstDispositivos.FirstOrDefault(x => x.MOVIM_ID_DISPOSITIVO == tareaProgramadaDTO.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG).DESCRIPCION;
    }

    //private TareaProgramadaDTO MapSourceToDTO(TareaProgramada item)
    //{
    //    var objeto = LstObjetos.Where(x => x.MOVIM_ID_OBJETO == item.TAREAS_PROGRAMADAS_OBJETO_ID_REG).FirstOrDefault();        
    //    var dispositivo = LstDispositivos.Where(x => x.MOVIM_ID_DISPOSITIVO == item.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG).FirstOrDefault();
    //    var ubicacionOrigen = LstAlmacenes.FirstOrDefault(x => x.MOVIM_ALMACEN == item.TAREAS_PROGRAMADAS_UBICACION_ORIGEN);
    //    var ubicacionDestino = LstAlmacenes.FirstOrDefault(x => x.MOVIM_ALMACEN == item.TAREAS_PROGRAMADAS_UBICACION_DESTINO);

    //    return new TareaProgramadaDTO()
    //    {
    //        TAREAS_PROGRAMADAS_ID_REG = item.ID,
    //        TAREAS_PROGRAMADAS_ID_ESTADO_REG = item.TAREAS_PROGRAMADAS_ID_ESTADO_REG,
    //        TAREAS_PROGRAMADAS_OBJETO_ID_REG = item.TAREAS_PROGRAMADAS_OBJETO_ID_REG,
    //        TAREAS_PROGRAMADAS_OBJETO_MATRICULA = objeto.DESCRIPCION,
    //        TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = FormatoFecha.ConvertirAFechaHora(item.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA),
    //        TAREAS_PROGRAMADAS_FECHA_COMPLETADA = FormatoFecha.ConvertirAFechaHora(item.TAREAS_PROGRAMADAS_FECHA_COMPLETADA),
    //        TAREAS_PROGRAMADAS_UBICACION_ORIGEN = item.TAREAS_PROGRAMADAS_UBICACION_ORIGEN,
    //        TAREAS_PROGRAMADAS_UBICACION_ORIGEN_DESCRIPCION = ubicacionOrigen.DESCRIPCION,
    //        TAREAS_PROGRAMADAS_UBICACION_DESTINO = item.TAREAS_PROGRAMADAS_UBICACION_DESTINO,
    //        TAREAS_PROGRAMADAS_UBICACION_DESTINO_DESCRIPCION = ubicacionDestino.DESCRIPCION,
    //        TAREAS_PROGRAMADAS_ORDENADO = item.TAREAS_PROGRAMADAS_ORDENADO,
    //        TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = item.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG,
    //        TAREAS_PROGRAMADAS_DISPOSITIVOS_DESCRIPCION = dispositivo.DESCRIPCION,   
    //        TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = item.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD,
    //        TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = item.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD,
    //        TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = item.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION
    //    };
    //}

    //private TareaProgramada MapDTOToSource(TareaProgramadaDTO tareaProgramadaDTO)
    //{
    //    return new TareaProgramada()
    //    {
    //        ID = tareaProgramadaDTO.TAREAS_PROGRAMADAS_ID_REG,
    //        TAREAS_PROGRAMADAS_ID_ESTADO_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_ID_ESTADO_REG,
    //        TAREAS_PROGRAMADAS_OBJETO_ID_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_OBJETO_ID_REG,
    //        TAREAS_PROGRAMADAS_FECHA_PROGRAMADA = tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_PROGRAMADA,
    //        TAREAS_PROGRAMADAS_FECHA_COMPLETADA = tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_COMPLETADA,
    //        TAREAS_PROGRAMADAS_UBICACION_ORIGEN = tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_ORIGEN,
    //        TAREAS_PROGRAMADAS_UBICACION_DESTINO = tareaProgramadaDTO.TAREAS_PROGRAMADAS_UBICACION_DESTINO,
    //        TAREAS_PROGRAMADAS_ORDENADO = tareaProgramadaDTO.TAREAS_PROGRAMADAS_ORDENADO,
    //        TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG = tareaProgramadaDTO.TAREAS_PROGRAMADAS_DISPOSITIVOS_ID_REG,
    //        TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD = tareaProgramadaDTO.TAREAS_PROGRAMADAS_DISPOSITIVO_LATITUD,
    //        TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD = tareaProgramadaDTO.TAREAS_PROGRAMADAS_DISPOSITIVO_LONGITUD,
    //        TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION = tareaProgramadaDTO.TAREAS_PROGRAMADAS_FECHA_ACTUALIZACION
    //    };
    //}
    #endregion

    #region Ordenamiento y filtro
    public ObservableCollection<TareaProgramadaDTO> AplicarFiltro(string? filter, bool verTodos)
    {
        return new ObservableCollection<TareaProgramadaDTO>(Items.Where(x => (verTodos || x.TAREAS_PROGRAMADAS_ID_ESTADO_REG == "A")));
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

    #region Sincronizacion
    public async Task<bool> SincronizarInformacion()
    {
        try
        {
            IsBusy = true;
            await _sincronizarRepo.Sincronizar();

            return true;
        }
        catch (RequestFailedException)
        {
            throw;
        }
        catch (SystemException)
        {
            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }
    #endregion

    #region Configs

    private async Task CargarConfiguracion()
    {
        var gridColor = await _configRepository.Leer("GridColor");
        _gridColor = Colores.HexToColor(gridColor.Valor!);

        var comboColor = await _configRepository.Leer("ComboColor");
        _comboColor = Colores.HexToColor(comboColor.Valor!);
    }

    #endregion

    #region Fechas y horas
    public async Task CargarFechasYHoras(int idDispositivo)
    {
        LstHoras.Clear();
        LstFechasDisponibles.Clear();

        var spec = new DispCalendarFechasHorasDispositivoSpec(idDispositivo);
        var dispCalendar = await _dispCalendarRepo.GetAllWithSpecsAsync(spec);
        if (dispCalendar.Count == 0)
        {
            for (int i = 0; i < 24; i++)
            {
                LstHoras.Add(i);
            }

            return;
        }

        foreach (var item in dispCalendar)
        {
            var fecha = DateTime.Parse(item.DISP_CALENDAR_FECHA);
            if (!string.IsNullOrEmpty(item.DISP_CALENDAR_FECHA) && fecha >= DateTime.Now.Date)
            {
                LstFechasDisponibles.Add(Convert.ToDateTime(item!.DISP_CALENDAR_FECHA));

                if (item.DISP_CALENDAR_00 == 1) LstHoras.Add(0);
                if (item.DISP_CALENDAR_01 == 1) LstHoras.Add(1);
                if (item.DISP_CALENDAR_02 == 1) LstHoras.Add(2);
                if (item.DISP_CALENDAR_03 == 1) LstHoras.Add(3);
                if (item.DISP_CALENDAR_04 == 1) LstHoras.Add(4);
                if (item.DISP_CALENDAR_05 == 1) LstHoras.Add(5);
                if (item.DISP_CALENDAR_06 == 1) LstHoras.Add(6);
                if (item.DISP_CALENDAR_07 == 1) LstHoras.Add(7);
                if (item.DISP_CALENDAR_08 == 1) LstHoras.Add(8);
                if (item.DISP_CALENDAR_09 == 1) LstHoras.Add(9);
                if (item.DISP_CALENDAR_10 == 1) LstHoras.Add(10);
                if (item.DISP_CALENDAR_11 == 1) LstHoras.Add(11);
                if (item.DISP_CALENDAR_12 == 1) LstHoras.Add(12);
                if (item.DISP_CALENDAR_13 == 1) LstHoras.Add(13);
                if (item.DISP_CALENDAR_14 == 1) LstHoras.Add(14);
                if (item.DISP_CALENDAR_15 == 1) LstHoras.Add(15);
                if (item.DISP_CALENDAR_16 == 1) LstHoras.Add(16);
                if (item.DISP_CALENDAR_17 == 1) LstHoras.Add(17);
                if (item.DISP_CALENDAR_18 == 1) LstHoras.Add(18);
                if (item.DISP_CALENDAR_19 == 1) LstHoras.Add(19);
                if (item.DISP_CALENDAR_20 == 1) LstHoras.Add(20);
                if (item.DISP_CALENDAR_21 == 1) LstHoras.Add(21);
                if (item.DISP_CALENDAR_22 == 1) LstHoras.Add(22);
                if (item.DISP_CALENDAR_23 == 1) LstHoras.Add(23);
            }
        }           
    }

    public List<int> ObtenerHoras()
    {
        return new List<int>(LstHoras);
    }

    #endregion
}
