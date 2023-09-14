using System.Collections.ObjectModel;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.DTO;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Mensajeria.Messages;
using ContainersDesktop.Logica.Mensajeria.Services;
using Windows.UI;

namespace ContainersDesktop.ViewModels;

public partial class ContainersGridViewModel : ObservableValidator
{
    private ObjetosViewModel _objetosViewModel = new();
    public ObjetosViewModel ObjetosViewModel => _objetosViewModel;


    private readonly IAsyncRepository<Objeto> _objetosServicio;
    private readonly IAsyncRepository<Lista> _listasServicio;
    private readonly IAsyncRepository<Movim> _movimientosServicio;
    private readonly IConfigRepository<UI_Config> _configRepository;
    private readonly AzureServiceBus _azureBus;
    private readonly IMapper _mapper;

    private string _cachedSortedColumn = string.Empty;

    //Estilos
    private Color _gridColor;
    private Color _comboColor;

    [ObservableProperty]
    public DateTime fechaInspec;

    private ObjetosListaDTO current;
    

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
    public Color GridColor => _gridColor;
    public Color ComboColor => _comboColor;

    #region Observable collections
    public ObservableCollection<ObjetosListaDTO> Source
    {
        get;
    } = new();
    public ObservableCollection<Movim> Movims { get; } = new();
    public ObservableCollection<MovimDTO> MovimsDTO { get; } = new();
    public ObservableCollection<Lista> LstListas { get; } = new ObservableCollection<Lista>();
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
    public ContainersGridViewModel(
        IConfigRepository<UI_Config> configRepository,
        AzureServiceBus azureBus,
        IAsyncRepository<Objeto> objetosServicio,
        IAsyncRepository<Lista> listasServicio,
        IAsyncRepository<Movim> movimientosServicio,
        IMapper mapper)
    {
        _configRepository = configRepository;
        _azureBus = azureBus;
        _objetosServicio = objetosServicio;
        _listasServicio = listasServicio;
        _movimientosServicio = movimientosServicio;

        CargarConfiguracion().Wait();
        _mapper = mapper;
    }

