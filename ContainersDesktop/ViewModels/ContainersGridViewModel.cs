using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.DTO;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Contracts.Services;
using ContainersDesktop.Infraestructura.Contracts.Services.Config;
using CoreDesktop.Dominio.Models.Mensajeria;
using CoreDesktop.Infraestructura.Mensajeria.Services;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace ContainersDesktop.ViewModels;

public partial class ContainersGridViewModel : ObservableValidator
{
    private ObjetosViewModel _objetosViewModel = new();
    public ObjetosViewModel ObjetosViewModel => _objetosViewModel;


    private readonly IObjetosServicio _objetosServicio;
    private readonly IListasServicio _listasServicio;
    private readonly IMovimientosServicio _movimientosServicio;
    private readonly IConfigRepository<UI_Config> _configRepository;
    private readonly AzureServiceBus _azureBus;
    private string _cachedSortedColumn = string.Empty;

    //Estilos
    [ObservableProperty]
    public Color gridColor;

    [ObservableProperty]
    public DateTime fechaInspec;

    private ObjetosListaDTO current;
    private Color _comboColor;

    public ObjetosListaDTO SelectedObjeto
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
    public bool EstadoActivo => current?.OBJ_ID_ESTADO_REG == "A" ? true : false;
    public bool EstadoBaja => current?.OBJ_ID_ESTADO_REG == "B" ? true : false;
    public Color ComboColor => _comboColor;

    #region Observable collections
    public ObservableCollection<ObjetosListaDTO> Source
    {
        get;
    } = new();
    public ObservableCollection<Movim> Movims { get; } = new();
    public ObservableCollection<MovimDTO> MovimsDTO { get; } = new();
    public ObservableCollection<Listas> LstListas { get; } = new ObservableCollection<Listas>();
    public ObservableCollection<SiglasDTO> LstSiglas { get; } = new ObservableCollection<SiglasDTO>();
    public ObservableCollection<SiglasDTO> LstSiglasActivos { get; } = new ObservableCollection<SiglasDTO>();
    public ObservableCollection<ModelosDTO> LstModelos { get; } = new ObservableCollection<ModelosDTO>();
    public ObservableCollection<ModelosDTO> LstModelosActivos { get; } = new ObservableCollection<ModelosDTO>();
    public ObservableCollection<VariantesDTO> LstVariantes { get; } = new ObservableCollection<VariantesDTO>();
    public ObservableCollection<VariantesDTO> LstVariantesActivos { get; } = new ObservableCollection<VariantesDTO>();
    public ObservableCollection<TiposDTO> LstTipos { get; } = new ObservableCollection<TiposDTO>();
    public ObservableCollection<TiposDTO> LstTiposActivos { get; } = new ObservableCollection<TiposDTO>();
    public ObservableCollection<PropietariosDTO> LstPropietarios { get; } = new ObservableCollection<PropietariosDTO>();
    public ObservableCollection<PropietariosDTO> LstPropietariosActivos { get; } = new ObservableCollection<PropietariosDTO>();
    public ObservableCollection<TaraDTO> LstTara { get; } = new ObservableCollection<TaraDTO>();
    public ObservableCollection<TaraDTO> LstTaraActivos { get; } = new ObservableCollection<TaraDTO>();
    public ObservableCollection<PmpDTO> LstPmp { get; } = new ObservableCollection<PmpDTO>();
    public ObservableCollection<PmpDTO> LstPmpActivos { get; } = new ObservableCollection<PmpDTO>();
    public ObservableCollection<AlturasExteriorDTO> LstAlturasExterior { get; } = new ObservableCollection<AlturasExteriorDTO>();
    public ObservableCollection<AlturasExteriorDTO> LstAlturasExteriorActivos { get; } = new ObservableCollection<AlturasExteriorDTO>();
    public ObservableCollection<CuellosCisneDTO> LstCuellosCisne { get; } = new ObservableCollection<CuellosCisneDTO>();
    public ObservableCollection<CuellosCisneDTO> LstCuellosCisneActivos { get; } = new ObservableCollection<CuellosCisneDTO>();
    public ObservableCollection<BarrasDTO> LstBarras { get; } = new ObservableCollection<BarrasDTO>();
    public ObservableCollection<BarrasDTO> LstBarrasActivos { get; } = new ObservableCollection<BarrasDTO>();
    public ObservableCollection<CablesDTO> LstCables { get; } = new ObservableCollection<CablesDTO>();
    public ObservableCollection<CablesDTO> LstCablesActivos { get; } = new ObservableCollection<CablesDTO>();
    public ObservableCollection<LineasVidaDTO> LstLineasVida { get; } = new ObservableCollection<LineasVidaDTO>();
    public ObservableCollection<LineasVidaDTO> LstLineasVidaActivos { get; } = new ObservableCollection<LineasVidaDTO>();

