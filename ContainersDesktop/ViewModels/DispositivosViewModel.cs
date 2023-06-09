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

    public async void GuardarDispositivo(Dispositivos dispositivo)
    {
        if (dispositivo != null && dispositivo.DISPOSITIVOS_ID_REG == 0)
        {
            await _dispositivosServicio.CrearDispositivo(dispositivo);
        }
        else
        {
            await _dispositivosServicio.ActualizarDispositivo(dispositivo);
        }
    }

    public async void BorrarDispositivo(Dispositivos dispositivo)
    {
        await _dispositivosServicio.BorrarDispositivo(dispositivo.DISPOSITIVOS_ID_REG);
        Source.Remove(dispositivo);
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
                    await _movimientosServicio.SincronizarMovimientos(dbDescarga);

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