    #region Listas y source
    public async Task CargarListasSource()
    {
        try
        {
            LstListas.Clear();

            //Cargo Listas
            var listas = await _listasServicio.GetAsync();
            if (listas.Any())
            {
                foreach (var item in listas)
                {
                    LstListas.Add(item);
                }
            }

            //Siglas
            var lstSiglas = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1000 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstSiglas)
            {
                LstSiglas.Add(new SiglasDTO() { OBJ_SIGLAS = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstSiglasActivos.Add(new SiglasDTO() { OBJ_SIGLAS = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Modelos
            var lstModelos = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1100 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstModelos)
            {
                LstModelos.Add(new ModelosDTO() { OBJ_MODELO = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstModelosActivos.Add(new ModelosDTO() { OBJ_MODELO = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Variantes
            var lstVariantes = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1200 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstVariantes)
            {
                LstVariantes.Add(new VariantesDTO() { OBJ_VARIANTE = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstVariantesActivos.Add(new VariantesDTO() { OBJ_VARIANTE = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Tipos
            var lstTipos = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1300 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstTipos)
            {
                LstTipos.Add(new TiposDTO() { OBJ_TIPO = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstTiposActivos.Add(new TiposDTO() { OBJ_TIPO = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Propietarios
            var lstPropietarios = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1400 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstPropietarios)
            {
                LstPropietarios.Add(new PropietariosDTO() { OBJ_PROPIETARIO = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstPropietariosActivos.Add(new PropietariosDTO() { OBJ_PROPIETARIO = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //TARA
            var lstTara = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1500 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstTara)
            {
                LstTara.Add(new TaraDTO() { OBJ_TARA = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstTaraActivos.Add(new TaraDTO() { OBJ_TARA = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //PMP
            var lstPmp = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1600 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstPmp)
            {
                LstPmp.Add(new PmpDTO() { OBJ_PMP = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")

                    LstPmpActivos.Add(new PmpDTO() { OBJ_PMP = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                {
                }
            }

            //Altura Exterior
            var lstAlturasExterior = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1700 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstAlturasExterior)
            {
                LstAlturasExterior.Add(new AlturasExteriorDTO() { OBJ_ALTURA_EXTERIOR = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstAlturasExteriorActivos.Add(new AlturasExteriorDTO() { OBJ_ALTURA_EXTERIOR = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Cuello Cisne
            var lstCuellosCisne = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1800 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstCuellosCisne)
            {
                LstCuellosCisne.Add(new CuellosCisneDTO() { OBJ_CUELLO_CISNE = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstCuellosCisneActivos.Add(new CuellosCisneDTO() { OBJ_CUELLO_CISNE = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }

            }

            //Barras
            var lstBarras = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 1900 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstBarras)
            {
                LstBarras.Add(new BarrasDTO() { OBJ_BARRAS = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstBarrasActivos.Add(new BarrasDTO() { OBJ_BARRAS = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Cables
            var lstCables = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 2000 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstCables)
            {
                LstCables.Add(new CablesDTO() { OBJ_CABLES = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstCablesActivos.Add(new CablesDTO() { OBJ_CABLES = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Lineas vida
            var lstLineasVida = LstListas
                .Where(x => (x.LISTAS_ID_LISTA == 2100 || x.ID == 1))
                .OrderBy(x => x.LISTAS_ID_LISTA_ORDEN)
                .ToList();
            foreach (var item in lstLineasVida)
            {
                LstLineasVida.Add(new LineasVidaDTO() { OBJ_LINEA_VIDA = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                if (item.Estado == "A")
                {
                    LstLineasVidaActivos.Add(new LineasVidaDTO() { OBJ_LINEA_VIDA = item.ID, DESCRIPCION = item.LISTAS_ID_LISTA_DESCRIP, LISTAS_ID_LISTA = item.LISTAS_ID_LISTA });
                }
            }

            //Source
            Source.Clear();
            var data = await _objetosServicio.GetAsync();
            if (data.Any())
            {
                foreach (var item in data)
                {
                    var objetoDTO = _mapper.Map<ObjetosListaDTO>(item);
                    CargarDescripciones(objetoDTO);
                    Source.Add(objetoDTO);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }        
    }

    private void CargarDescripciones(ObjetosListaDTO objeto)
    {
        objeto.OBJ_SIGLAS_DESCRIPCION = LstSiglas.FirstOrDefault(x => x.OBJ_SIGLAS == objeto.OBJ_SIGLAS).DESCRIPCION;
        objeto.OBJ_MODELO_DESCRIPCION = LstModelos.FirstOrDefault(x => x.OBJ_MODELO == objeto.OBJ_MODELO).DESCRIPCION;
        objeto.OBJ_VARIANTE_DESCRIPCION = LstVariantes.FirstOrDefault(x => x.OBJ_VARIANTE == objeto.OBJ_VARIANTE).DESCRIPCION;
        objeto.OBJ_TIPO_DESCRIPCION = LstTipos.FirstOrDefault(x => x.OBJ_TIPO == objeto.OBJ_TIPO).DESCRIPCION;
        objeto.OBJ_INSPEC_CSC = FormatoFecha.ConvertirAFechaCorta(objeto.OBJ_INSPEC_CSC);
        objeto.OBJ_PROPIETARIO_DESCRIPCION = LstPropietarios.FirstOrDefault(x => x.OBJ_PROPIETARIO == objeto.OBJ_PROPIETARIO).DESCRIPCION;
        objeto.OBJ_TARA_DESCRIPCION = LstTara.FirstOrDefault(x => x.OBJ_TARA == objeto.OBJ_TARA).DESCRIPCION;
        objeto.OBJ_PMP_DESCRIPCION = LstPmp.FirstOrDefault(x => x.OBJ_PMP == objeto.OBJ_PMP).DESCRIPCION;
        objeto.OBJ_ALTURA_EXTERIOR_DESCRIPCION = LstAlturasExterior.FirstOrDefault(x => x.OBJ_ALTURA_EXTERIOR == objeto.OBJ_ALTURA_EXTERIOR).DESCRIPCION;
        objeto.OBJ_CUELLO_CISNE_DESCRIPCION = LstCuellosCisne.FirstOrDefault(x => x.OBJ_CUELLO_CISNE == objeto.OBJ_CUELLO_CISNE).DESCRIPCION;
        objeto.OBJ_BARRAS_DESCRIPCION = LstBarras.FirstOrDefault(x => x.OBJ_BARRAS == objeto.OBJ_BARRAS).DESCRIPCION;
        objeto.OBJ_CABLES_DESCRIPCION = LstCables.FirstOrDefault(x => x.OBJ_CABLES == objeto.OBJ_CABLES).DESCRIPCION;
        objeto.OBJ_LINEA_VIDA_DESCRIPCION = LstLineasVida.FirstOrDefault(x => x.OBJ_LINEA_VIDA == objeto.OBJ_LINEA_VIDA).DESCRIPCION;
        objeto.OBJ_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(objeto.OBJ_FECHA_ACTUALIZACION);
        objeto.OBJ_COLOR = objeto.OBJ_COLOR;
    }
    #endregion

    #region CRUD
    public async Task CrearObjeto(ObjetosListaDTO objetoDTO)
    {
        try
        {
            var objeto = _mapper.Map<Objeto>(objetoDTO);
            objetoDTO.OBJ_ID_REG = await _objetosServicio.AddAsync(objeto);

            objetoDTO.OBJ_INSPEC_CSC = FormatoFecha.ConvertirAFechaCorta(objeto.OBJ_INSPEC_CSC);
            objetoDTO.OBJ_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(objetoDTO.OBJ_FECHA_ACTUALIZACION);
            Source.Add(objetoDTO);

            var mensaje = _mapper.Map<ContainerCreado>(objetoDTO);
            await _azureBus.EnviarMensaje(mensaje);
        }
        catch (Exception)
        {
            throw;
        }        
    }

    public async Task ActualizarObjeto(ObjetosListaDTO objetoDTO)
    {
        try
        {
            var objeto = (Objeto)_mapper.Map(objetoDTO, typeof(ObjetosListaDTO), typeof(Objeto));
            await _objetosServicio.UpdateAsync(objeto);

            var i = Source.IndexOf(objetoDTO);
            objetoDTO.OBJ_INSPEC_CSC = FormatoFecha.ConvertirAFechaCorta(objetoDTO.OBJ_INSPEC_CSC);
            objetoDTO.OBJ_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(objetoDTO.OBJ_FECHA_ACTUALIZACION);
            Source[i] = objetoDTO;

            var mensaje = _mapper.Map<ContainerModificado>(objetoDTO);
            await _azureBus.EnviarMensaje(mensaje);
        }
        catch (Exception)
        {
            throw;
        }        
    }

    public async Task BorrarRecuperarRegistro()
    {
        try
        {
            var accion = EstadoActivo ? "B" : "A";
            SelectedObjeto.OBJ_ID_ESTADO_REG = accion;
            SelectedObjeto.OBJ_FECHA_ACTUALIZACION = FormatoFecha.FechaEstandar(DateTime.Now);

            var objeto = (Objeto)_mapper.Map(SelectedObjeto, typeof(ObjetosListaDTO), typeof(Objeto));
            await _objetosServicio.UpdateAsync(objeto);            

            //Actualizo Source
            var i = Source.IndexOf(SelectedObjeto);
            Source[i] = SelectedObjeto;
            Source[i].OBJ_FECHA_ACTUALIZACION = FormatoFecha.ConvertirAFechaHora(SelectedObjeto.OBJ_FECHA_ACTUALIZACION);

            var mensaje = _mapper.Map<ContainerModificado>(SelectedObjeto);
            await _azureBus.EnviarMensaje(mensaje);
        }
        catch (Exception)
        {

            throw;
        }
        
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

    #region Configs

    private async Task CargarConfiguracion()
    {
        var gridColor = await _configRepository.Leer("GridColor");
        _gridColor = Colores.HexToColor(gridColor.Valor!);

        var comboColor = await _configRepository.Leer("ComboColor");
        _comboColor = Colores.HexToColor(comboColor.Valor!);
    }
    
    #endregion
}