    #endregion
    public ContainersGridViewModel(IObjetosServicio objetosServicio, IListasServicio listasServicio, IMovimientosServicio movimientosServicio, IConfigRepository<UI_Config> configRepository, AzureServiceBus azureBus)
    {
        _objetosServicio = objetosServicio;
        _listasServicio = listasServicio;
        _movimientosServicio = movimientosServicio;
        _configRepository = configRepository;

        CargarConfiguracion().Wait();
        _azureBus = azureBus;
    }

    #region Listas y source
    public async Task CargarListasSource()
    {
        try
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
            var lstSiglas = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1000 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstSiglas)
            {
                LstSiglas.Add(new SiglasDTO() { OBJ_SIGLAS = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstSiglasActivos.Add(new SiglasDTO() { OBJ_SIGLAS = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Modelos
            var lstModelos = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1100 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstModelos)
            {
                LstModelos.Add(new ModelosDTO() { OBJ_MODELO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstModelosActivos.Add(new ModelosDTO() { OBJ_MODELO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Variantes
            var lstVariantes = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1200 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstVariantes)
            {
                LstVariantes.Add(new VariantesDTO() { OBJ_VARIANTE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstVariantesActivos.Add(new VariantesDTO() { OBJ_VARIANTE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Tipos
            var lstTipos = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1300 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstTipos)
            {
                LstTipos.Add(new TiposDTO() { OBJ_TIPO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstTiposActivos.Add(new TiposDTO() { OBJ_TIPO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Propietarios
            var lstPropietarios = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1400 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstPropietarios)
            {
                LstPropietarios.Add(new PropietariosDTO() { OBJ_PROPIETARIO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstPropietariosActivos.Add(new PropietariosDTO() { OBJ_PROPIETARIO = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //TARA
            var lstTara = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1500 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstTara)
            {
                LstTara.Add(new TaraDTO() { OBJ_TARA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstTaraActivos.Add(new TaraDTO() { OBJ_TARA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //PMP
            var lstPmp = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1600 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstPmp)
            {
                LstPmp.Add(new PmpDTO() { OBJ_PMP = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")

                    LstPmpActivos.Add(new PmpDTO() { OBJ_PMP = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                {
                }
            }

            //Altura Exterior
            var lstAlturasExterior = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1700 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstAlturasExterior)
            {
                LstAlturasExterior.Add(new AlturasExteriorDTO() { OBJ_ALTURA_EXTERIOR = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstAlturasExteriorActivos.Add(new AlturasExteriorDTO() { OBJ_ALTURA_EXTERIOR = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Cuello Cisne
            var lstCuellosCisne = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1800 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstCuellosCisne)
            {
                LstCuellosCisne.Add(new CuellosCisneDTO() { OBJ_CUELLO_CISNE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstCuellosCisneActivos.Add(new CuellosCisneDTO() { OBJ_CUELLO_CISNE = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }

            }

            //Barras
            var lstBarras = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1900 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstBarras)
            {
                LstBarras.Add(new BarrasDTO() { OBJ_BARRAS = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstBarrasActivos.Add(new BarrasDTO() { OBJ_BARRAS = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Cables
            var lstCables = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 2000 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstCables)
            {
                LstCables.Add(new CablesDTO() { OBJ_CABLES = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstCablesActivos.Add(new CablesDTO() { OBJ_CABLES = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Lineas vida
            var lstLineasVida = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 2100 || x.LISTAS_ID_REG == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstLineasVida)
            {
                LstLineasVida.Add(new LineasVidaDTO() { OBJ_LINEA_VIDA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.LISTAS_ID_ESTADO_REG == "A")
                {
                    LstLineasVidaActivos.Add(new LineasVidaDTO() { OBJ_LINEA_VIDA = item.LISTAS_ID_REG, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Source
            Source.Clear();
            var data = await _objetosServicio.ObtenerObjetos();
            if (data.Any())
            {
                foreach (var item in data)
                {
                    Source.Add(GenerarDTO(item));
                }
            }
        }
        catch (Exception)
        {
            throw;
        }        
    }
    #endregion

    #region CRUD
    public async Task CrearObjeto(ObjetosListaDTO objetoDTO)
    {
        var objeto = GenerarObjeto(objetoDTO);
        objetoDTO.OBJ_ID_REG = await _objetosServicio.CrearObjeto(objeto);
        objetoDTO.OBJ_INSPEC_CSC = FormatoFecha.ConvertirAFechaCorta(objeto.OBJ_INSPEC_CSC);
        objetoDTO.OBJ_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(objetoDTO.OBJ_FECHA_ACTUALIZACION);
        Source.Add(objetoDTO);
    }

    public async Task ActualizarObjeto(ObjetosListaDTO objetoDTO)
    {
        var objeto = GenerarObjeto(objetoDTO);
        await _objetosServicio.ActualizarObjeto(objeto);
        var i = Source.IndexOf(objetoDTO);
        objetoDTO.OBJ_INSPEC_CSC = FormatoFecha.ConvertirAFechaCorta(objetoDTO.OBJ_INSPEC_CSC);
        objetoDTO.OBJ_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(objetoDTO.OBJ_FECHA_ACTUALIZACION);
        Source[i] = objetoDTO;

        var mensaje = new ModifiedContainer(objeto);
        await _azureBus.EnviarMensaje(mensaje);
    }

    public async Task BorrarRecuperarRegistro()
    {
        var accion = EstadoActivo ? "B" : "A";
        SelectedObjeto.OBJ_ID_ESTADO_REG = accion;
        SelectedObjeto.OBJ_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);
        await _objetosServicio.BorrarRecuperarRegistro(GenerarObjeto(SelectedObjeto));

        //Actualizo Source
        var i = Source.IndexOf(SelectedObjeto);        
        Source[i] = SelectedObjeto;
        Source[i].OBJ_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(SelectedObjeto.OBJ_FECHA_ACTUALIZACION);
    }

    #endregion

    #region Ordenamiento y filtro
    public ObservableCollection<ObjetosListaDTO> ApplyFilter(string? filter, bool verTodos)
    {
        return new ObservableCollection<ObjetosListaDTO>(Source.Where(x => 
            (string.IsNullOrEmpty(filter) || x.OBJ_MATRICULA.Contains(filter, StringComparison.OrdinalIgnoreCase)) &&
            (verTodos || x.OBJ_ID_ESTADO_REG == "A")
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

    public ObservableCollection<ObjetosListaDTO> SortData(string sortBy, bool ascending)
    {
        _cachedSortedColumn = sortBy;
        switch (sortBy)
        {
            case "Matricula":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                      orderby item.OBJ_MATRICULA ascending
                                                                      select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                      orderby item.OBJ_MATRICULA descending
                                                                      select item);
                }

            case "Siglas":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                      orderby item.OBJ_SIGLAS_DESCRIPCION ascending
                                                                      select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                      orderby item.OBJ_SIGLAS_DESCRIPCION descending
                                                                      select item);
                }

            case "Modelo":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                      orderby item.OBJ_MODELO_DESCRIPCION ascending
                                                                      select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                      orderby item.OBJ_MODELO_DESCRIPCION descending
                                                                      select item);
                }

            case "Variante":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                      orderby item.OBJ_VARIANTE_DESCRIPCION ascending
                                                                      select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                      orderby item.OBJ_VARIANTE_DESCRIPCION descending
                                                                      select item);
                }

            case "Tipo":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                        orderby item.OBJ_TIPO_DESCRIPCION ascending
                                                                        select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                        orderby item.OBJ_TIPO_DESCRIPCION descending
                                                                        select item);
                }

            case "InspecCSC":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_INSPEC_CSC ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_INSPEC_CSC descending
                                                                     select item);
                }

            case "Propietario":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_PROPIETARIO_DESCRIPCION ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_PROPIETARIO_DESCRIPCION descending
                                                                     select item);
                }

            case "Tara":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_TARA_DESCRIPCION ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_TARA_DESCRIPCION descending
                                                                     select item);
                }

            case "Pmp":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_PMP_DESCRIPCION ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_PMP_DESCRIPCION descending
                                                                     select item);
                }

            case "CargaUtil":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_CARGA_UTIL ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_CARGA_UTIL descending
                                                                     select item);
                }

            case "AlturaExterior":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_ALTURA_EXTERIOR_DESCRIPCION ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_ALTURA_EXTERIOR_DESCRIPCION descending
                                                                     select item);
                }

            case "CuelloCisne":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_CUELLO_CISNE_DESCRIPCION ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_CUELLO_CISNE_DESCRIPCION descending
                                                                     select item);
                }

            case "Barras":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_BARRAS_DESCRIPCION ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_BARRAS_DESCRIPCION descending
                                                                     select item);
                }

            case "Cables":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_CABLES_DESCRIPCION ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_CABLES_DESCRIPCION descending
                                                                     select item);
                }

            case "LineasVida":
                if (ascending)
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_LINEA_VIDA_DESCRIPCION ascending
                                                                     select item);
                }
                else
                {
                    return new ObservableCollection<ObjetosListaDTO>(from item in Source
                                                                     orderby item.OBJ_LINEA_VIDA_DESCRIPCION descending
                                                                     select item);
                }
        }

        return Source;
    }
    #endregion

    #region Mapeos
    private ObjetosListaDTO GenerarDTO(Objetos objeto)
    {
        return new ObjetosListaDTO()
        {
            OBJ_ID_REG = objeto.OBJ_ID_REG,
            OBJ_ID_ESTADO_REG = objeto.OBJ_ID_ESTADO_REG,
            OBJ_MATRICULA = objeto.OBJ_MATRICULA,
            OBJ_ID_OBJETO = objeto.OBJ_ID_OBJETO,
            OBJ_SIGLAS = objeto.OBJ_SIGLAS,
            OBJ_SIGLAS_DESCRIPCION = LstSiglas.FirstOrDefault(x => x.OBJ_SIGLAS == objeto.OBJ_SIGLAS).DESCRIPCION,
            OBJ_MODELO = objeto.OBJ_MODELO,
            OBJ_MODELO_DESCRIPCION = LstModelos.FirstOrDefault(x => x.OBJ_MODELO == objeto.OBJ_MODELO).DESCRIPCION,
            OBJ_VARIANTE = objeto.OBJ_VARIANTE,
            OBJ_VARIANTE_DESCRIPCION = LstVariantes.FirstOrDefault(x => x.OBJ_VARIANTE == objeto.OBJ_VARIANTE).DESCRIPCION,
            OBJ_TIPO = objeto.OBJ_TIPO,
            OBJ_TIPO_DESCRIPCION = LstTipos.FirstOrDefault(x => x.OBJ_TIPO == objeto.OBJ_TIPO).DESCRIPCION,
            OBJ_INSPEC_CSC = objeto.OBJ_INSPEC_CSC,
            OBJ_PROPIETARIO = objeto.OBJ_PROPIETARIO,
            OBJ_PROPIETARIO_DESCRIPCION = LstPropietarios.FirstOrDefault(x => x.OBJ_PROPIETARIO == objeto.OBJ_PROPIETARIO).DESCRIPCION,
            OBJ_TARA = objeto.OBJ_TARA,
            OBJ_TARA_DESCRIPCION = LstTara.FirstOrDefault(x => x.OBJ_TARA == objeto.OBJ_TARA).DESCRIPCION,
            OBJ_PMP = objeto.OBJ_PMP,
            OBJ_PMP_DESCRIPCION = LstPmp.FirstOrDefault(x => x.OBJ_PMP == objeto.OBJ_PMP).DESCRIPCION,
            OBJ_CARGA_UTIL = objeto.OBJ_CARGA_UTIL,
            OBJ_ALTURA_EXTERIOR = objeto.OBJ_ALTURA_EXTERIOR,
            OBJ_ALTURA_EXTERIOR_DESCRIPCION = LstAlturasExterior.FirstOrDefault(x => x.OBJ_ALTURA_EXTERIOR == objeto.OBJ_ALTURA_EXTERIOR).DESCRIPCION,
            OBJ_CUELLO_CISNE = objeto.OBJ_CUELLO_CISNE,
            OBJ_CUELLO_CISNE_DESCRIPCION = LstCuellosCisne.FirstOrDefault(x => x.OBJ_CUELLO_CISNE == objeto.OBJ_CUELLO_CISNE).DESCRIPCION,
            OBJ_BARRAS = objeto.OBJ_BARRAS,
            OBJ_BARRAS_DESCRIPCION = LstBarras.FirstOrDefault(x => x.OBJ_BARRAS == objeto.OBJ_BARRAS).DESCRIPCION,
            OBJ_CABLES = objeto.OBJ_CABLES,
            OBJ_CABLES_DESCRIPCION = LstCables.FirstOrDefault(x => x.OBJ_CABLES == objeto.OBJ_CABLES).DESCRIPCION,
            OBJ_LINEA_VIDA = objeto.OBJ_LINEA_VIDA,
            OBJ_LINEA_VIDA_DESCRIPCION = LstLineasVida.FirstOrDefault(x => x.OBJ_LINEA_VIDA == objeto.OBJ_LINEA_VIDA).DESCRIPCION,
            OBJ_OBSERVACIONES = objeto.OBJ_OBSERVACIONES,
            OBJ_FECHA_ACTUALIZACION = objeto.OBJ_FECHA_ACTUALIZACION,
            OBJ_COLOR = objeto.OBJ_COLOR,
        };
    }

    private Objetos GenerarObjeto(ObjetosListaDTO objetoDTO)
    {
        return new Objetos()
        {
            OBJ_ID_REG = objetoDTO.OBJ_ID_REG,
            OBJ_ID_ESTADO_REG = objetoDTO.OBJ_ID_ESTADO_REG,
            OBJ_MATRICULA = objetoDTO.OBJ_MATRICULA,
            OBJ_ID_OBJETO = 0,
            OBJ_SIGLAS = objetoDTO.OBJ_SIGLAS,
            OBJ_SIGLAS_LISTA = LstSiglas.FirstOrDefault(x => x.OBJ_SIGLAS == objetoDTO.OBJ_SIGLAS).LISTAS_ID_LISTA,
            OBJ_MODELO = objetoDTO.OBJ_MODELO,
            OBJ_MODELO_LISTA = LstModelos.FirstOrDefault(x => x.OBJ_MODELO == objetoDTO.OBJ_MODELO).LISTAS_ID_LISTA,
            OBJ_VARIANTE = objetoDTO.OBJ_VARIANTE,
            OBJ_VARIANTE_LISTA = LstVariantes.FirstOrDefault(x => x.OBJ_VARIANTE == objetoDTO.OBJ_VARIANTE).LISTAS_ID_LISTA,
            OBJ_TIPO = objetoDTO.OBJ_TIPO,
            OBJ_TIPO_LISTA = LstTipos.FirstOrDefault(x => x.OBJ_TIPO == objetoDTO.OBJ_TIPO).LISTAS_ID_LISTA,
            OBJ_INSPEC_CSC = objetoDTO.OBJ_INSPEC_CSC,
            OBJ_PROPIETARIO = objetoDTO.OBJ_PROPIETARIO,
            OBJ_PROPIETARIO_LISTA = LstPropietarios.FirstOrDefault(x => x.OBJ_PROPIETARIO == objetoDTO.OBJ_PROPIETARIO).LISTAS_ID_LISTA,
            OBJ_TARA = objetoDTO.OBJ_TARA,
            OBJ_TARA_LISTA = LstTara.FirstOrDefault(x => x.OBJ_TARA == objetoDTO.OBJ_TARA).LISTAS_ID_LISTA,
            OBJ_PMP = objetoDTO.OBJ_PMP,
            OBJ_PMP_LISTA = LstPmp.FirstOrDefault(x => x.OBJ_PMP == objetoDTO.OBJ_PMP).LISTAS_ID_LISTA,
            OBJ_CARGA_UTIL = objetoDTO.OBJ_CARGA_UTIL,
            OBJ_ALTURA_EXTERIOR = objetoDTO.OBJ_ALTURA_EXTERIOR,
            OBJ_ALTURA_EXTERIOR_LISTA = LstAlturasExterior.FirstOrDefault(x => x.OBJ_ALTURA_EXTERIOR == objetoDTO.OBJ_ALTURA_EXTERIOR).LISTAS_ID_LISTA,
            OBJ_CUELLO_CISNE = objetoDTO.OBJ_CUELLO_CISNE,
            OBJ_CUELLO_CISNE_LISTA = LstCuellosCisne.FirstOrDefault(x => x.OBJ_CUELLO_CISNE == objetoDTO.OBJ_CUELLO_CISNE).LISTAS_ID_LISTA,
            OBJ_BARRAS = objetoDTO.OBJ_BARRAS,
            OBJ_BARRAS_LISTA = LstBarras.FirstOrDefault(x => x.OBJ_BARRAS == objetoDTO.OBJ_BARRAS).LISTAS_ID_LISTA,
            OBJ_CABLES = objetoDTO.OBJ_CABLES,
            OBJ_CABLES_LISTA = LstCables.FirstOrDefault(x => x.OBJ_CABLES == objetoDTO.OBJ_CABLES).LISTAS_ID_LISTA,
            OBJ_LINEA_VIDA = objetoDTO.OBJ_LINEA_VIDA,
            OBJ_LINEA_VIDA_LISTA = LstLineasVida.FirstOrDefault(x => x.OBJ_LINEA_VIDA == objetoDTO.OBJ_LINEA_VIDA).LISTAS_ID_LISTA,
            OBJ_OBSERVACIONES = objetoDTO.OBJ_OBSERVACIONES,
            OBJ_FECHA_ACTUALIZACION = objetoDTO.OBJ_FECHA_ACTUALIZACION,
            OBJ_COLOR = objetoDTO.OBJ_COLOR,
        };
    }
    #endregion

    #region Configs

    private async Task CargarConfiguracion()
    {
        var gridColor = await _configRepository.Leer("GridColor");
        GridColor = Colores.HexToColor(gridColor.Valor);

        var comboColor = await _configRepository.Leer("ComboColor");
        _comboColor = Colores.HexToColor(comboColor.Valor);
    }
    
    #endregion
}
