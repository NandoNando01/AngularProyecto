# mi-app — Angular 22 + SSR + Vitest

## Comandos de desarrollo

| Comando | Acción |
|---------|--------|
| `npm start` | Servidor de desarrollo en `http://localhost:4200` |
| `npm run build` | Build de producción en `dist/` |
| `npm test` | Ejecutar tests unitarios Vitest (gestionado por Angular CLI) |
| `npm run watch` | Build con `--configuration development` watch mode |
| `npm run serve:ssr:mi-app` | Servir build SSR desde `dist/mi-app/server/server.mjs` |
| `ng generate component <name>` | Generar componente standalone |

Sin eslint ni stylelint configurados. Formatear con Prettier (config en `.prettierrc`: `printWidth: 100`, `singleQuote: true`, HTML usa parser `"angular"`).

## Arquitectura

- **Componentes standalone** (`bootstrapApplication`, sin NgModules). Los nuevos componentes usan array `imports` en `@Component`.
- **SSR** via `@angular/ssr` + Express. Entrada del servidor: `src/server.ts`, escucha en `$PORT` o `4000`.
- **Testing**: Vitest (sin Karma). Sin archivo de config vitest — el builder `@angular/build:unit-test` lo gestiona. Tipos desde `vitest/globals` en `tsconfig.spec.json`.
- **Bootstrap 5** disponible globalmente desde `styles.css`.
- Rutas definidas en `src/app/app.routes.ts` (actualmente vacías). Modos de ruta SSR en `src/app/app.routes.server.ts` (default: `Prerender`).

## Desarrollo Guiado por Especificaciones (SDD)

Los tipos del frontend se **auto-generan** desde `specs/openapi.yaml` (raíz del proyecto):

```bash
npm run generate:api-types   # → src/app/models/generated/api-types.ts
```

- **Siempre** regenerar después de cualquier cambio en la spec.
- `src/app/models/auth.model.ts` re-exporta desde `generated/api-types.ts` — **no** editar definiciones de tipos manuales.

## Convenciones de archivos

- Tests: `src/**/*.spec.ts` (excluidos del build de la app via `tsconfig.app.json`)
- Los componentes usan `styleUrl` (singular) y `templateUrl` para archivos externos. Signals para estado reactivo.
- EditorConfig: indentación de 2 espacios, comillas simples para TS.
- `src/server.ts` puede definir endpoints Express API bajo `/api/*`.