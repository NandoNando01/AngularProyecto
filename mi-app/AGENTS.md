# mi-app — Angular 22 + SSR + Vitest

## Dev commands

| Command | Action |
|---------|--------|
| `npm start` | Dev server on `http://localhost:4200` |
| `npm run build` | Production build to `dist/` |
| `npm test` | Run Vitest unit tests (managed by Angular CLI) |
| `npm run watch` | Build with `--configuration development` watch mode |
| `npm run serve:ssr:mi-app` | Serve SSR build from `dist/mi-app/server/server.mjs` |
| `ng generate component <name>` | Generate standalone component |

No eslint or stylelint configured. Format with Prettier (config in `.prettierrc`: `printWidth: 100`, `singleQuote: true`, HTML uses `"angular"` parser).

## Architecture

- **Standalone components** (`bootstrapApplication`, no NgModules). New components use `imports` array on `@Component`.
- **SSR** via `@angular/ssr` + Express. Server entry: `src/server.ts`, listens on `$PORT` or `4000`.
- **Testing**: Vitest (no Karma). No vitest config file — `@angular/build:unit-test` builder manages it. Types from `vitest/globals` in `tsconfig.spec.json`.
- **Bootstrap 5** available globally from `styles.css`.
- Routes defined in `src/app/app.routes.ts` (currently empty). SSR route modes in `src/app/app.routes.server.ts` (default: `Prerender`).

## File conventions

- Tests: `src/**/*.spec.ts` (excluded from app build via `tsconfig.app.json`)
- Components use `styleUrl` (singular) and `templateUrl` for external files. Signals for reactive state.
- EditorConfig: 2-space indent, single quotes for TS.
- `src/server.ts` can define Express API endpoints under `/api/*`.