﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models;
using ContainersDesktop.DTO;

namespace ContainersDesktop.ViewModels;
public partial class MovimientosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IMovimientosServicio _movimientosServicio;
    private readonly IListasServicio _listasServicio;
    private readonly IDispositivosServicio _dispositivosServicio;
    private readonly IObjetosServicio _objetosServicio;
    private string _cachedSortedColumn = string.Empty;

    private MovimDTO current;
    public MovimDTO Current
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
    public ObservableCollection<MovimDTO> Items
    { get; } = new();
    public ObservableCollection<Listas> LstListas { get; } = new();
    public ObservableCollection<DispositivosDTO> LstDispositivos { get; } = new();
    public ObservableCollection<DispositivosDTO> LstDispositivosActivos { get; } = new();
    public ObservableCollection<ObjetosDTO> LstObjetos { get; } = new();
    public ObservableCollection<ObjetosDTO> LstObjetosActivos { get; } = new();
    public ObservableCollection<TiposMovimientoDTO> LstTiposMovimiento { get; } = new();
    public ObservableCollection<TiposMovimientoDTO> LstTiposMovimientoActivos { get; } = new();
    public ObservableCollection<PesosDTO> LstPesos { get; } = new();
    public ObservableCollection<PesosDTO> LstPesosActivos { get; } = new();
    public ObservableCollection<TransportistasDTO> LstTransportistas { get; } = new();
    public ObservableCollection<TransportistasDTO> LstTransportistasActivos { get; } = new();
    public ObservableCollection<ClientesDTO> LstClientes { get; } = new();
    public ObservableCollection<ClientesDTO> LstClientesActivos { get; } = new();
    public ObservableCollection<ChoferesDTO> LstChoferes { get; } = new();
    public ObservableCollection<ChoferesDTO> LstChoferesActivos { get; } = new();
    public ObservableCollection<EntradaSalidaDTO> LstEntradaSalida { get; } = new();
    public ObservableCollection<EntradaSalidaDTO> LstEntradaSalidaActivos { get; } = new();
    public ObservableCollection<AlmacenesDTO> LstAlmacenes { get; } = new();
    public ObservableCollection<AlmacenesDTO> LstAlmacenesActivos { get; } = new();
    #endregion

    public MovimientosViewModel(IMovimientosServicio movimientosServicio, IListasServicio listasServicio, IDispositivosServicio dispositivosServicio, IObjetosServicio objetosServicio)
    {
        _movimientosServicio = movimientosServicio;
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


    #region CRUD
    public async Task BorrarMovimiento()
    {
        await _movimientosServicio.BorrarMovimiento(Current.MOVIM_ID_REG);
        //var item = Items.FirstOrDefault(x => x.MOVIM_ID_REG == Current.MOVIM_ID_REG);
        Items.Remove(Current);
    }

    public async Task AgregarMovimiento(MovimDTO movimDTO)
    {
        var movim = GenerarMovim(movimDTO);
        movimDTO.MOVIM_ID_REG = await _movimientosServicio.CrearMovimiento(movim);
        Items.Add(movimDTO);
    }

    public async Task ActualizarMovimiento(MovimDTO movimDTO)
    {
        try
        {
            var movim = GenerarMovim(movimDTO);
            await _movimientosServicio.ActualizarMovimiento(movim);
            var i = Items.IndexOf(movimDTO);
            movimDTO.MOVIM_FECHA = FormatoFecha.ConvertirAFechaCorta(movimDTO.MOVIM_FECHA);
            Items[i] = movimDTO;
        }
        catch (Exception ex)
        {

            throw ex;
        }
        
    }
    #endregion

    #region Listas y Source
    private async Task CargarSource(ObjetosListaDTO objeto)
    {
        //Movimientos
        if (objeto is not null)        
        {
            Objeto = new ObjetosDTO()
            {
                MOVIM_ID_OBJETO = objeto.OBJ_ID_REG,
                DESCRIPCION = objeto.OBJ_MATRICULA,
            };
        }        

        Items.Clear();
        var data = Objeto != null ? await _movimientosServicio.ObtenerMovimientosObjeto(objeto.OBJ_ID_REG) : await _movimientosServicio.ObtenerMovimientosTodos();
        if (data.Any())
        {
            foreach (var item in data)
            {
                var dto = GenerarDTO(item);
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

        //Listas relacionadas
        var lstTipoMovimiento = LstListas.Where(x => x.LISTAS_ID_LISTA == 3000 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstTipoMovimiento)
        {
            LstTiposMovimiento.Add(new TiposMovimientoDTO() { MOVIM_TIPO_MOVIM = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            if (item.LISTAS_ID_ESTADO_REG == "A")
            {
                LstTiposMovimientoActivos.Add(new TiposMovimientoDTO() { MOVIM_TIPO_MOVIM = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            }
        }

        var lstPesos = LstListas.Where(x => x.LISTAS_ID_LISTA == 3100 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstPesos)
        {
            LstPesos.Add(new PesosDTO() { MOVIM_PESO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            if (item.LISTAS_ID_ESTADO_REG == "A")
            {
                LstPesosActivos.Add(new PesosDTO() { MOVIM_PESO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            }
        }

        var lstTransportistas = LstListas.Where(x => x.LISTAS_ID_LISTA == 3200 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstTransportistas)
        {
            LstTransportistas.Add(new TransportistasDTO() { MOVIM_TRANSPORTISTA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            if (item.LISTAS_ID_ESTADO_REG == "A")
            {
                LstTransportistasActivos.Add(new TransportistasDTO() { MOVIM_TRANSPORTISTA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            }
        }

        var lstClientes = LstListas.Where(x => x.LISTAS_ID_LISTA == 3300 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstClientes)
        {
            LstClientes.Add(new ClientesDTO() { MOVIM_CLIENTE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            if (item.LISTAS_ID_ESTADO_REG == "A")
            {
                LstClientesActivos.Add(new ClientesDTO() { MOVIM_CLIENTE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            }
        }

        var lstChoferes = LstListas.Where(x => x.LISTAS_ID_LISTA == 3400 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstChoferes)
        {
            LstChoferes.Add(new ChoferesDTO() { MOVIM_CHOFER = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            if (item.LISTAS_ID_ESTADO_REG == "A")
            {
                LstChoferesActivos.Add(new ChoferesDTO() { MOVIM_CHOFER = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            }
        }

        var lstEntradaSalida = LstListas.Where(x => x.LISTAS_ID_LISTA == 3500 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstEntradaSalida)
        {
            LstEntradaSalida.Add(new EntradaSalidaDTO() { MOVIM_ENTRADA_SALIDA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            if (item.LISTAS_ID_ESTADO_REG == "A")
            {
                LstEntradaSalidaActivos.Add(new EntradaSalidaDTO() { MOVIM_ENTRADA_SALIDA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            }
        }

        var lstAlmacenes = LstListas.Where(x => x.LISTAS_ID_LISTA == 3600 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstAlmacenes)
        {
            LstAlmacenes.Add(new AlmacenesDTO() { MOVIM_ALMACEN = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            if (item.LISTAS_ID_ESTADO_REG == "A")
            {
                LstAlmacenesActivos.Add(new AlmacenesDTO() { MOVIM_ALMACEN = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
            }
        }
    }

    #endregion

    #region Generar Objetos
    private MovimDTO GenerarDTO(Movim item)
    {
        var objeto = LstObjetos.Where(x => x.MOVIM_ID_OBJETO == item.MOVIM_ID_OBJETO).FirstOrDefault();
        var tipoMovimiento = LstTiposMovimiento.Where(x => x.MOVIM_TIPO_MOVIM == item.MOVIM_TIPO_MOVIM).FirstOrDefault();
        var peso = LstPesos.Where(x => x.MOVIM_PESO == item.MOVIM_PESO).FirstOrDefault();
        var transportista = LstTransportistas.Where(x => x.MOVIM_TRANSPORTISTA == item.MOVIM_TRANSPORTISTA).FirstOrDefault();
        var cliente = LstClientes.Where(x => x.MOVIM_CLIENTE == item.MOVIM_CLIENTE).FirstOrDefault();
        var chofer = LstChoferes.Where(x => x.MOVIM_CHOFER == item.MOVIM_CHOFER).FirstOrDefault();
        var entradaSalida = LstEntradaSalida.Where(x => x.MOVIM_ENTRADA_SALIDA == item.MOVIM_ENTRADA_SALIDA).FirstOrDefault();
        var almacen = LstAlmacenes.Where(x => x.MOVIM_ALMACEN == item.MOVIM_ALMACEN).FirstOrDefault();
        var dispositivo = LstDispositivos.Where(x => x.MOVIM_ID_DISPOSITIVO == item.MOVIM_ID_DISPOSITIVO).FirstOrDefault();

        return new MovimDTO()
        {
            MOVIM_ID_REG = item.MOVIM_ID_REG,
            MOVIM_ID_REG_MOBILE = 0,
            MOVIM_ID_ESTADO_REG = item.MOVIM_ID_ESTADO_REG,
            MOVIM_ID_OBJETO = objeto.MOVIM_ID_OBJETO,
            MOVIM_MATRICULA_OBJ = objeto.DESCRIPCION,
            MOVIM_FECHA = item.MOVIM_FECHA,
            MOVIM_TIPO_MOVIM = item.MOVIM_TIPO_MOVIM,
            MOVIM_TIPO_MOVIM_LISTA = tipoMovimiento.LISTAS_ID_LISTA,
            MOVIM_TIPO_MOVIM_DESCRIPCION = tipoMovimiento.DESCRIPCION,
            MOVIM_PESO = item.MOVIM_PESO,
            MOVIM_PESO_LISTA = peso.LISTAS_ID_LISTA,
            MOVIM_PESO_DESCRIPCION = peso.DESCRIPCION,
            MOVIM_TRANSPORTISTA = item.MOVIM_TRANSPORTISTA,
            MOVIM_TRANSPORTISTA_LISTA = transportista.LISTAS_ID_LISTA,
            MOVIM_TRANSPORTISTA_DESCRIPCION = transportista.DESCRIPCION,
            MOVIM_CLIENTE = item.MOVIM_CLIENTE,
            MOVIM_CLIENTE_LISTA = cliente.LISTAS_ID_LISTA,
            MOVIM_CLIENTE_DESCRIPCION = cliente.DESCRIPCION,
            MOVIM_CHOFER = item.MOVIM_CHOFER,
            MOVIM_CHOFER_LISTA = chofer.LISTAS_ID_LISTA,
            MOVIM_CHOFER_DESCRIPCION = chofer.DESCRIPCION,
            MOVIM_CAMION_ID = item.MOVIM_CAMION_ID,
            MOVIM_REMOLQUE_ID = item.MOVIM_REMOLQUE_ID,
            MOVIM_ALBARAN_ID = item.MOVIM_ALBARAN_ID,
            MOVIM_OBSERVACIONES = item.MOVIM_OBSERVACIONES,
            MOVIM_ENTRADA_SALIDA = item.MOVIM_ENTRADA_SALIDA,
            MOVIM_ENTRADA_SALIDA_LISTA = entradaSalida.LISTAS_ID_LISTA,
            MOVIM_ENTRADA_SALIDA_DESCRIPCION = entradaSalida.DESCRIPCION,
            MOVIM_ALMACEN = item.MOVIM_ALMACEN,
            MOVIM_ALMACEN_LISTA = almacen.LISTAS_ID_LISTA,
            MOVIM_ALMACEN_DESCRIPCION = almacen.DESCRIPCION,
            MOVIM_FECHA_ACTUALIZACION = item.MOVIM_FECHA_ACTUALIZACION,
            MOVIM_ID_DISPOSITIVO = dispositivo.MOVIM_ID_DISPOSITIVO,
            MOVIM_DISPOSITIVO_DESCRIPCION = dispositivo.DESCRIPCION,
            
        };
    }

    private Movim GenerarMovim(MovimDTO movimDTO)
    {
        return new Movim()
        {
            MOVIM_ID_REG = movimDTO.MOVIM_ID_REG,
            MOVIM_ID_ESTADO_REG = "A",
            MOVIM_ID_REG_MOBILE = 0,
            MOVIM_ID_OBJETO = movimDTO.MOVIM_ID_OBJETO, //LstObjetos.Where(x => x.MOVIM_ID_OBJETO == movimDTO.MOVIM_ID_OBJETO).FirstOrDefault().MOVIM_ID_OBJETO,
            MOVIM_FECHA = movimDTO.MOVIM_FECHA,
            MOVIM_TIPO_MOVIM = movimDTO.MOVIM_TIPO_MOVIM,
            MOVIM_TIPO_MOVIM_LISTA = movimDTO.MOVIM_TIPO_MOVIM_LISTA,
            MOVIM_PESO = movimDTO.MOVIM_PESO,
            MOVIM_PESO_LISTA = movimDTO.MOVIM_PESO_LISTA,
            MOVIM_TRANSPORTISTA = movimDTO.MOVIM_TRANSPORTISTA,
            MOVIM_TRANSPORTISTA_LISTA = movimDTO.MOVIM_TRANSPORTISTA_LISTA,
            MOVIM_CLIENTE = movimDTO.MOVIM_CLIENTE,
            MOVIM_CLIENTE_LISTA = movimDTO.MOVIM_CLIENTE_LISTA,
            MOVIM_CHOFER = movimDTO.MOVIM_CHOFER,
            MOVIM_CHOFER_LISTA = movimDTO.MOVIM_CHOFER_LISTA,
            MOVIM_CAMION_ID = movimDTO.MOVIM_CAMION_ID,
            MOVIM_REMOLQUE_ID = movimDTO.MOVIM_REMOLQUE_ID,
            MOVIM_ALBARAN_ID = movimDTO.MOVIM_ALBARAN_ID,
            MOVIM_OBSERVACIONES = movimDTO.MOVIM_OBSERVACIONES,
            MOVIM_ENTRADA_SALIDA = movimDTO.MOVIM_ENTRADA_SALIDA,
            MOVIM_ENTRADA_SALIDA_LISTA = movimDTO.MOVIM_ENTRADA_SALIDA_LISTA,
            MOVIM_ALMACEN = movimDTO.MOVIM_ALMACEN,
            MOVIM_ALMACEN_LISTA = movimDTO.MOVIM_ALMACEN_LISTA,
            MOVIM_FECHA_ACTUALIZACION = movimDTO.MOVIM_FECHA_ACTUALIZACION,
            MOVIM_ID_DISPOSITIVO = movimDTO.MOVIM_ID_DISPOSITIVO,
            MOVIM_PDF = "",
        };
    }
    #endregion

    #region Ordenamiento y filtro
    public ObservableCollection<MovimDTO> ApplyFilter(string? filter, bool verTodos)
    {
        return new ObservableCollection<MovimDTO>(Items.Where(x =>
            (verTodos || x.MOVIM_ID_ESTADO_REG == "A")
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

    public ObservableCollection<MovimDTO> SortData(string sortBy, bool ascending)
    {
        _cachedSortedColumn = sortBy;
        switch (sortBy)
        {
            case "Dispositivo":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_DISPOSITIVO_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_DISPOSITIVO_DESCRIPCION descending
                                                              select item);
                }

            case "Fecha":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_FECHA ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_FECHA descending
                                                              select item);
                }

            case "Container":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_MATRICULA_OBJ ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_MATRICULA_OBJ descending
                                                              select item);
                }

            case "Tipo":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_TIPO_MOVIM_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_TIPO_MOVIM_DESCRIPCION descending
                                                              select item);
                }

            case "Peso":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_PESO_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_PESO_DESCRIPCION descending
                                                              select item);
                }

            case "Transportista":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_TRANSPORTISTA_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_TRANSPORTISTA_DESCRIPCION descending
                                                              select item);
                }

            case "Cliente":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_CLIENTE_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_CLIENTE_DESCRIPCION descending
                                                              select item);
                }

            case "Chofer":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_CHOFER_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_CHOFER_DESCRIPCION descending
                                                              select item);
                }

            case "Camion":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_CAMION_ID ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_CAMION_ID descending
                                                              select item);
                }

            case "Remolque":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_REMOLQUE_ID ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_REMOLQUE_ID descending
                                                              select item);
                }

            case "Albaran":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_ALBARAN_ID ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_ALBARAN_ID descending
                                                              select item);
                }

            case "EntradaSalida":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_ENTRADA_SALIDA_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_ENTRADA_SALIDA_DESCRIPCION descending
                                                              select item);
                }

            case "Almacen":
                if (ascending)
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_ALMACEN_DESCRIPCION ascending
                                                              select item);
                }
                else
                {
                    return new ObservableCollection<MovimDTO>(from item in Items
                                                              orderby item.MOVIM_ALMACEN_DESCRIPCION descending
                                                              select item);
                }
        }

        return Items;
    }
    #endregion
}