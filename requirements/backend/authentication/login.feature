# language: es
@backend @auth @login
Característica: Inicio de Sesión (Login)
  Como sistema API
  Permitir que un usuario existente se autentique con correo y contraseña
  Para emitir un token JWT que autorice el acceso a endpoints protegidos

  # ──────────────────────────────────────────────
  # Contracto del endpoint
  # ──────────────────────────────────────────────
  # POST /api/auth/login
  #
  # Request Body (LoginDto):
  #   {
  #     "email":    string (requerido, formato email),
  #     "password": string (requerido)
  #   }
  #
  # Response (200 OK):
  #   ApiResponse<AuthResponseDto> {
  #     "success": true,
  #     "message": "Inicio de sesión exitoso.",
  #     "data": {
  #       "id":        int,
  #       "email":     string,
  #       "fullName":  string,
  #       "token":     string (JWT),
  #       "expiresAt": datetime (UTC)
  #     }
  #   }
  #
  # Response (401 Unauthorized):
  #   ApiResponse<null> {
  #     "success": false,
  #     "message": "Credenciales inválidas.",
  #     "data": null
  #   }
  #
  # Response (400 Bad Request):
  #   ApiResponse<null> {
  #     "success": false,
  #     "message": "<motivo del error de validación>",
  #     "data": null
  #   }
  # ──────────────────────────────────────────────

  Regla: Las credenciales deben ser válidas para autenticar
    Escenario: Login exitoso con credenciales correctas
      Dado que existe un usuario registrado con:
        | campo    | valor                  |
        | email    | usuario@test.com       |
        | password | MiContraseña#1         |
        | fullName | Carlos Rodríguez       |
      Cuando envío una solicitud POST a "/api/auth/login" con:
        | campo    | valor            |
        | email    | usuario@test.com |
        | password | MiContraseña#1   |
      Entonces la respuesta HTTP debe ser 200
      Y la respuesta debe contener "success" con valor true
      Y la respuesta debe contener un "token" JWT no nulo
      Y la respuesta debe contener "expiresAt" con fecha futura

    Escenario: Rechazo por contraseña incorrecta
      Dado que existe un usuario registrado con:
        | campo    | valor                  |
        | email    | usuario@test.com       |
        | password | MiContraseña#1         |
        | fullName | Carlos Rodríguez       |
      Cuando envío una solicitud POST a "/api/auth/login" con:
        | campo    | valor              |
        | email    | usuario@test.com   |
        | password | ContraseñaIncorrecta#1 |
      Entonces la respuesta HTTP debe ser 401
      Y la respuesta debe contener "success" con valor false
      Y la respuesta debe contener el mensaje "Credenciales inválidas."

    Escenario: Rechazo por correo inexistente
      Dado que no existe un usuario con el correo "noexiste@test.com"
      Cuando envío una solicitud POST a "/api/auth/login" con:
        | campo    | valor              |
        | email    | noexiste@test.com  |
        | password | Cualquier#1        |
      Entonces la respuesta HTTP debe ser 401
      Y la respuesta debe contener "success" con valor false
      Y la respuesta debe contener el mensaje "Credenciales inválidas."

  Regla: Los campos de entrada deben ser válidos
    Escenario: Rechazo por campos vacíos
      Cuando envío una solicitud POST a "/api/auth/login" con:
        | campo    | valor  |
        | email    |        |
        | password |        |
      Entonces la respuesta HTTP debe ser 400
      Y la respuesta debe contener "success" con valor false

    Escenario: Rechazo por formato de correo inválido
      Cuando envío una solicitud POST a "/api/auth/login" con:
        | campo    | valor           |
        | email    | no-es-email     |
        | password | Alguna#1        |
      Entonces la respuesta HTTP debe ser 400
      Y la respuesta debe contener "success" con valor false

    Escenario: Rechazo por campos faltantes
      Cuando envío una solicitud POST a "/api/auth/login" con:
        | campo   | valor            |
        | email   | user@test.com    |
      Entonces la respuesta HTTP debe ser 400
      Y la respuesta debe contener "success" con valor false

  Regla: El token JWT debe tener una fecha de expiración válida
    Escenario: El token expira según la configuración del sistema
      Dado que existe un usuario registrado con:
        | campo    | valor                  |
        | email    | token@test.com         |
        | password | TokenTest#1            |
        | fullName | Token User             |
      Cuando envío una solicitud POST a "/api/auth/login" con:
        | campo    | valor          |
        | email    | token@test.com |
        | password | TokenTest#1    |
      Entonces la respuesta HTTP debe ser 200
      Y el "expiresAt" del token debe ser mayor que la fecha actual
      Y el "expiresAt" del token debe ser menor o igual a 24 horas desde ahora

  Regla: La respuesta debe seguir el patrón ApiResponse<T>
    Escenario: Estructura de respuesta exitosa
      Dado que existe un usuario registrado con:
        | campo    | valor                  |
        | email    | estructura@test.com    |
        | password | Estruc#123             |
        | fullName | Test Estructura        |
      Cuando envío una solicitud POST a "/api/auth/login" con:
        | campo    | valor              |
        | email    | estructura@test.com |
        | password | Estruc#123         |
      Entonces la respuesta HTTP debe ser 200
      Y la respuesta debe contener los campos "success", "message" y "data"
      Y el campo "data" debe contener los campos "id", "email", "fullName", "token" y "expiresAt"
