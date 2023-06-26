using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Services;

namespace ContainersDesktop.ViewModels;
public partial class DispositivosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDispositivosServicio _dispositivosServicio;
    private readonly IMovimientosServicio _movimientosServicio;
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

    public DispositivosViewModel(IDispositivosServicio dispositivosServicio, AzureStorageManagement azureStorageManagement, IMovimientosServicio movimientosServicio)
    {
        _dispositivosServicio = dispositivosServicio;
        _azureStorageManagement = azureStorageManagement;
        _movimientosServicio = movimientosServicio;
    }

    public void OnNavigatedFrom()
    {
        
    }
    public async void OnNavigatedTo(object parameter)
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
        Source.Add(dispositivo);       
    }

    public async Task ActualizarDispositivo(Dispositivos dispositivo)
    {
        await _dispositivosServicio.ActualizarDispositivo(dispositivo);
        //var i = Source.IndexOf(dispositivo);
        //Source[i] = dispositivo;
    }

    public async Task BorrarDispositivo()
    {
        await _dispositivosServicio.BorrarDispositivo(SelectedDispositivo.DISPOSITIVOS_ID_REG);
        Source.Remove(SelectedDispositivo);
    }

    public async Task<bool> SincronizarInformacion()
    {
        try
        {
            await SubirDatos();

            await BajarDatos();

            return true;
        }
        catch (Exception)
        {

            throw;
        }
        
    }

    private async Task<bool> SubirDatos()
    {
        //Subo Base
        try
        {
            var dispositivos = await _dispositivosServicio.ObtenerDispositivos();

            foreach (var item in dispositivos)
            {
                await _azureStorageManagement.UploadFile(item.DISPOSITIVOS_CONTAINER);
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
    }

    private async Task<bool> BajarDatos()
    {
        var dbDescarga = string.Empty;
        //Subo Base
        try
        {
            var dispositivos = await _dispositivosServicio.ObtenerDispositivos();

            foreach (var item in dispositivos)
            {
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
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error", ex);
        }
        finally
        {
            if (File.Exists(dbDescarga))
            {
                File.Delete(dbDescarga);
            }
        }
    }
}
