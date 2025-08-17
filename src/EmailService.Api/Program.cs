using OpenTelemetry.Instrumentation.Runtime;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using EmailService.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter("global", _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 100,
            Window = TimeSpan.FromSeconds(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        }));
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("EmailService.Api"))
    .WithTracing(t => t
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(m => m
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter());

var app = builder.Build();

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

// stub: apenas aceita e responde 202, integração com fila entra depois
app.MapPost("/v1/emails", (EmailRequest req) =>
{
    return Results.Accepted(value: new { status = "queued", req.Subject });
})
.WithName("QueueEmail")
.Produces(StatusCodes.Status202Accepted);

app.Run();



