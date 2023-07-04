using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Services;
using ContainersDesktop.DTO;
using Windows.UI.WebUI;

namespace ContainersDesktop.ViewModels;
public partial class MovimientosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IMovimientosServicio _movimientosServicio;
    private readonly IListasServicio _listasServicio;
    private readonly IDispositivosServicio _dispositivosServicio;
    private readonly IObjetosServicio _objetosServicio;

    private Movim current;
    public Movim Current
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }
    public bool HasCurrent => current is not null;

    public ObservableCollection<Movim> Items
    {
        get;
    } = new();
    public ObservableCollection<Listas> LstListas { get; } = new();
    public ObservableCollection<DispositivosDTO> LstDispositivos { get; } = new();
    public ObservableCollection<ObjetosDTO> LstObjetos { get; } = new();
    public ObservableCollection<TiposMovimientoDTO> LstTiposMovimiento
    {
    get; } = new();
    public ObservableCollection<PesosDTO> LstPesos
    {
        get;
    } = new();
    public ObservableCollection<TransportistasDTO> LstTransportistas
    {
        get;
    } = new();
    public ObservableCollection<ClientesDTO> LstClientes
    {
        get;
    } = new();
    public ObservableCollection<ChoferesDTO> LstChoferes
    {
        get;
    } = new();
    public ObservableCollection<EntradaSalidaDTO> LstEntradaSalida
    {
        get;
    } = new();
    public ObservableCollection<AlmacenesDTO> LstAlmacenes
    {
        get;
    } = new();

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
        await CargarSource(parameter as Objetos);
        await CargarListas();
    }

    private async Task CargarSource(Objetos objeto)
    {
        //Movimientos
        Items.Clear();
        var data = objeto != null ? await _movimientosServicio.ObtenerMovimientosObjeto(objeto.OBJ_ID_REG) : await _movimientosServicio.ObtenerMovimientosTodos();
        if (data.Any())
        {
            foreach (var item in data)
            {
                Items.Add(item);
            }
        }
    }

    public async Task BorrarMovimiento()
    {
        await _movimientosServicio.BorrarMovimiento(Current.MOVIM_ID_REG);
        var item = Items.FirstOrDefault(x => x.MOVIM_ID_REG == Current.MOVIM_ID_REG);
        Items.Remove(item);
    }

    public async Task AgregarMovimiento(Movim movim)
    {
        await _movimientosServicio.CrearMovimiento(movim);
        Items.Add(movim);
    }

    public async Task ActualizarMovimiento(Movim movim)
    {
        await _movimientosServicio.ActualizarMovimiento(movim);        
    }

    //private MovimDTO CrearDTO(Movim item)
    //{    
    //    return new MovimDTO()
    //    {
    //        MOVIM_ID_REG = item.MOVIM_ID_REG,
    //        MOVIM_MATRICULA_OBJ = LstObjetos.Where(x => x.OBJ_ID_REG == item.MOVIM_ID_OBJETO).FirstOrDefault().OBJ_MATRICULA,
    //        MOVIM_FECHA = item.MOVIM_FECHA,
    //        MOVIM_TIPO_MOVIM = item.MOVIM_TIPO_MOVIM,
    //        MOVIM_TIPO_MOVIM_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_TIPO_MOVIM).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
    //        MOVIM_PESO = item.MOVIM_PESO,
    //        MOVIM_PESO_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_PESO).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
    //        MOVIM_TRANSPORTISTA = item.MOVIM_TRANSPORTISTA,
    //        MOVIM_TRANSPORTISTA_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_TRANSPORTISTA).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
    //        MOVIM_CLIENTE = item.MOVIM_CLIENTE,
    //        MOVIM_CLIENTE_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_CLIENTE).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
    //        MOVIM_CHOFER = item.MOVIM_CHOFER,
    //        MOVIM_CHOFER_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_CHOFER).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
    //        MOVIM_CAMION_ID = item.MOVIM_CAMION_ID,
    //        MOVIM_REMOLQUE_ID = item.MOVIM_REMOLQUE_ID,
    //        MOVIM_ALBARAN_ID = item.MOVIM_ALBARAN_ID,
    //        MOVIM_OBSERVACIONES = item.MOVIM_OBSERVACIONES,
    //        MOVIM_ENTRADA_SALIDA = item.MOVIM_ENTRADA_SALIDA,
    //        MOVIM_ENTRADA_SALIDA_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_ENTRADA_SALIDA).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
    //        MOVIM_ALMACEN = item.MOVIM_ALMACEN,
    //        MOVIM_ALMACEN_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_ALMACEN).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
    //        MOVIM_FECHA_ACTUALIZACION = item.MOVIM_FECHA_ACTUALIZACION,
    //        MOVIM_ID_DISPOSITIVO = LstDispositivos.Where(x => x.DISPOSITIVOS_ID_REG == item.MOVIM_ID_DISPOSITIVO).FirstOrDefault().DISPOSITIVOS_DESCRIP,
    //    };  
    //}

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
            }
        }        

        //Listas relacionadas
        var lstTipoMovimiento = LstListas.Where(x => x.LISTAS_ID_LISTA == 3000 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstTipoMovimiento)
        {
            LstTiposMovimiento.Add(new TiposMovimientoDTO() { MOVIM_TIPO_MOVIM = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        var lstPesos = LstListas.Where(x => x.LISTAS_ID_LISTA == 3100 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstPesos)
        {
            LstPesos.Add(new PesosDTO() { MOVIM_PESO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        var lstTransportistas = LstListas.Where(x => x.LISTAS_ID_LISTA == 3200 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstTransportistas)
        {
            LstTransportistas.Add(new TransportistasDTO() { MOVIM_TRANSPORTISTA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        var lstClientes = LstListas.Where(x => x.LISTAS_ID_LISTA == 3300 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstClientes)
        {
            LstClientes.Add(new ClientesDTO() { MOVIM_CLIENTE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        var lstChoferes = LstListas.Where(x => x.LISTAS_ID_LISTA == 3400 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstChoferes)
        {
            LstChoferes.Add(new ChoferesDTO() { MOVIM_CHOFER = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        var lstEntradaSalida = LstListas.Where(x => x.LISTAS_ID_LISTA == 3500 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstEntradaSalida)
        {
            LstEntradaSalida.Add(new EntradaSalidaDTO() { MOVIM_ENTRADA_SALIDA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        var lstAlmacenes = LstListas.Where(x => x.LISTAS_ID_LISTA == 3600 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstAlmacenes)
        {
            LstAlmacenes.Add(new AlmacenesDTO() { MOVIM_ALMACEN = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }
    }
}
