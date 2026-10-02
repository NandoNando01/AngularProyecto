# Api — ASP.NET Core 8 Web API

## Estructura del proyecto
- **Solución**: `Api.sln` (raíz) — proyectos `Api\Api.csproj` y `Api\Api.Tests\Api.Tests.csproj`
- **Punto de entrada**: `Api/Program.cs` usando top-level statements, configuración ASP.NET Core 8
- **Framework**: .NET 8, nullable habilitado, implicit usings habilitado
- **EF Core 8 + SQL Server** (via docker-compose), Swagger 6.4.0

## Peculiaridad del directorio duplicado
Hay dos directorios de proyecto anidados: `Api/` y `Api/Api/`. Ambos `Api/Api.csproj` y `Api/Api/Api.csproj` existen. **Trabajar desde `Api/` (el primer nivel), NO desde `Api/Api/`.** El `Api/Api/Api/` anidado está excluido de la compilación via `Compile Remove="Api\**\*.cs"` en el csproj — no editar archivos allí.

## Arquitectura (reglas obligatorias)
Arquitectura en capas exigida:
- `Controllers/` — controladores HTTP delgados: validar modelo, delegar a servicios.
- `Domain/Models/` — entidades de dominio/base de datos
- `Domain/IService/` — interfaces de servicios de negocio
- `Domain/iRepositories/` — interfaces de repositorio + `IUnitOfWork`
- `Service/` — implementaciones de lógica de negocio
- `Persistence/Contex/` — `ApiDbContext`
- `Persistence/Repositories/` — implementaciones de repositorio + `UnitOfWork`
- `DTO/` — Data Transfer Objects (nunca exponer entidades directamente)
- `Utils/` — helpers, `ApiResponse<T>`, `ExceptionMiddleware`, excepciones
- `Hubs/` — Hubs SignalR (aún sin usar)
- `Migrations/` — Migraciones EF Core
- `Dockerfile`, `docker-compose.yml`

## Comandos (ejecutar desde `Api/`)
```powershell
# Build / test (la solución incluye Api + Api.Tests)
dotnet build Api.sln
dotnet test Api.sln

# Ejecutar (lanza Swagger UI en /swagger por defecto)
dotnet run --project Api/Api.csproj

# Migraciones EF (usa herramienta local)
dotnet tool restore               # instala dotnet-ef 8.0.11 local
dotnet ef migrations add <Name>   # via herramienta local: dotnet tool run dotnet-ef
dotnet ef database update
```

**Usar el `dotnet-ef` LOCAL (8.0.11) via `.config/dotnet-tools.json`. El `dotnet-ef` global (v10) falla al compilar este proyecto net8.0.**

## Servidor de desarrollo
- HTTP: `http://localhost:5169`
- HTTPS: `https://localhost:7219`
- Swagger UI se abre al iniciar en modo Development
- SQL Server con docker: `docker compose up -d db` (contraseña SA en docker-compose.yml)

## Convenciones
- Ruta del controller: `api/[controller]`, DTOs para entrada/salida, envoltura `ApiResponse<T>`
- DI via constructor; los servicios dependen de `IUnitOfWork`/repositorios, no de DbContext
- Excepciones: lanzar `NotFoundException`/`BusinessException`; el `ExceptionMiddleware` global las mapea a 404/400
- Los cambios en lógica de negocio/servicios requieren tests unitarios en `Api/Api.Tests/` (xUnit + Moq)

## Desarrollo Guiado por Especificaciones (SDD)
La API **debe** coincidir con `../specs/openapi.yaml` (relativo a este directorio).  
**Flujo**: spec primero → backend → frontend → validar.

1. Editar `../specs/openapi.yaml` con nuevos endpoints/DTOs/errores
2. Construir la implementación .NET
3. Regenerar tipos del frontend (`cd ../mi-app && npm run generate:api-types`)
4. Validar: `node ../scripts/validate-spec.mjs`

**No** agregar endpoints o DTOs sin actualizar la spec primero.