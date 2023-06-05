using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;
using ContainersDesktop.Core.Services;

namespace ContainersDesktop.ViewModels;

public partial class TiposListasGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IClaListServicio _claListServicio;

    public ObservableCollection<ClaList> Source { get; set; } = new ();

    public TiposListasGridViewModel(IClaListServicio claListServicio)
    {
        _claListServicio = claListServicio;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _claListServicio.ObtenerClaListas();

        foreach (var item in data.OrderBy(x => x.CLALIST_DESCRIP))
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public async void GuardarClaList(ClaList claList)
    {
        //try
        //{
            if (claList != null && claList.CLALIST_ID_REG == 0)
            //var result = await _objetosServicio.ObtenerObjetoPorId(objeto.OBJ_ID_REG);
            {
                await _claListServicio.CrearClaLista(claList);
            }
            else
            {
                await _claListServicio.ActualizarClaLista(claList);
            }
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }

    public async void BorrarObjeto(ClaList claList)
    {
        await _claListServicio.BorrarClaLista(claList.CLALIST_ID_REG);
        Source.Remove(claList);
    }
}
