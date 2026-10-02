# Angula_Proyecto — espacio de trabajo con dos subproyectos

Dos proyectos independientes, cada uno con su **propio repo git**. Leé el `AGENTS.md` de cada proyecto — son las guías autoritativas detalladas.

| Directorio | Stack | Repo / cómo trabajar |
|------------|-------|----------------------|
| `Api/` | ASP.NET Core 8 Web API (arquitectura en capas: MVC + Repository/UoW + DTO + EF Core SQL Server, Swagger, tests xUnit) | Ver `Api/AGENTS.md` — **build/run desde `Api/`, NO desde `Api/Api/`** (peculiaridad del directorio anidado duplicado). |
| `mi-app/` | Angular 22 + SSR + Vitest + Bootstrap 5 | Ver `mi-app/AGENTS.md` — componentes standalone, `@angular/build:unit-test` gestiona Vitest. |
| `features/` | vacío | Placeholder, aún no usado. |

## Cómo trabajar aquí

- Los comandos sólo funcionan desde el directorio del subproyecto correspondiente (ej. `npm test` en `mi-app/`, `dotnet build Api.sln` en `Api/`). No hay scripts de build/test en la raíz.
- Los cambios en un proyecto no afectan al otro; no están conectados entre sí (la app Angular define sus propios endpoints `/api/*` Express en `src/server.ts`).
- El proyecto `.NET` exige una **arquitectura estricta en capas** (controllers → services → repository/UoW → EF Core, sólo DTOs). Ver `Api/AGENTS.md`. Toda nueva lógica de negocio **debe** incluir tests xUnit + Moq en `Api/Api.Tests/`.

## SDD — Desarrollo Guiado por Especificaciones (Spec-Driven Development)

Este proyecto sigue **Desarrollo Guiado por Especificaciones**. La fuente de verdad única es `specs/openapi.yaml`.

**Flujo de trabajo para cualquier cambio en la API:**
1. Editar `specs/openapi.yaml` primero (agregar/cambiar endpoints, DTOs, respuestas)
2. Regenerar tipos del frontend: `cd mi-app && npm run generate:api-types`
3. Implementar el cambio en el backend .NET
4. Actualizar los servicios del frontend para que coincidan con la nueva spec
5. Validar: `node scripts/validate-spec.mjs`

**Nunca** cambies el contrato del backend o frontend sin actualizar la spec primero.