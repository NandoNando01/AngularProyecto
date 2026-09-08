# Angula_Proyecto — two-subproject workspace

Two independent projects, each its **own git repo** (no repo at this root). Read each project's `AGENTS.md` — they are the authoritative, detailed guides.

| Directory | Stack | Repo / how to work |
|-----------|-------|--------------------|
| `Api/` | ASP.NET Core 8 Web API (layered architecture: MVC + Repository/UoW + DTO + EF Core SQL Server, Swagger, xUnit tests) | See `Api/AGENTS.md` — **build/run from `Api/`, NOT `Api/Api/`** (duplicated nested dir quirk). |
| `mi-app/` | Angular 22 + SSR + Vitest + Bootstrap 5 | See `mi-app/AGENTS.md` — standalone components, `@angular/build:unit-test` manages Vitest. |
| `features/` | empty | Placeholder, not yet used. |

## Working here

- Commands only work from inside the relevant subproject directory (e.g. `npm test` in `mi-app/`, `dotnet build Api.sln` in `Api/`). There are no root-level build/test scripts.
- Changes to one project don't affect the other; they are not wired together (the Angular app defines its own `/api/*` Express endpoints in `src/server.ts`).
- The `.NET` project enforces a **strict layered architecture** (controllers → services → repository/UoW → EF Core, DTOs only). See `Api/AGENTS.md`. Any new business logic **must** include xUnit + Moq tests in `Api/Api.Tests/`.
