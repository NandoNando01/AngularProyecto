# language: es
@frontend @auth @login
Característica: Formulario de Inicio de Sesión (Login)
  Como usuario de la aplicación Angular
  Poder autenticarme desde la interfaz de usuario
  Para acceder al sistema con mi cuenta existente

  # ──────────────────────────────────────────────
  # Componente: LoginComponent
  # Ruta: /login
  # Servicio consumido: AuthService.login(LoginPayload)
  # Modelo de salida: AuthResponse { id, email, fullName, token, expiresAt }
  # ──────────────────────────────────────────────
  #
  # Formulario reactivo (FormGroup):
  #   - email:    FormControl (requerido, email pattern)
  #   - password: FormControl (requerido)
  #
  # Flujo:
  #   1. Usuario ingresa correo y contraseña
  #   2. Se validan los controles en tiempo real (onBlur + onValueChanges)
  #   3. Al enviar → AuthService.login() → POST /api/auth/login
  #   4. Éxito → guardar token en localStorage → redirigir a /dashboard
  #   5. Error → mostrar mensaje inline "Credenciales inválidas."

  Regla: El formulario solo se envía si todos los campos son válidos
    Escenario: El botón de iniciar sesión está deshabilitado con formulario inválido
      Dado que estoy en la página de login
      Y el formulario tiene los campos vacíos
      Entonces el botón "Iniciar Sesión" debe estar deshabilitado

    Escenario: El botón de iniciar sesión se habilita con formulario válido
      Dado que estoy en la página de login
      Y completo el campo "Correo electrónico" con "usuario@test.com"
      Y completo el campo "Contraseña" con "MiContraseña#1"
      Entonces el botón "Iniciar Sesión" debe estar habilitado

  Regla: La validación de correo electrónico es estricta
    Escenario: Mensaje de error por formato de correo inválido
      Dado que estoy en la página de login
      Y completo el campo "Correo electrónico" con "texto-plano"
      Y hago clic fuera del campo
      Entonces debo ver el mensaje de error "Ingrese un correo electrónico válido."

    Escenario: Mensaje de error al dejar correo vacío
      Dado que estoy en la página de login
      Y hago clic en el campo "Correo electrónico"
      Y hago clic fuera del campo
      Entonces debo ver el mensaje de error "El correo es obligatorio."

  Regla: La validación de contraseña es obligatoria
    Escenario: Mensaje de error al dejar contraseña vacía
      Dado que estoy en la página de login
      Y hago clic en el campo "Contraseña"
      Y hago clic fuera del campo
      Entonces debo ver el mensaje de error "La contraseña es obligatoria."

  Regla: La comunicación con el backend se realiza mediante AuthService
    Escenario: Login exitoso redirige al dashboard
      Dado que estoy en la página de login
      Y completo el campo "Correo electrónico" con "usuario@test.com"
      Y completo el campo "Contraseña" con "MiContraseña#1"
      Cuando hago clic en el botón "Iniciar Sesión"
      Entonces se debe realizar una petición POST al servicio de login
      Y debo ser redirigido a la ruta "/dashboard"
      Y el token JWT debe almacenarse en localStorage

    Escenario: Login fallido muestra error sin redirigir
      Dado que estoy en la página de login
      Y completo el campo "Correo electrónico" con "usuario@test.com"
      Y completo el campo "Contraseña" con "ContraseñaIncorrecta#1"
      Y el backend responde con error 401: "Credenciales inválidas."
      Cuando hago clic en el botón "Iniciar Sesión"
      Entonces debo ver un mensaje de error general "Credenciales inválidas."
      Y no debo ser redirigido
      Y no debe almacenarse ningún token en localStorage

  Regla: El formulario muestra indicadores de carga durante el envío
    Escenario: Spinner visible durante la petición al backend
      Dado que estoy en la página de login
      Y completo el campo "Correo electrónico" con "usuario@test.com"
      Y completo el campo "Contraseña" con "MiContraseña#1"
      Cuando hago clic en el botón "Iniciar Sesión"
      Entonces el botón debe mostrarse en estado de carga
      Y el botón "Iniciar Sesión" debe estar deshabilitado

  Regla: El formulario permite navegar al registro
    Escenario: Link de navegación a registro
      Dado que estoy en la página de login
      Cuando hago clic en el enlace "¿No tienes cuenta? Regístrate"
      Entonces debo ser redirigido a la ruta "/register"

  Regla: La sesión se gestiona desde el frontend
    Escenario: Almacena el token correctamente tras login exitoso
      Dado que estoy en la página de login
      Y completo el campo "Correo electrónico" con "usuario@test.com"
      Y completo el campo "Contraseña" con "MiContraseña#1"
      Cuando hago clic en el botón "Iniciar Sesión"
      Entonces el token JWT debe almacenarse en localStorage bajo la clave "auth_token"
      Y los datos del usuario (id, email, fullName) deben almacenarse en localStorage bajo la clave "user_data"

    Escenario: Redirección automática si ya existe un token válido
      Dado que ya existe un token JWT válido en localStorage
      Cuando navego a la ruta "/login"
      Entonces debo ser redirigido automáticamente a la ruta "/dashboard"
