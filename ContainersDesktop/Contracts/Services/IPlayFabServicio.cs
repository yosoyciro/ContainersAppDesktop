namespace ContainersDesktop.Core.Contracts.Services;
public interface IPlayFabServicio
{
    Task<bool> Login(string username, string password);
}
