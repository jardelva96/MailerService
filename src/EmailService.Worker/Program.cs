using Microsoft.Extensions.Hosting;
using EmailService.WorkerApp;

var builder = Host.CreateApplicationBuilder(args);

// registra o hosted service explicitamente (resolver CA1812)
builder.Services.AddHostedService<EmailWorker>();

var host = builder.Build();
await host.RunAsync().ConfigureAwait(false);
