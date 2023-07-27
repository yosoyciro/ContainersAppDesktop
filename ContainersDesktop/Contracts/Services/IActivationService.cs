using System.Threading.Tasks;

namespace ContainersDesktop.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
