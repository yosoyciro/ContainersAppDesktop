using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using ContainersDesktop.DTO;
using Microsoft.UI.Xaml;

namespace ContainersDesktop.ViewModels;

public partial class ContainersGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IObjetosServicio _objetosServicio;
    private readonly IListasServicio _listasServicio;

    [ObservableProperty]
    public DateTime fechaInspec;
    //private ObservableCollection<Objetos> _source = new();
    //public ObservableCollection<Objetos> Source
    //{
    //    get => _source;
    //    set
    //    {
    //        SetProperty(ref _source, value, true);
    //        OnPropertyChanged(nameof(Source));
    //    }
    //}
    public ObservableCollection<Objetos> Source
    {
        get;
    } = new();
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

    public ContainersGridViewModel(IObjetosServicio objetosServicio, IListasServicio listasServicio)
    {
        _objetosServicio = objetosServicio;
        _listasServicio = listasServicio;
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
        foreach (var item in lstSiglas)
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
        foreach (var item in lstSiglas)
        {
            LstPmp.Add(new PmpDTO() { OBJ_PMP = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Altura Exterior
        var lstAlturasExterior = LstListas.Where(x => x.LISTAS_ID_LISTA == 1700 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstSiglas)
        {
            LstAlturasExterior.Add(new AlturasExteriorDTO() { OBJ_ALTURA_EXTERIOR = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Cuello Cisne
        var lstCuellosCisne = LstListas.Where(x => x.LISTAS_ID_LISTA == 1800 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstSiglas)
        {
            LstCuellosCisne.Add(new CuellosCisneDTO() { OBJ_CUELLO_CISNE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Barras
        var lstBarras = LstListas.Where(x => x.LISTAS_ID_LISTA == 1900 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstSiglas)
        {
            LstBarras.Add(new BarrasDTO() { OBJ_BARRAS = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Cables
        var lstCables = LstListas.Where(x => x.LISTAS_ID_LISTA == 2000 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstSiglas)
        {
            LstCables.Add(new CablesDTO() { OBJ_CABLES = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP });
        }

        //Lineas vida
        var lstLineasVida = LstListas.Where(x => x.LISTAS_ID_LISTA == 2100 || x.LISTAS_ID_REG == 1).ToList();
        foreach (var item in lstSiglas)
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

    public async void CrearNuevoObjeto()
    {
        var nuevoObjeto = new Objetos()
        {
            OBJ_MATRICULA = "JAJKA",
            OBJ_ID_ESTADO_REG = "A",
            OBJ_SIGLAS_LISTA = 1000,
            OBJ_SIGLAS = 1,
            OBJ_MODELO_LISTA = 1100,
            OBJ_MODELO = 1,
            OBJ_ID_OBJETO = 1198,
            OBJ_VARIANTE_LISTA = 1200,
            OBJ_VARIANTE = 1,
            OBJ_TIPO_LISTA = 1300,
            OBJ_TIPO = 1,
            OBJ_INSPEC_CSC = "",
            OBJ_PROPIETARIO_LISTA = 1400,
            OBJ_PROPIETARIO = 1,
            OBJ_TARA_LISTA = 1500,
            OBJ_TARA = 1,
            OBJ_PMP_LISTA = 1600,
            OBJ_PMP = 1,
            OBJ_CARGA_UTIL = 0,
            OBJ_ALTURA_EXTERIOR_LISTA = 1700,
            OBJ_ALTURA_EXTERIOR = 1,
            OBJ_CUELLO_CISNE_LISTA = 1800,
            OBJ_CUELLO_CISNE = 1,
            OBJ_BARRAS_LISTA = 1900,
            OBJ_BARRAS = 1,
            OBJ_CABLES_LISTA = 2000,
            OBJ_CABLES = 1,
            OBJ_LINEA_VIDA_LISTA = 2100,
            OBJ_LINEA_VIDA = 1,
            OBJ_OBSERVACIONES = ""
        };

        var result = await _objetosServicio.CrearObjeto(nuevoObjeto);
        //if (result)
        //{
        //    Source.Add(nuevoObjeto);
        //};
    }

    public async void ActualizarObjeto(Objetos objeto)
    {        
        await _objetosServicio.ActualizarObjeto(objeto);
    }

    public void OnNavigatedFrom()
    {
    }    
}
