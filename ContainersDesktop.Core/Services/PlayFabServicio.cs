using ContainersDesktop.Core.Contracts.Services;
using PlayFab.ClientModels;
using PlayFab;

namespace ContainersDesktop.Core.Services;
public class PlayFabServicio : IPlayFabServicio
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
