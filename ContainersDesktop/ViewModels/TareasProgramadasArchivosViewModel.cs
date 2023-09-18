using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Comunes.Helpers;
using ContainersDesktop.Dominio.Models;
using ContainersDesktop.Infraestructura.Persistencia.Contracts;
using ContainersDesktop.Logica.Contracts;
using ContainersDesktop.Logica.Specification.Implementaciones;
using Microsoft.UI.Xaml.Controls;

namespace ContainersDesktop.ViewModels;
public class TareasProgramadasArchivosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IAsyncRepository<TareaProgramadaArchivo> _tareasProgramadasArchivosRepo;

    private ObservableCollection<TareaProgramadaArchivo>? _source = new();
    private int TAREAS_PROGRAMADAS_ID_REG;
    public ObservableCollection<TareaProgramadaArchivo>? Source => _source;
    private TareaProgramadaArchivo current;
    public TareaProgramadaArchivo Current
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }
    public bool HasCurrent => current is not null;

    public TareasProgramadasArchivosViewModel(IAsyncRepository<TareaProgramadaArchivo> tareasProgramadasArchivosRepo)
    {
        _tareasProgramadasArchivosRepo = tareasProgramadasArchivosRepo;
    }

    public async Task CargarSource()
    {
        //Source.Clear();

        //var data = await _tareasProgramadasArchivosRepo.GetAsync();

        //foreach (var item in data)
        //{
        //    item.FechaActualizacion = FormatoFecha.ConvertirAFechaHora(item.FechaActualizacion!);
        //    Source.Add(item);
        //}
        var spec = new TareasProgramadasArchivosSpec(TAREAS_PROGRAMADAS_ID_REG);
        _source = new ObservableCollection<TareaProgramadaArchivo>(await _tareasProgramadasArchivosRepo.GetAllWithSpecsAsync(spec));
    }

    public ObservableCollection<TareaProgramadaArchivo> AplicarFiltro()
    {
        return new ObservableCollection<TareaProgramadaArchivo>(Source);
    }

    public void OnNavigatedTo(object parameter)
    {
        Console.WriteLine("parameter", parameter);
        TAREAS_PROGRAMADAS_ID_REG = (int)parameter;
    }
    public void OnNavigatedFrom() => throw new NotImplementedException();
}
