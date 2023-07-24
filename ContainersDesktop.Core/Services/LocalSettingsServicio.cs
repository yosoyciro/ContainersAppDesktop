using ContainersDesktop.Core.Contracts.Services;
using Windows.Storage;

namespace ContainersDesktop.Core.Services;
public class LocalSettingsServicio : ILocalSettingsServicio
{
    private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

    public void GuardarComposite() => throw new NotImplementedException();
    public void GuardarElemento(string key, string value)
    {
        localSettings.Values[key] = value;
    }
    public string LeerElemento(string key)
    {
        return localSettings.Values[key] as string;
    }
}
