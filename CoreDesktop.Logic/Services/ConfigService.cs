using System.Runtime.CompilerServices;
using ContainersDesktop.Dominio.Models.UI_ConfigModels;
using ContainersDesktop.Infraestructura.Contracts.Services.Config;

namespace CoreDesktop.Logic.Services;
public class ConfigService
{
    private readonly IConfigRepository<UI_Config> _configRepository;
    public ConfigService(IConfigRepository<UI_Config> configRepository)
    {
        _configRepository = configRepository;
    }

    public async Task<UI_Config> GetConfig(string clave)
    {
        return await _configRepository.Leer(clave);
    }
}
