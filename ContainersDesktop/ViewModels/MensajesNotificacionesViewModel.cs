using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.ViewModels;
public partial class MensajesNotificacionesViewModel : ObservableRecipient
{
    [ObservableProperty]
    private int mensajesNoProcesados;

    public void SetMensajesNoLeidos(int mensajesNoProcesados)
    {
        try
        {
            this.MensajesNoProcesados = mensajesNoProcesados;
        }
        catch (Exception ex)
        {

            throw;
        }
        
    }
}
