using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<EmailWorker>();

var host = builder.Build();
await host.RunAsync();

internal sealed class EmailWorker : BackgroundService
{
    private readonly ILogger<EmailWorker> _logger;
    public EmailWorker(ILogger<EmailWorker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email worker started");
        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO: consumir fila e enviar emails
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
