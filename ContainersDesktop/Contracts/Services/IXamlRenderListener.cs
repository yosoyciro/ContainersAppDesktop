using Microsoft.UI.Xaml;

namespace ContainersDesktop.Contracts.Services;
public interface IXamlRenderListener
{
    void OnXamlRendered(FrameworkElement control);
}
