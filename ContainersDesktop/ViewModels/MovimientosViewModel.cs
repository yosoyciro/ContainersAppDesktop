using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using ContainersDesktop.DTO;

namespace ContainersDesktop.ViewModels;
public partial class MovimientosViewModel : ObservableRecipient, INavigationAware
{
    private readonly IMovimientosServicio _movimientosServicio;
    private readonly IListasServicio _listasServicio;
    private readonly IDispositivosServicio _dispositivosServicio;

    private Movim current;
    public Movim Current
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }
    public bool HasCurrent => current is not null;

    public ObservableCollection<MovimDTO> Items
    {
        get;
    } = new();
    public ObservableCollection<Listas> LstListas { get; } = new ObservableCollection<Listas>();
    public ObservableCollection<Dispositivos> LstDispositivos { get; } = new ObservableCollection<Dispositivos>();

    public MovimientosViewModel(IMovimientosServicio movimientosServicio, IListasServicio listasServicio, IDispositivosServicio dispositivosServicio)
    {
        _movimientosServicio = movimientosServicio;
        _listasServicio = listasServicio;
        _dispositivosServicio = dispositivosServicio;
    }
    public void OnNavigatedFrom()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        //Cargo Listas
        LstListas.Clear();        
        var listas = await _listasServicio.ObtenerListas();
        if (listas.Any())
        {
            foreach (var item in listas)
            {
                LstListas.Add(item);
            }
        }

        //Dispositivos
        LstDispositivos.Clear();
        var dispositivos = await _dispositivosServicio.ObtenerDispositivos();
        if (dispositivos.Any())
        {
            foreach (var item in dispositivos)
            {
                LstDispositivos.Add(item);
            }
        }


        //Movimientos
        Items.Clear();
        var data = await _movimientosServicio.ObtenerMovimientosTodos();
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
                    MOVIM_ID_DISPOSITIVO = LstDispositivos.Where(x => x.DISPOSITIVOS_ID_REG == item.MOVIM_ID_DISPOSITIVO).FirstOrDefault().DISPOSITIVOS_DESCRIP,
                };

                Items.Add(movimDTO);
            }
        }
    }
}
