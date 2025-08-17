using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailService.WorkerApp;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Instantiated by Generic Host via DI (AddHostedService).")]
internal sealed class EmailWorker : BackgroundService
{
    private readonly ILogger<EmailWorker> _logger;
    public EmailWorker(ILogger<EmailWorker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        WorkerLogs.WorkerStarted(_logger);

        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO: consumir fila e enviar emails
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken).ConfigureAwait(false);
        }
    }
}

internal static partial class WorkerLogs
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Email worker started")]
    internal static partial void WorkerStarted(ILogger logger);
}
