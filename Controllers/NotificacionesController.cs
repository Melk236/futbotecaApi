using FutbotecaApi.Context;
using FutbotecaApi.Dtos.Create;
using FutbotecaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FutbotecaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificacionesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {

            var notificacion = await _context.Notificaciones.Select(x => new
            {
                x.Id,
                x.Leida,
                x.Tipo,
                x.UsuarioEnviaId,
                x.UsuarioRecibeId,
                x.FechaCreacion,
                x.VideoId
            }).ToListAsync();

            return Ok(notificacion);

        }
        [HttpPost]
        public async Task<IActionResult> Crear(CreateNotificacionDto dto)
        {
            var notificacion = new Notificaciones
            {
                Tipo = dto.tipo,
                Leida = false,
                UsuarioRecibeId = dto.UsuarioRecibeId,
                UsuarioEnviaId = dto.UsuarioEnviaId,
                VideoId=dto.VideoId

            };
            _context.AddAsync(notificacion);
            await _context.SaveChangesAsync();
            return Ok(notificacion);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, CreateNotificacionDto dto)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);

            if (notificacion==null)
            {
                return NotFound(new {message="No se ha encontrado la noticación"});
            }
            notificacion.Tipo = dto.tipo;
            notificacion.Leida = true;
            notificacion.UsuarioEnviaId = dto.UsuarioEnviaId;
            notificacion.UsuarioRecibeId = dto.UsuarioRecibeId;
            await _context.SaveChangesAsync();
            return Ok(notificacion);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarVideo(int id)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);
            if (notificacion== null)
            {
                return NotFound(new { message = "No se encontró el video" });
            }

            // Borrar todas las notificaciones que tengan ese VideoId
           
            

            _context.Notificaciones.Remove(notificacion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Video y notificaciones eliminados correctamente" });
        }

    }
}
