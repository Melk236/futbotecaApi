using Microsoft.AspNetCore.Mvc;
using FutbotecaApi.Dtos.Create;
using Microsoft.AspNetCore.Identity;
using FutbotecaApi.Dtos;
using FutbotecaApi.Models;
using FutbotecaApi.Context;
using Microsoft.EntityFrameworkCore;
using FutbotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FutbotecaApi.Dtos.Update;


namespace FutbotecaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context; // Para la tabla Usuarios
        private readonly JwtService _jwtService;

        public UsuariosController(UserManager<ApplicationUser> userManager, AppDbContext context, JwtService servicio)
        {
            _userManager = userManager;
            _context = context;
            _jwtService = servicio;

        }

        // Registro de usuario
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUsuarioDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.NombreUsuario,

                Avatar = dto.Avatar
            };

            // Crear el usuario en Identity
            var result = await _userManager.CreateAsync(user, dto.Contrasena);

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = passwordHasher.HashPassword(user, dto.Contrasena);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Ahora que el usuario está registrado en Identity, lo guardamos en nuestra tabla 'Usuarios'
            var usuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                Contraseña = hashedPassword,  // Debes encriptar la contraseña antes de almacenarla
                Avatar = dto.Avatar,
                FechaRegistro = DateTime.Now
            };


            // Guardamos el nuevo usuario en la tabla 'Usuarios'

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            var token = _jwtService.GenerarToken(user.UserName);
            return Ok(new { message = token });
        }

        // Login de usuario

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.NombreUsuario);

            
            if (user == null || user.UserName != dto.NombreUsuario ||
                !await _userManager.CheckPasswordAsync(user, dto.Contrasena))
                return Unauthorized(new { message = "Credenciales inválidas" });

            var token = _jwtService.GenerarToken(user.UserName);
            return Ok(new { message = token });
        }
        [HttpGet("perfil")]
        public async Task<IActionResult> getPerfil()
        {
            // Intenta diferentes formas de obtener el nombre de usuario
            string nombreUsuario = null;

            // Opción 1: Claim específico con URI completo
            var nameClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            if (nameClaim != null)
            {
                nombreUsuario = nameClaim.Value;
            }

            // Opción 2: Buscar por ClaimTypes.Name
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                nombreUsuario = User.FindFirst(ClaimTypes.Name)?.Value;
            }

            // Opción 3: User.Identity.Name
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                nombreUsuario = User.Identity?.Name;
            }

            // Opción 4: NameIdentifier
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                nombreUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            if (string.IsNullOrEmpty(nombreUsuario))
            {
                return Unauthorized(new { message = "No se pudo identificar al usuario" });
            }

            var usuario = await _context.Usuarios
                .Where(u => u.NombreUsuario == nombreUsuario)
                .Select(u => new
                {
                    u.Id,
                    u.NombreUsuario,
                    u.Avatar,
                    u.FechaRegistro,
                    u.Descripcion,


                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new { message = $"Usuario {nombreUsuario} no encontrado" });
            }

            return Ok(usuario);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuarioAdmin(int id, [FromForm] UpdateUsuarioDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            // Manejar archivo de avatar
            var archivo = dto.Avatar;
            if (archivo != null)
            {
                var extensiones = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(archivo.FileName).ToLower();
                if (!extensiones.Contains(extension))
                {
                    return BadRequest(new { message = "Extensión no válida" });
                }
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }
                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName).ToLower();
                var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo);
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }
                usuario.Avatar = "/uploads/" + nombreArchivo;
            }

            // Actualizar descripción
            if (!string.IsNullOrEmpty(dto.Descripcion))
            {
                usuario.Descripcion = dto.Descripcion;
            }

            // Actualizar nombre de usuario tanto en la tabla personalizada como en Identity
            if (!string.IsNullOrEmpty(dto.NombreUsuario) && dto.NombreUsuario != usuario.NombreUsuario)
            {
                // Verificar que el nuevo nombre de usuario no esté en uso
                var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == dto.NombreUsuario && u.Id != id);
                if (usuarioExistente != null)
                {
                    return BadRequest(new { message = "El nombre de usuario ya está en uso" });
                }

                // Buscar el usuario en Identity por el nombre actual
                var identityUser = await _userManager.FindByNameAsync(usuario.NombreUsuario);
                if (identityUser != null)
                {
                    // Actualizar el UserName en Identity
                    identityUser.UserName = dto.NombreUsuario;
                    identityUser.NormalizedUserName = dto.NombreUsuario.ToUpper();

                    var identityResult = await _userManager.UpdateAsync(identityUser);
                    if (!identityResult.Succeeded)
                    {
                        return BadRequest(new
                        {
                            message = "Error al actualizar usuario en Identity",
                            errors = identityResult.Errors.Select(e => e.Description)
                        });
                    }
                }

                // Actualizar en la tabla personalizada
                usuario.NombreUsuario = dto.NombreUsuario;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            var user = await _context.Usuarios.Select(c => new UsuarioDto2
            {
                Id = c.Id,
                NombreUsuario = c.NombreUsuario,
                Avatar = c.Avatar,
                FechaRegistro = c.FechaRegistro,
                Descripcion=c.Descripcion
            }
            ).ToListAsync();
            return Ok(user);

        }
        [HttpPut]
        public async Task<IActionResult> Actualizar([FromForm] UpdateUsuarioDto dto)
        {
            string nombreUsuario = User.FindFirst(ClaimTypes.Name).Value;
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                return BadRequest(new { message = "Usuario no auutenticado" });
            }
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no ncontrado" });

            }

            var archivo = dto.Avatar;
            if (archivo!=null)
            {
                var extensiones = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(archivo.FileName).ToLower();

                if (!extensiones.Contains(extension))
                {
                    return BadRequest(new { message = "Extensión no válida" });
                }
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }
                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName).ToLower();//La ruta en el servidor
                var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo);//La ruta actual + la ruta creada arriba.
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                usuario.Avatar = "/uploads/" + nombreArchivo;
            }
            
            if (!string.IsNullOrEmpty(dto.Descripcion))
            {
                usuario.Descripcion = dto.Descripcion;
            }
            if (dto.NombreUsuario!=null)
            {
                usuario.NombreUsuario = dto.NombreUsuario;
            }
           
            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> eliminar(int id)
        {
            // Buscar en la base de datos personalizada
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado en base de datos" });
            }

            // Buscar en Identity (UserManager)
            var identityUser = await _userManager.FindByNameAsync(usuario.NombreUsuario);

            if (identityUser != null)
            {
                var resultado = await _userManager.DeleteAsync(identityUser);

                if (!resultado.Succeeded)
                {
                    return BadRequest(new { message = "Error al eliminar usuario en Identity", errores = resultado.Errors });
                }
            }
            // Eliminar seguimientos donde este usuario es el seguidor
            var seguimientos = await _context.Seguimientos
                .Where(s => s.SeguidorId == id || s.SeguidoId == id)
                .ToListAsync();

            _context.Seguimientos.RemoveRange(seguimientos);
            // Eliminar del contexto personalizado
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

    }
}