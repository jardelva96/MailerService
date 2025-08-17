FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/EmailService.Api/EmailService.Api.csproj", "src/EmailService.Api/"]
COPY ["src/EmailService.Application/EmailService.Application.csproj", "src/EmailService.Application/"]
COPY ["src/EmailService.Domain/EmailService.Domain.csproj", "src/EmailService.Domain/"]
COPY ["src/EmailService.Infrastructure/EmailService.Infrastructure.csproj", "src/EmailService.Infrastructure/"]
COPY ["src/EmailService.Integrations/EmailService.Integrations.csproj", "src/EmailService.Integrations/"]
COPY ["src/EmailService.Contracts/EmailService.Contracts.csproj", "src/EmailService.Contracts/"]
COPY ["src/EmailService.Shared/EmailService.Shared.csproj", "src/EmailService.Shared/"]
RUN dotnet restore "src/EmailService.Api/EmailService.Api.csproj"

COPY . .
WORKDIR "/src/src/EmailService.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmailService.Api.dll"]
