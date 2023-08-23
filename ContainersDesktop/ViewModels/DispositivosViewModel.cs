using System.Collections.ObjectModel;
using Azure;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Contracts.Services;
using ContainersDesktop.Logica.Services;

namespace ContainersDesktop.ViewModels;
public partial class DispositivosViewModel : ObservableRecipient
{    
    private readonly IDispositivosServicio _dispositivosServicio;
    private readonly AzureStorageManagement _azureStorageManagement;
    private readonly SincronizarServicio _sincronizarServicio;
    private readonly DispositivosFormViewModel _formViewModel = new();
    public DispositivosFormViewModel FormViewModel => _formViewModel;

    private Dispositivos current;
    public Dispositivos SelectedDispositivo
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
    public bool EstadoActivo => current?.DISPOSITIVOS_ID_ESTADO_REG == "A" ? true : false;
    public bool EstadoBaja => current?.DISPOSITIVOS_ID_ESTADO_REG == "B" ? true : false;

    public ObservableCollection<Dispositivos> Source { get; } = new();
    [ObservableProperty]
    public bool isBusy = false;
    
    private string _cachedSortedColumn = string.Empty;

    public DispositivosViewModel(IDispositivosServicio dispositivosServicio, AzureStorageManagement azureStorageManagement, SincronizarServicio sincronizarServicio)
    {
        _dispositivosServicio = dispositivosServicio;
        _azureStorageManagement = azureStorageManagement;
        _sincronizarServicio = sincronizarServicio;
    }

    #region CRUD
    public async Task CargarSource()
    {
        Source.Clear();
        var data = await _dispositivosServicio.ObtenerDispositivos();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public async Task CrearDispositivo(Dispositivos dispositivo)
    {
        
        dispositivo.DISPOSITIVOS_ID_REG = await _dispositivosServicio.CrearDispositivo(dispositivo);
        if (dispositivo.DISPOSITIVOS_ID_REG > 0)
        {
            Source.Add(dispositivo);
        }        
    }

    public async Task ActualizarDispositivo(Dispositivos dispositivo)
    {
        await _dispositivosServicio.ActualizarDispositivo(dispositivo);

        //Actualizo Source
        var i = Source.IndexOf(dispositivo);        
        Source[i] = dispositivo;
    }

    public async Task BorrarRecuperarDispositivo()
    {
        var accion = EstadoActivo ? "B" : "A";
        await _dispositivosServicio.BorrarRecuperarDispositivo(SelectedDispositivo.DISPOSITIVOS_ID_REG, accion);

        //Actualizo Source
        var i = Source.IndexOf(SelectedDispositivo);
        SelectedDispositivo.DISPOSITIVOS_ID_ESTADO_REG = accion;
        Source[i] = SelectedDispositivo;
    }

    public async Task<bool> ExisteContainer(string container, string plataforma)
    {
        if (plataforma == "local")
        {
            return await _dispositivosServicio.ExisteContainer(container);
        }
        else
        {
            return _azureStorageManagement.ExisteContainer(container);
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
    public ObservableCollection<Dispositivos> ApplyFilter(string? filter, bool verTodos)
    {
        return new ObservableCollection<Dispositivos>(Source.Where(x =>
            (string.IsNullOrEmpty(filter) || x.DISPOSITIVOS_DESCRIP.Contains(filter, StringComparison.OrdinalIgnoreCase)) &&
            (verTodos || x.DISPOSITIVOS_ID_ESTADO_REG == "A")
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

    public ObservableCollection<Dispositivos> SortData(string sortBy, bool ascending)
    {
        _cachedSortedColumn = sortBy;
        switch (sortBy)
        {
            case "Id":
                if (ascending)
                {
                    return new ObservableCollection<Dispositivos>(from item in Source
                                                             orderby item.DISPOSITIVOS_ID_REG ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Dispositivos>(from item in Source
                                                             orderby item.DISPOSITIVOS_ID_REG descending
                                                             select item);
                }

            case "Descripcion":
                if (ascending)
                {
                    return new ObservableCollection<Dispositivos>(from item in Source
                                                             orderby item.DISPOSITIVOS_DESCRIP ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Dispositivos>(from item in Source
                                                             orderby item.DISPOSITIVOS_DESCRIP descending
                                                             select item);
                }

            case "Container":
                if (ascending)
                {
                    return new ObservableCollection<Dispositivos>(from item in Source
                                                             orderby item.DISPOSITIVOS_CONTAINER ascending
                                                             select item);
                }
                else
                {
                    return new ObservableCollection<Dispositivos>(from item in Source
                                                             orderby item.DISPOSITIVOS_CONTAINER descending
                                                             select item);
                }            
        }

        return Source;
    }
    #endregion
}
