### MailerService

A production-ready .NET 8 email microservice composed of an API and a background Worker, packaged with Docker Compose and instrumented end-to-end with OpenTelemetry (OTLP → Jaeger) and structured logging to Seq. It uses RabbitMQ for messaging and PostgreSQL for persistence, with unit and integration tests.

# Status: ✅ API and Worker build & run (Docker or dotnet), tests are passing.

### ✨ Features
Clean solution split: API (HTTP) + Worker (background processing) + layered Application/Domain/Infrastructure.
Contracts-first DTOs (EmailRequest, AttachmentDto, etc.) designed for analyzer friendliness (immutable records, IReadOnlyList<>, ReadOnlyMemory<byte>).
Observability by default
Traces & metrics via OpenTelemetry → OTLP(4317) → Jaeger UI.
Structured logs to Seq.
Health checks endpoints.
Messaging: RabbitMQ ready (MassTransit packages referenced in the Worker).
Database: PostgreSQL (Infrastructure project + DbContext).
Dockerized: one command boots the whole stack.
Tests: xUnit unit and integration test projects.

### CI: GitHub Actions workflow to build & test on push/PR.
```bash
🗂️ Solution Layout
MailerService/
├─ docker-compose.yml                 # Full stack: API, Worker, Jaeger, Seq, RabbitMQ, Postgres
├─ Directory.Build.props              # Common .NET settings/analyzers
├─ .editorconfig                      # Consistent code style
├─ .github/workflows/ci.yml           # CI: build + test
├─ adr/
│  └─ 0001-record-architecture-decisions.md
├─ src/
│  ├─ EmailService.Api/               # ASP.NET Core API (HTTP endpoints)
│  │  ├─ Program.cs
│  │  ├─ appsettings.json
│  │  └─ Dockerfile
│  ├─ EmailService.Worker/            # Background service (queue→send pipeline)
│  │  ├─ Program.cs
│  │  ├─ EmailWorker.cs               # LoggerMessage-based logging, cancellation-friendly loop
│  │  ├─ appsettings.json
│  │  └─ Dockerfile
│  ├─ EmailService.Application/       # Use cases, commands/handlers, orchestration
│  ├─ EmailService.Domain/            # Entities, value objects, domain logic
│  ├─ EmailService.Infrastructure/    # EF Core DbContext, repositories, external adapters
│  │  └─ Persistence/EmailDbContext.cs
│  ├─ EmailService.Integrations/      # SMTP/3rd-party providers (extensible)
│  ├─ EmailService.Contracts/         # Public DTOs (request/response, records)
│  │  └─ EmailRequest.cs
│  └─ EmailService.Shared/            # Cross-cutting helpers (namespace: EmailService.Common)
├─ tests/
│  ├─ EmailService.UnitTests/         # Fast tests for Application/Domain
│  └─ EmailService.IntegrationTests/  # API/Infra integration tests
├─ LICENSE
├─ CODE_OF_CONDUCT.md
├─ SECURITY.md
└─ README.md
```
### 🚀 Quick Start (Docker)

Prerequisites: Docker 

### from repo root
```bash
docker compose up -d
docker compose logs -f api worker
```
Useful URLs (defaults from compose)
```bash
Jaeger UI (traces): http://localhost:16686

OTLP gRPC (ingest): localhost:4317

Seq (logs): http://localhost:5341

RabbitMQ (broker): amqp://localhost:5672 — Management UI http://localhost:15672 (default user/pass if configured: guest / guest)

PostgreSQL: localhost:5432
```
API port: If the API’s host port is not fixed (or you changed it), query it:
```bash
docker compose port api 8080
```

Then hit http://localhost:<mapped-port>/health or open Swagger (if enabled): http://localhost:<mapped-port>/swagger.

### 🧪 Run & Test without Docker

Prerequisites: .NET 8 SDK and local dependencies (or adjust connection strings).

### restore & build everything
```bash
dotnet restore
dotnet build
dotnet test
```

### Run individually:
```bash
# API
dotnet run --project src/EmailService.Api

# Worker
dotnet run --project src/EmailService.Worker
```
### 🔧 Configuration

Configuration uses standard ASP.NET Core providers:
```bash
appsettings.json / appsettings.{Environment}.json

Environment variables (__ denotes nested sections)
```
### Common keys:
```bash
ConnectionStrings__Postgres=Host=...;Port=5432;Database=...;Username=...;Password=...

RabbitMQ__Host=localhost

RabbitMQ__VirtualHost=/

RabbitMQ__User=guest

RabbitMQ__Password=guest

OTEL_EXPORTER_OTLP_ENDPOINT=http://jaeger:4317 (inside Docker network) or http://localhost:4317 locally

Seq__Url=http://seq:80 (inside Docker) or http://localhost:5341 locally
```
See src/EmailService.Api/appsettings.json and src/EmailService.Worker/appsettings.json for exact keys included in the project.

### 📦 Contracts (DTOs)
```bash
EmailRequest — analyzer-friendly records:

public sealed record EmailAddress(string? Name, string Address);

public sealed record AttachmentDto(
    string FileName,
    string ContentType,
    ReadOnlyMemory<byte> Content // serialized as base64 in JSON
);

public sealed record EmailRequest(
    EmailAddress From,
    IReadOnlyList<EmailAddress> To,
    string Subject,
    string HtmlBody,
    string? TextBody = null,
    IReadOnlyList<EmailAddress>? Cc = null,
    IReadOnlyList<EmailAddress>? Bcc = null,
    IReadOnlyList<AttachmentDto>? Attachments = null,
    string? TenantId = null,
    string? MessageIdempotencyKey = null
);
```

### Sample JSON (attachments’ content is base64):
```bash
{
  "from": { "name": "System", "address": "no-reply@example.com" },
  "to": [{ "name": "Jane", "address": "jane@example.com" }],
  "subject": "Welcome!",
  "htmlBody": "<h1>Hello</h1><p>Glad you're here.</p>",
  "textBody": "Hello - Glad you're here.",
  "attachments": [
    {
      "fileName": "hello.txt",
      "contentType": "text/plain",
      "content": "SGVsbG8sIE1haWxlclNlcnZpY2Uh"
    }
  ],
  "tenantId": "acme",
  "messageIdempotencyKey": "a2b0c4d8"
}
```

The public HTTP endpoint for sending emails is exposed by the API (see Swagger).
The Worker is prepared to consume messages from RabbitMQ and execute the send pipeline.

### 🔭 Observability

Tracing & Metrics: OpenTelemetry.* packages enabled for ASP.NET Core + HTTP + runtime metrics; exporter is OTLP → Jaeger.

Logging: Structured logs via Serilog to Seq; correlation flows across API and Worker.

Health checks: GET /health (and optionally /healthz) for liveness/readiness.

### 🧰 Tech Stack

Runtime: .NET 8 (LTS)

API: ASP.NET Core

Worker: BackgroundService (MassTransit packages for RabbitMQ)

Messaging: RabbitMQ

Storage: PostgreSQL (EF Core)

Observability: OpenTelemetry → Jaeger, Serilog → Seq

Testing: xUnit (unit + integration)

CI: GitHub Actions

### 🗺️ Roadmap (suggested next steps)

Implement the RabbitMQ consumer + handler in the Worker using MassTransit.

Add SMTP/SendGrid integration(s) behind an interface in Integrations.

Add retries/circuit-breakers (Polly) around external providers.

Add OpenAPI/Swagger configuration (if disabled) and example requests.

Add database migrations & automated apply on startup (if needed).
