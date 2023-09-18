using System.Collections.ObjectModel;
using AutoMapper;
using Azure;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Mensajeria.Messages;
using ContainersDesktop.Logica.Services;
using ContainersDesktop.Logica.Mensajeria.Services;
using Windows.UI;
using CommunityToolkit.Mvvm.Input;

namespace ContainersDesktop.ViewModels;
public partial class DispositivosViewModel : ObservableRecipient
{    
    private readonly IAsyncRepository<Dispositivo> _dispositivosRepo;
    private readonly AzureStorageManagement _azureStorageManagement;
    private readonly SincronizarServicio _sincronizarServicio;
    private readonly DispositivosFormViewModel _formViewModel = new();
    private readonly IConfigRepository<UI_Config> _configRepository;
    private readonly IMapper _mapper;
    private readonly AzureServiceBus _azureServiceBus;

    //Estilos
    private Color _gridColor;
    private Color _comboColor;

    public DispositivosFormViewModel FormViewModel => _formViewModel;

    private Dispositivo current;
    public Dispositivo SelectedDispositivo
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
    public bool EstadoActivo => current?.Estado == "A" ? true : false;
    public bool EstadoBaja => current?.Estado == "B" ? true : false;
    public Color GridColor => _gridColor;
    public Color ComboColor => _comboColor;

    public ObservableCollection<Dispositivo> Source { get; } = new();
    [ObservableProperty]
    public bool isBusy = false;
    
    private string _cachedSortedColumn = string.Empty;

    public DispositivosViewModel(IAsyncRepository<Dispositivo> dispositivosServicio, AzureStorageManagement azureStorageManagement, SincronizarServicio sincronizarServicio, IConfigRepository<UI_Config> configRepository, IMapper mapper, AzureServiceBus serviceBus)
    {
        _dispositivosRepo = dispositivosServicio;
        _azureStorageManagement = azureStorageManagement;
        _sincronizarServicio = sincronizarServicio;
        _configRepository = configRepository;

        CargarConfiguracion().Wait();
        _mapper = mapper;
        _azureServiceBus = serviceBus;
    }

    #region CRUD
    public async Task CargarSource()
    {
        Source.Clear();
        var data = await _dispositivosRepo.GetAsync();

        foreach (var item in data)
        {
            item.FechaActualizacion = FormatoFecha.ConvertirAFechaHora(item.FechaActualizacion);
            Source.Add(item);
        }
    }

    public async Task CrearDispositivo(Dispositivo dispositivo)
    {
        try
        {            
            dispositivo.ID = await _dispositivosRepo.AddAsync(dispositivo);
            if (dispositivo.ID > 0)
            {
                dispositivo.FechaActualizacion = FormatoFecha.ConvertirAFechaHora(dispositivo.FechaActualizacion);
                Source.Add(dispositivo);
            }

            //Mensaje a Azure Service Bus
            var mensaje = _mapper.Map<Dispositivo, DispositivoCreado>(dispositivo);
            await _azureServiceBus.EnviarMensaje(mensaje);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task ActualizarDispositivo(Dispositivo dispositivo)
    {
        try
        {
            await _dispositivosRepo.UpdateAsync(dispositivo);

            //Actualizo Source
            var i = Source.IndexOf(dispositivo);
            Source[i] = dispositivo;

            //Mensaje a Azure Service Bus
            var mensaje = _mapper.Map<Dispositivo, DispositivoModificado>(dispositivo);
            await _azureServiceBus.EnviarMensaje(mensaje);
        }
        catch (Exception)
        {

            throw;
        }        
    }

    public async Task BorrarRecuperarDispositivo()
    {
        try
        {
            var accion = EstadoActivo ? "B" : "A";
            await _dispositivosRepo.UpdateAsync(SelectedDispositivo);

            //Actualizo Source
            var i = Source.IndexOf(SelectedDispositivo);
            SelectedDispositivo.Estado = accion;
            Source[i] = SelectedDispositivo;

            //Mensaje a Azure Service Bus
            var mensaje = _mapper.Map<Dispositivo, DispositivoModificado>(SelectedDispositivo);
            await _azureServiceBus.EnviarMensaje(mensaje);
        }
        catch (Exception)
        {

            throw;
        }
        
    }
    
    #endregion

    #region Sincronizacion
    public async Task<bool> SincronizarInformacion()
    {
        try
        {
            IsBusy = true;
            await _sincronizarServicio.Sincronizar();

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

    #region Ordenamiento y filtro
    public ObservableCollection<Dispositivo> ApplyFilter(string? filter, bool verTodos)
    {
        return new ObservableCollection<Dispositivo>(Source.Where(x =>
            (string.IsNullOrEmpty(filter) || x.DISPOSITIVOS_DESCRIP.Contains(filter, StringComparison.OrdinalIgnoreCase)) &&
            (verTodos || x.Estado == "A")
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

    public ObservableCollection<Dispositivo> SortData(string sortBy, bool ascending)
    {
        _cachedSortedColumn = sortBy;
        switch (sortBy)
        {
            case "Id":
                if (ascending)
                {
                    return new ObservableCollection<Dispositivo>(from item in Source
                                                             orderby item.ID ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Dispositivo>(from item in Source
                                                             orderby item.ID descending
                                                             select item);
                }

            case "Descripcion":
                if (ascending)
                {
                    return new ObservableCollection<Dispositivo>(from item in Source
                                                             orderby item.DISPOSITIVOS_DESCRIP ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Dispositivo>(from item in Source
                                                             orderby item.DISPOSITIVOS_DESCRIP descending
                                                             select item);
                }

            case "Container":
                if (ascending)
                {
                    return new ObservableCollection<Dispositivo>(from item in Source
                                                             orderby item.DISPOSITIVOS_CONTAINER ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Dispositivo>(from item in Source
                                                             orderby item.DISPOSITIVOS_CONTAINER descending
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
