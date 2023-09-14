using ContainersDesktop.Logica.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ContainersDesktop.Logica.Workers;
public class ProcesarMensajesWorker : BackgroundService
{
    private readonly ILogger<ProcesarMensajesWorker> _logger;
    private readonly MensajesServicio _mensajeServicio;

    public ProcesarMensajesWorker(ILogger<ProcesarMensajesWorker> logger, MensajesServicio mensajeServicio)
    {
        _logger = logger;
        _mensajeServicio = mensajeServicio;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _mensajeServicio.ProcesarPendientes();
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(60000, stoppingToken);
        }
    }
}
