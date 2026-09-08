# language: es
@frontend @auth @registro
Característica: Formulario de Registro de Usuario
  Como usuario de la aplicación Angular
  Poder registrarme desde la interfaz de usuario
  Para crear una cuenta y acceder al sistema

  # ──────────────────────────────────────────────
  # Componente: RegisterComponent
  # Ruta: /register
  # Servicio consumido: AuthService.register(RegisterPayload)
  # Modelo de salida: AuthResponse { id, email, fullName, token, expiresAt }
  # ──────────────────────────────────────────────
  #
  # Formulario reactivo (FormGroup):
  #   - fullName: FormControl (requerido, min 2, max 100)
  #   - email:    FormControl (requerido, email pattern)
  #   - password: FormControl (requerido, min 8, max 100)
  #
  # Flujo:
  #   1. Usuario completa el formulario
  #   2. Se validan los controles en tiempo real (onBlur + onValueChanges)
  #   3. Al enviar → AuthService.register() → POST /api/auth/register
  #   4. Éxito → guardar token en localStorage → redirigir a /dashboard
  #   5. Error → mostrar mensaje inline debajo del campo correspondiente

  Regla: El formulario solo se envía si todos los campos son válidos
    Escenario: El botón de registro está deshabilitado con formulario inválido
      Dado que estoy en la página de registro
      Y el formulario tiene los campos vacíos
      Entonces el botón "Registrarse" debe estar deshabilitado

    Escenario: El botón de registro se habilita con formulario válido
      Dado que estoy en la página de registro
      Y completo el campo "Nombre completo" con "Juan Pérez"
      Y completo el campo "Correo electrónico" con "juan@example.com"
      Y completo el campo "Contraseña" con "Segura#123"
      Entonces el botón "Registrarse" debe estar habilitado

  Regla: La validación de campo nombre completo es obligatoria
    Escenario: Mensaje de error al dejar nombre vacío y hacer blur
      Dado que estoy en la página de registro
      Y hago clic en el campo "Nombre completo"
      Y hago clic fuera del campo
      Entonces debo ver el mensaje de error "El nombre es obligatorio."

    Escenario: Mensaje de error al ingresar un solo carácter
      Dado que estoy en la página de registro
      Y completo el campo "Nombre completo" con "J"
      Y hago clic fuera del campo
      Entonces debo ver el mensaje de error "El nombre debe tener entre 2 y 100 caracteres."

  Regla: La validación de correo electrónico es estricta
    Escenario: Mensaje de error por formato de correo inválido
      Dado que estoy en la página de registro
      Y completo el campo "Correo electrónico" con "no-es-email"
      Y hago clic fuera del campo
      Entonces debo ver el mensaje de error "Ingrese un correo electrónico válido."

    Escenario: Mensaje de error al dejar correo vacío
      Dado que estoy en la página de registro
      Y hago clic en el campo "Correo electrónico"
      Y hago clic fuera del campo
      Entonces debo ver el mensaje de error "El correo es obligatorio."

  Regla: La validación de contraseña cumple estándares de seguridad
    Escenario: Mensaje de error por contraseña menor a 8 caracteres
      Dado que estoy en la página de registro
      Y completo el campo "Contraseña" con "Ab1#"
      Y hago clic fuera del campo
      Entonces debo ver el mensaje de error "La contraseña debe tener al menos 8 caracteres."

    Escenario: Mensaje de error al dejar contraseña vacía
      Dado que estoy en la página de registro
      Y hago clic en el campo "Contraseña"
      Y hago clic fuera del campo
      Entonces debo ver el mensaje de error "La contraseña es obligatoria."

  Regla: La comunicación con el backend se realiza mediante AuthService
    Escenario: Registro exitoso redirige al dashboard
      Dado que estoy en la página de registro
      Y completo el formulario con datos válidos:
        | campo           | valor              |
        | Nombre completo | Ana García         |
        | Correo          | ana@example.com    |
        | Contraseña      | Segura#123         |
      Cuando hago clic en el botón "Registrarse"
      Entonces se debe realizar una petición POST al servicio de registro
      Y debo ser redirigido a la ruta "/dashboard"
      Y el token JWT debe almacenarse en localStorage

    Escenario: Registro fallido muestra error sin redirigir
      Dado que estoy en la página de registro
      Y completo el formulario con datos válidos:
        | campo           | valor                  |
        | Nombre completo | Pedro López            |
        | Correo          | existente@example.com  |
        | Contraseña      | Segura#123             |
      Y el backend responde con error 400: "El correo electrónico ya está registrado."
      Cuando hago clic en el botón "Registrarse"
      Entonces debo ver un mensaje de error general "El correo electrónico ya está registrado."
      Y no debo ser redirigido
      Y no debe almacenarse ningún token en localStorage

  Regla: El formulario muestra indicadores de carga durante el envío
    Escenario: Spinner visible durante la petición al backend
      Dado que estoy en la página de registro
      Y completo el formulario con datos válidos:
        | campo           | valor              |
        | Nombre completo | Laura Martínez     |
        | Correo          | laura@example.com  |
        | Contraseña      | Segura#123         |
      Cuando hago clic en el botón "Registrarse"
      Entonces el botón debe mostrarse en estado de carga
      Y el botón "Registrarse" debe estar deshabilitado

  Regla: El formulario permite navegar al login
    Escenario: Link de navegación a login
      Dado que estoy en la página de registro
      Cuando hago clic en el enlace "¿Ya tienes cuenta? Inicia sesión"
      Entonces debo ser redirigido a la ruta "/login"
