using PlayFab.ClientModels;
using PlayFab;
using System.Threading.Tasks;

namespace ContainersDesktop.Logica.Services;
public class PlayFabServicio
{
    public async Task<bool> Login(string username, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = username,
            Password = password,
            TitleId = "EF0DA"
        };

        PlayFabSettings.staticSettings.TitleId = "EF0DA";
        var result = await PlayFabClientAPI.LoginWithEmailAddressAsync(request);

        return result.Error != null ? false : true;
    }
}
