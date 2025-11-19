# futbotecaApi

API REST desarrollada en .NET Core para la gestión de usuarios, vídeos, comentarios, likes, seguimientos, notificaciones y reportes en una plataforma de fútbol.

## Tecnologías utilizadas (backend)

- **ASP.NET Core Web API** para la creación de endpoints RESTful.
- **Entity Framework Core** para el acceso y gestión de la base de datos relacional.
- **Autenticación JWT (Json Web Token)** para la autenticación y autorización segura de los usuarios.
- **ASP.NET Identity** y `UserManager<T>` para la gestión avanzada de usuarios.
- **Servicios personalizados** inyectados vía DI (por ejemplo, `JwtService` para generación de tokens JWT).

---

## Estructura principal

### Controladores

- `UsuariosController`: Registro, login, consulta y actualización de usuarios. Utiliza ASP.NET Identity para autenticación y administración, y emplea un servicio JWT para la emisión de tokens.
- `SeguimientoesController`: Lógica para seguir/dejar de seguir usuarios, listado de seguimientos.
- `NotificacionesController`: Lógica para consultar y administrar notificaciones vinculadas a usuarios y vídeos.
- Otros controladores para entidades como Videos, Comentarios, Likes, Reportes, etc.

### Servicios

- **JwtService**: Servicio responsable de la generación de tokens JWT firmados usando credenciales configuradas en `appsettings.json`, y el establecimiento de los claims pertinentes para cada usuario.

  - Uso:
    - Recibe el nombre de usuario.
    - Crea un token JWT con claims como `Name` y un identificador único (`Jti`), firmado y con fecha de expiración.
    - El controlador de usuarios usa este servicio tanto al registrar como autenticar usuarios.

  - Ejemplo de servicio:
    ```csharp
    public class JwtService
    {
        public JwtService(IConfiguration config) { ... }
        public string GenerarToken(string nombreUsuario) { ... }
    }
    ```

### Autenticación

- **Login/JWT**:
  - Al registrar o autenticar un usuario se genera un token JWT con la info relevante utilizando el servicio `JwtService`.
  - El token debe ser incluido en las llamadas protegidas en el header `Authorization: Bearer <token>`.

- **Claims**:
  - Los endpoints protegidos pueden recuperar la identidad del usuario mediante los claims incluidos en el JWT (`ClaimTypes.Name`, NameIdentifier, etc).

- **Protección de rutas**:
  - Se usan atributos como `[Authorize]` para limitar el acceso a endpoints a usuarios autenticados.

### Persistencia de datos

- **Entity Framework Core**
  - Diseño code-first: las entidades como Usuario, Video, Comentario, etc, están modeladas en C#.
  - `AppDbContext` hereda de `IdentityDbContext<ApplicationUser>` para combinar gestión de usuarios con el resto de entidades.

### Estructura de carpetas destacadas

- `/Controllers`: Controladores HTTP y lógica de negocio para cada entidad.
- `/Services`: Servicios personalizados como el de JWT.
- `/Context`: Contexto de la base de datos con configuración de EF Core.

---

## Instalación y ejecución

1. Clona el repositorio
   ```bash
   git clone https://github.com/Melk236/futbotecaApi.git
   cd futbotecaApi
   ```
2. Instala dependencias
   ```bash
   dotnet restore
   ```
3. Configura tus variables jwt y database en `appsettings.json` según tu entorno:
   ```json
   "Jwt": {
     "Key": "tu_clave_secreta",
     "Issuer": "tu_issuer",
     "Audience": "tu_audience",
     "ExpireMinutes": 120
   }
   ```
4. Ejecuta la API:
   ```bash
   dotnet run
   ```

---

## Endpoints principales

- **POST /api/Usuarios/register** — Registro de usuario
- **POST /api/Usuarios/login** — Autenticación y obtención de JWT
- **GET /api/Usuarios/perfil** — Consulta del perfil del usuario autenticado
- CRUD de Videos, Comentarios, Likes, Seguimientos, Reportes y Notificaciones (consulta los respectivos controladores para detalles)

---

## Autor

Melk236
