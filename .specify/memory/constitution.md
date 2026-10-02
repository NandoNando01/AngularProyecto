<!-- Sync Impact Report: v0.0.0 → v1.0.0. Los 5 principios y 2 secciones creados desde cero. Gobierno establecido. Sin elementos pendientes. -->

# Constitución de Angula_Proyecto
<!-- Constitución de especificaciones para la aplicación full-stack AngularProyecto -->

## Principios Fundamentales

### I. Desarrollo Guiado por Especificaciones (SDD)
La fuente de verdad única es `specs/openapi.yaml`. Todo cambio en la API DEBE comenzar con la especificación. Nunca cambiar el contrato del backend o frontend sin actualizar la spec primero. La spec define todos los endpoints, DTOs, respuestas y códigos de error.

### II. Arquitectura en Capas (Backend)
ASP.NET Core 8 exige una arquitectura estricta en capas: Controllers → Services → Repository/UoW → EF Core. Los DTOs son los únicos contratos de datos que cruzan capas. Los Controllers DEBEN ser delgados — validar entrada y delegar a servicios. Sin lógica de negocio en controllers o repositorios. Las excepciones de negocio se mapean a códigos HTTP via el `ExceptionMiddleware` global.

### III. Test-First (NO NEGOCIABLE)
xUnit + Moq para backend .NET; Vitest para frontend Angular. Toda lógica de negocio y cambios en servicios DEBEN incluir tests unitarios. Seguir: Spec → Escribir tests fallidos → Implementar → Verificar. Ciclo Red-Green-Refactor estrictamente aplicado. Cobertura mínima: todos los métodos de servicio y casos borde.

### IV. Componentes Standalone en Angular
El frontend usa `bootstrapApplication` con componentes standalone (sin NgModules). Signals para estado reactivo, `styleUrl` (singular) para CSS externo, Vitest para testing unitario. Angular Material para componentes de UI. SSR via `@angular/ssr` + Express.

### V. Seguridad y Autenticación
Autenticación JWT Bearer token con hash de contraseñas PBKDF2 + SHA-256. Todos los endpoints `/api/product` requieren autenticación. `/api/auth/register` y `/api/auth/login` son públicos. Validación de entrada en todos los DTOs con mensajes de error adecuados. Las contraseñas DEBEN contener al menos una mayúscula y un dígito (mín. 8 caracteres).

## Stack Tecnológico

- **Backend**: .NET 8, EF Core 8, SQL Server 2022 (Docker), Swagger 6.4.0, JWT Bearer auth
- **Frontend**: Angular 22, SSR, Angular Material, Chart.js, Vitest, Bootstrap 5 (disponible globalmente)
- **Auth**: JWT Bearer tokens, PBKDF2 + SHA-256 (100K iteraciones, salt de 16 bytes)
- **Formato API**: OpenAPI 3.0 / `specs/openapi.yaml`
- **Infraestructura**: Docker Compose (API + SQL Server), dotnet-ef 8.0.11 para migraciones
- **CI/Calidad**: Prettier para formateo, sin eslint configurado

## Flujo de Desarrollo

Flujo SDD para cualquier cambio en la API:
1. Editar `specs/openapi.yaml` primero (endpoints, DTOs, respuestas)
2. Regenerar tipos del frontend: `cd mi-app && npm run generate:api-types`
3. Implementar el cambio en el backend .NET respetando la arquitectura en capas
4. Actualizar los servicios del frontend para que coincidan con la nueva spec
5. Validar: `node scripts/validate-spec.mjs`

Los comandos se ejecutan desde los directorios de cada subproyecto (no desde la raíz):
- `Api/` → `dotnet build Api.sln`, `dotnet test Api.sln`, `dotnet run --project Api/Api.csproj`
- `mi-app/` → `npm start` (dev en :4200), `npm test`, `npm run build`

## Gobierno

Esta constitución prevalece sobre todas las demás prácticas. Las enmiendas requieren documentación, aprobación y plan de migración. Todos los PRs deben verificar cumplimiento con `specs/openapi.yaml`. La complejidad debe justificarse — empezar simple, principios YAGNI. Usar los archivos `AGENTS.md` como guía de desarrollo en tiempo de ejecución a nivel de proyecto.

**Versión**: 1.0.0 | **Ratificada**: 2026-10-01 | **Última enmienda**: 2026-10-01