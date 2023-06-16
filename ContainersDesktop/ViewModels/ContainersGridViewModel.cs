using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using ContainersDesktop.DTO;

namespace ContainersDesktop.ViewModels;

public partial class ContainersGridViewModel : ObservableValidator, INavigationAware
{
    private ObjetosViewModel _objetosViewModel = new();
    public ObjetosViewModel ObjetosViewModel => _objetosViewModel;


    private readonly IObjetosServicio _objetosServicio;
    private readonly IListasServicio _listasServicio;
    private readonly IMovimientosServicio _movimientosServicio;

    [ObservableProperty]
    public DateTime fechaInspec;

    private Objetos current;
    public Objetos SelectedObjeto
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }
    public bool HasCurrent => current is not null;

    public ObservableCollection<Objetos> Source
    {
        get;
    } = new();
    public ObservableCollection<Movim> Movims { get; } = new();
    public ObservableCollection<MovimDTO> MovimsDTO { get; } = new();
    public ObservableCollection<Listas> LstListas { get; } = new ObservableCollection<Listas>();
    public ObservableCollection<SiglasDTO> LstSiglas { get; } = new ObservableCollection<SiglasDTO>();
    public ObservableCollection<ModelosDTO> LstModelos { get; } = new ObservableCollection<ModelosDTO>();
    public ObservableCollection<VariantesDTO> LstVariantes { get; } = new ObservableCollection<VariantesDTO>();
    public ObservableCollection<TiposDTO> LstTipos { get; } = new ObservableCollection<TiposDTO>();
    public ObservableCollection<PropietariosDTO> LstPropietarios { get; } = new ObservableCollection<PropietariosDTO>();
    public ObservableCollection<TaraDTO> LstTara { get; } = new ObservableCollection<TaraDTO>();
    public ObservableCollection<PmpDTO> LstPmp { get; } = new ObservableCollection<PmpDTO>();
    public ObservableCollection<AlturasExteriorDTO> LstAlturasExterior { get; } = new ObservableCollection<AlturasExteriorDTO>();
    public ObservableCollection<CuellosCisneDTO> LstCuellosCisne { get; } = new ObservableCollection<CuellosCisneDTO>();
    public ObservableCollection<BarrasDTO> LstBarras { get; } = new ObservableCollection<BarrasDTO>();
    public ObservableCollection<CablesDTO> LstCables { get; } = new ObservableCollection<CablesDTO>();
    public ObservableCollection<LineasVidaDTO> LstLineasVida { get; } = new ObservableCollection<LineasVidaDTO>();

    public ObservableCollection<TiposMovimientoDTO> LstTiposMovimiento { get; } = new ObservableCollection<TiposMovimientoDTO>();
    public ObservableCollection<TransportistasDTO> LstTransportistas { get; } = new ObservableCollection<TransportistasDTO>();

    public ContainersGridViewModel(IObjetosServicio objetosServicio, IListasServicio listasServicio, IMovimientosServicio movimientosServicio)
    {
        _objetosServicio = objetosServicio;
        _listasServicio = listasServicio;
        _movimientosServicio = movimientosServicio;
    }

    public async void OnNavigatedTo(object parameter)
    {
        LstListas.Clear();

        //Cargo Listas
        var listas = await _listasServicio.ObtenerListas();
        if (listas.Any())
        {
            foreach (var item in listas)
            {
                LstListas.Add(item);
            }
        }

        //Siglas
        var lstSiglas = LstListas.Where(x => x.LISTAS_ID_LISTA == 1000 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstSiglas)
        {
            LstSiglas.Add(new SiglasDTO() { OBJ_SIGLAS = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Modelos
        var lstModelos = LstListas.Where(x => x.LISTAS_ID_LISTA == 1100 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstModelos)
        {
            LstModelos.Add(new ModelosDTO() { OBJ_MODELO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Variantes
        var lstVariantes = LstListas.Where(x => x.LISTAS_ID_LISTA == 1200 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstVariantes)
        {
            LstVariantes.Add(new VariantesDTO() { OBJ_VARIANTE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Tipos
        var lstTipos = LstListas.Where(x => x.LISTAS_ID_LISTA == 1300 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstTipos)
        {
            LstTipos.Add(new TiposDTO() { OBJ_TIPO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Propietarios
        var lstPropietarios = LstListas.Where(x => x.LISTAS_ID_LISTA == 1400 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstPropietarios)
        {
            LstPropietarios.Add(new PropietariosDTO() { OBJ_PROPIETARIO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //TARA
        var lstTara = LstListas.Where(x => x.LISTAS_ID_LISTA == 1500 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstTara)
        {
            LstTara.Add(new TaraDTO() { OBJ_TARA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //PMP
        var lstPmp = LstListas.Where(x => x.LISTAS_ID_LISTA == 1600 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstPmp)
        {
            LstPmp.Add(new PmpDTO() { OBJ_PMP = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Altura Exterior
        var lstAlturasExterior = LstListas.Where(x => x.LISTAS_ID_LISTA == 1700 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstAlturasExterior)
        {
            LstAlturasExterior.Add(new AlturasExteriorDTO() { OBJ_ALTURA_EXTERIOR = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Cuello Cisne
        var lstCuellosCisne = LstListas.Where(x => x.LISTAS_ID_LISTA == 1800 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstCuellosCisne)
        {
            LstCuellosCisne.Add(new CuellosCisneDTO() { OBJ_CUELLO_CISNE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Barras
        var lstBarras = LstListas.Where(x => x.LISTAS_ID_LISTA == 1900 || x.LISTAS_ID_REG == 1).OrderBy(x => x.LISTAS_ID_LISTA_ORDEN).ToList();
        foreach (var item in lstBarras)
        {
            LstBarras.Add(new BarrasDTO() { OBJ_BARRAS = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Cables
        var lstCables = LstListas.Where(x => x.LISTAS_ID_LISTA == 2000 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstCables)
        {
            LstCables.Add(new CablesDTO() { OBJ_CABLES = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Lineas vida
        var lstLineasVida = LstListas.Where(x => x.LISTAS_ID_LISTA == 2100 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstLineasVida)
        {
            LstLineasVida.Add(new LineasVidaDTO() { OBJ_LINEA_VIDA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }        

        var data = await _objetosServicio.ObtenerObjetos();
        if (data.Any())
        {
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }        
    }        

    public async Task CrearObjeto(Objetos objeto)
    {
        await _objetosServicio.CrearObjeto(objeto);
    }

    public async Task ActualizarObjeto(Objetos objeto)
    {
        await _objetosServicio.ActualizarObjeto(objeto);
        var i = Source.IndexOf(objeto);
        Source[i] = objeto;
    }

    public async Task BorrarObjeto()
    {
        await _objetosServicio.BorrarObjeto(SelectedObjeto.OBJ_ID_REG);
        Source.Remove(SelectedObjeto);
    }

    public async void CargarMovimientos(Objetos objeto)
    {
        MovimsDTO.Clear();

        var data = await _movimientosServicio.ObtenerMovimientos(objeto.OBJ_ID_REG);
        if (data.Any())
        {
            foreach (var item in data)
            {
                var movimDTO = new MovimDTO()
                {
                    MOVIM_ID_REG = item.MOVIM_ID_REG,
                    MOVIM_FECHA = item.MOVIM_FECHA,
                    MOVIM_TIPO_MOVIM = item.MOVIM_TIPO_MOVIM,
                    MOVIM_TIPO_MOVIM_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_TIPO_MOVIM).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,                    
                    MOVIM_PESO = item.MOVIM_PESO,
                    MOVIM_PESO_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_PESO).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
                    MOVIM_TRANSPORTISTA = item.MOVIM_TRANSPORTISTA,
                    MOVIM_TRANSPORTISTA_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_TRANSPORTISTA).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
                    MOVIM_CLIENTE = item.MOVIM_CLIENTE,
                    MOVIM_CLIENTE_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_CLIENTE).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
                    MOVIM_CHOFER = item.MOVIM_CHOFER,
                    MOVIM_CHOFER_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_CHOFER).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
                    MOVIM_CAMION_ID = item.MOVIM_CAMION_ID,
                    MOVIM_REMOLQUE_ID = item.MOVIM_REMOLQUE_ID,
                    MOVIM_ALBARAN_ID = item.MOVIM_ALBARAN_ID,
                    MOVIM_OBSERVACIONES = item.MOVIM_OBSERVACIONES,
                    MOVIM_ENTRADA_SALIDA = item.MOVIM_ENTRADA_SALIDA,
                    MOVIM_ENTRADA_SALIDA_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_ENTRADA_SALIDA).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
                    MOVIM_ALMACEN = item.MOVIM_ALMACEN,
                    MOVIM_ALMACEN_DESCRIPCION = LstListas.Where(x => x.LISTAS_ID_REG == item.MOVIM_ALMACEN).FirstOrDefault().LISTAS_ID_LISTA_DESCRIP,
                    MOVIM_FECHA_ACTUALIZACION = item.MOVIM_FECHA_ACTUALIZACION,
                };

                MovimsDTO.Add(movimDTO);
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }    
}
