namespace ContainersDesktop.Core.Contracts.Services;
public interface ILocalSettingsServicio
{
    void GuardarElemento(string key, string value);
    void GuardarComposite();
    string LeerElemento(string key);
}
