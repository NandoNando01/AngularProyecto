# language: es
@backend @auth @registro
Característica: Registro de Usuario
  Como sistema API
  Permitir que un nuevo usuario se registre con sus credenciales
  Para que pueda autenticarse y acceder a los recursos protegidos

  # ──────────────────────────────────────────────
  # Contracto del endpoint
  # ──────────────────────────────────────────────
  # POST /api/auth/register
  #
  # Request Body (RegisterDto):
  #   {
  #     "email":    string (requerido, formato email, max 150),
  #     "password": string (requerido, min 8, max 100),
  #     "fullName": string (requerido, min 2, max 100)
  #   }
  #
  # Response (201 Created):
  #   ApiResponse<AuthResponseDto> {
  #     "success": true,
  #     "message": "Usuario registrado correctamente.",
  #     "data": {
  #       "id":       int,
  #       "email":    string,
  #       "fullName": string,
  #       "token":    string (JWT),
  #       "expiresAt": datetime (UTC)
  #     }
  #   }
  #
  # Response (400 Bad Request):
  #   ApiResponse<null> {
  #     "success": false,
  #     "message": "<motivo del error>",
  #     "data": null
  #   }
  # ──────────────────────────────────────────────

  Regla: El correo electrónico debe ser único en el sistema
    Escenario: Registro exitoso con datos válidos
      Dado que no existe un usuario con el correo "nuevo@example.com"
      Cuando envío una solicitud POST a "/api/auth/register" con:
        | campo    | valor                |
        | email    | nuevo@example.com    |
        | password | Segura#123           |
        | fullName | Juan Pérez           |
      Entonces la respuesta HTTP debe ser 201
      Y la respuesta debe contener "success" con valor true
      Y la respuesta debe contener un "token" JWT no nulo
      Y la respuesta debe contener "expiresAt" con fecha futura
      Y el usuario debe existir en la base de datos con el correo "nuevo@example.com"

    Escenario: Rechazo por correo duplicado
      Dado que ya existe un usuario con el correo "existente@example.com"
      Cuando envío una solicitud POST a "/api/auth/register" con:
        | campo    | valor                   |
        | email    | existente@example.com   |
        | password | Segura#123              |
        | fullName | María López             |
      Entonces la respuesta HTTP debe ser 400
      Y la respuesta debe contener "success" con valor false
      Y la respuesta debe contener el mensaje "El correo electrónico ya está registrado."

  Regla: La contraseña debe cumplir requisitos de seguridad
    Escenario: Rechazo por contraseña débil - menor a 8 caracteres
      Dado que no existe un usuario con el correo "debil@example.com"
      Cuando envío una solicitud POST a "/api/auth/register" con:
        | campo    | valor              |
        | email    | debil@example.com  |
        | password | Ab1#               |
        | fullName | Usuario Débil      |
      Entonces la respuesta HTTP debe ser 400
      Y la respuesta debe contener "success" con valor false

    Escenario: Rechazo por contraseña sin letras mayúsculas
      Dado que no existe un usuario con el correo "nomayuscula@example.com"
      Cuando envío una solicitud POST a "/api/auth/register" con:
        | campo    | valor                      |
        | email    | nomayuscula@example.com    |
        | password | segura#123                 |
        | fullName | Sin Mayúsculas             |
      Entonces la respuesta HTTP debe ser 400
      Y la respuesta debe contener "success" con valor false

    Escenario: Rechazo por contraseña sin números
      Dado que no existe un usuario con el correo "nonumero@example.com"
      Cuando envío una solicitud POST a "/api/auth/register" con:
        | campo    | valor                    |
        | email    | nonumero@example.com     |
        | password | SeguraSinNum#            |
        | fullName | Sin Números              |
      Entonces la respuesta HTTP debe ser 400
      Y la respuesta debe contener "success" con valor false

  Regla: Todos los campos obligatorios deben estar presentes
    Escenario: Rechazo por campos faltantes
      Dado que no existe un usuario con el correo "incompleto@example.com"
      Cuando envío una solicitud POST a "/api/auth/register" con:
        | campo   | valor                  |
        | email   | incompleto@example.com |
      Entonces la respuesta HTTP debe ser 400
      Y la respuesta debe contener "success" con valor false

    Escenario: Rechazo por formato de correo inválido
      Dado que no existe un usuario con el correo "noemail"
      Cuando envío una solicitud POST a "/api/auth/register" con:
        | campo    | valor            |
        | email    | noemail          |
        | password | Segura#123       |
        | fullName | Sin Correo       |
      Entonces la respuesta HTTP debe ser 400
      Y la respuesta debe contener "success" con valor false

  Regla: La respuesta debe seguir el patrón ApiResponse<T>
    Escenario: Estructura de respuesta exitosa
      Dado que no existe un usuario con el correo "estructura@example.com"
      Cuando envío una solicitud POST a "/api/auth/register" con:
        | campo    | valor                     |
        | email    | estructura@example.com    |
        | password | Segura#123                |
        | fullName | Test Estructura           |
      Entonces la respuesta HTTP debe ser 201
      Y la respuesta debe contener los campos "success", "message" y "data"
      Y el campo "data" debe contener los campos "id", "email", "fullName", "token" y "expiresAt"
