﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Services;

namespace ContainersDesktop.ViewModels;
public partial class DispositivosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDispositivosServicio _dispositivosServicio;
    private readonly IMovimientosServicio _movimientosServicio;
    private readonly ISincronizacionServicio _sincronizacionServicio;
    private readonly AzureStorageManagement _azureStorageManagement;
    private Dispositivos current;
    public Dispositivos SelectedDispositivo
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }
    public bool HasCurrent => current is not null;

    public ObservableCollection<Dispositivos> Source { get; } = new();
    private string _cachedSortedColumn = string.Empty;

    public DispositivosViewModel(IDispositivosServicio dispositivosServicio, AzureStorageManagement azureStorageManagement, IMovimientosServicio movimientosServicio, ISincronizacionServicio sincronizacionServicio)
    {
        _dispositivosServicio = dispositivosServicio;
        _azureStorageManagement = azureStorageManagement;
        _movimientosServicio = movimientosServicio;
        _sincronizacionServicio = sincronizacionServicio;
    }

    public void OnNavigatedFrom()
    {
        
    }
    public async void OnNavigatedTo(object parameter)
    {
        await CargarSource();
    }

    #region CRUD
    private async Task CargarSource()
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
        
        await _dispositivosServicio.CrearDispositivo(dispositivo);

        await CargarSource();
    }

    public async Task ActualizarDispositivo(Dispositivos dispositivo)
    {
        await _dispositivosServicio.ActualizarDispositivo(dispositivo);
        //await CargarSource();
    }

    public async Task BorrarDispositivo()
    {
        await _dispositivosServicio.BorrarDispositivo(SelectedDispositivo.DISPOSITIVOS_ID_REG);
        await CargarSource();
    }
    #endregion

    #region Sincronizacion
    public async Task<bool> SincronizarInformacion()
    {
        try
        {
            await Sincronizar();

            return true;
        }
        catch (Exception)
        {
            throw;
        }
        
    }

    private async Task Sincronizar()
    {
        var dbDescarga = string.Empty;
        DateTime fechaHoraInicio = DateTime.Now;
        DateTime fechaHoraFin = DateTime.Now;
        int idDispositivo = 0;
        //Subo Base
        try
        {
            var dispositivos = await _dispositivosServicio.ObtenerDispositivos();

            foreach (var item in dispositivos.Where(x => x.DISPOSITIVOS_ID_ESTADO_REG == "A" && !string.IsNullOrEmpty(x.DISPOSITIVOS_CONTAINER)))
            {
                idDispositivo = item.DISPOSITIVOS_ID_REG;
                fechaHoraInicio = DateTime.Now;

                //Subo al contenedor
                await _azureStorageManagement.UploadFile(item.DISPOSITIVOS_CONTAINER);

                //Bajo del container
                dbDescarga = await _azureStorageManagement.DownloadFile(item.DISPOSITIVOS_CONTAINER);

                if (dbDescarga != string.Empty)
                {
                    //TODO - Proceso e incorporo los movimientos
                    await _movimientosServicio.SincronizarMovimientos(dbDescarga, item.DISPOSITIVOS_ID_REG);

                    if (File.Exists(dbDescarga))
                    {
                        File.Delete(dbDescarga);
                    }
                }

                fechaHoraFin = DateTime.Now;

                //Grabo sincronizacion
                var sincronizacion = new Sincronizaciones()
                {
                    SINCRONIZACIONES_FECHA_HORA_INICIO = FormatoFecha.FechaEstandar(fechaHoraInicio),
                    SINCRONIZACIONES_FECHA_HORA_FIN = FormatoFecha.FechaEstandar(fechaHoraFin),
                    SINCRONIZACIONES_DISPOSITIVO_ORIGEN = item.DISPOSITIVOS_ID_REG,
                    SINCRONIZACIONES_RESULTADO = "Ok",
                };
                await _sincronizacionServicio.CrearSincronizacion(sincronizacion);
            }
        }
        catch (Exception ex)
        {
            //Grabo sincronizacion
            var sincronizacion = new Sincronizaciones()
            {
                SINCRONIZACIONES_FECHA_HORA_INICIO = FormatoFecha.FechaEstandar(fechaHoraInicio),
                SINCRONIZACIONES_FECHA_HORA_FIN = FormatoFecha.FechaEstandar(fechaHoraFin),
                SINCRONIZACIONES_DISPOSITIVO_ORIGEN = idDispositivo,
                SINCRONIZACIONES_RESULTADO = "Error " + ex.Message,
            };
            await _sincronizacionServicio.CrearSincronizacion(sincronizacion);

            throw new Exception("Error", ex);
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
