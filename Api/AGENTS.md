# Api — ASP.NET Core 8 Web API

## Project structure
- **Solution**: `Api.sln` (root) — projects `Api\Api.csproj` and `Api\Api.Tests\Api.Tests.csproj`
- **Entrypoint**: `Api/Program.cs` using top-level statements, ASP.NET Core 8 setup
- **Framework**: .NET 8, nullable enabled, implicit usings enabled
- **EF Core 8 + SQL Server** (via docker-compose), Swagger 6.4.0

## Duplicated directory quirk
There are two nested project dirs: `Api/` and `Api/Api/`. Both `Api/Api.csproj` and `Api/Api/Api.csproj` exist. **Work from `Api/` (the first-level project), NOT `Api/Api/`.** The nested `Api/Api/Api/` is excluded from compilation via `Compile Remove="Api\**\*.cs"` in the csproj — do not edit files there.

## Architecture (mandatory rules)
Layered architecture enforced:
- `Controllers/` — thin HTTP controllers: validate model, delegate to services.
- `Domain/Models/` — domain/database entities
- `Domain/IService/` — business service interfaces
- `Domain/iRepositories/` — repository + `IUnitOfWork` interfaces
- `Service/` — business logic implementations
- `Persistence/Contex/` — `ApiDbContext`
- `Persistence/Repositories/` — repository + `UnitOfWork` implementations
- `DTO/` — Data Transfer Objects (never expose entities directly)
- `Utils/` — helpers, `ApiResponse<T>`, `ExceptionMiddleware`, exceptions
- `Hubs/` — SignalR hubs (unused yet)
- `Migrations/` — EF Core migrations
- `Dockerfile`, `docker-compose.yml`

## Commands (run from `Api/`)
```powershell
# Build / test (solution includes Api + Api.Tests)
dotnet build Api.sln
dotnet test Api.sln

# Run (launches Swagger UI at /swagger by default)
dotnet run --project Api/Api.csproj

# EF migrations (uses local tool)
dotnet tool restore               # installs local dotnet-ef 8.0.11
dotnet ef migrations add <Name>   # via local tool: dotnet tool run dotnet-ef
dotnet ef database update
```

**Use the LOCAL `dotnet-ef` (8.0.11) via `.config/dotnet-tools.json`. The global `dotnet-ef` is v10 and fails to build this net8.0 project.**

## Dev server
- HTTP: `http://localhost:5169`
- HTTPS: `https://localhost:7219`
- Swagger UI launches on start in Development mode
- SQL Server with docker: `docker compose up -d db` (SA password in docker-compose.yml)

## Conventions
- Controller route: `api/[controller]`, DTOs for in/out, `ApiResponse<T>` envelope
- DI via constructor; services depend on `IUnitOfWork`/repositories, not DbContext
- Exceptions: throw `NotFoundException`/`BusinessException`; global `ExceptionMiddleware` maps them to 404/400
- Business logic/service changes require unit tests in `Api/Api.Tests/` (xUnit + Moq)