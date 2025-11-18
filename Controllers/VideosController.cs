using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FutbotecaApi.Context;
using FutbotecaApi.Models;
using FutbotecaApi.Dtos;
using FutbotecaApi.Dtos.Create;
using FutbotecaApi.Dtos.Update;

namespace FutbotecaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VideosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Videos
        [HttpGet]
        public async Task<ActionResult> GetVideos()
        {
            var videos = await _context.Videos.Select(c => new VideoDto
            {
                Id = c.Id,
                Descripcion = c.Descripcion,
                FechaSubida = c.FechaSubida,
                Titulo = c.Titulo,
                Url = c.Url,
                UsuarioId = c.UsuarioId,
                Vistas = c.Vistas,
                Categoria=c.Categoria
                

            }).ToListAsync();
               

            return Ok(videos);
        }

        // GET: api/Videos/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetVideo(int id)
        {
            var buscavideo = await _context.Videos.FindAsync(id);
               

            if (buscavideo == null)
                return NotFound();

            var video = new VideoDto
            {
                Id = buscavideo.Id,
                FechaSubida = buscavideo.FechaSubida,
                Descripcion = buscavideo.Descripcion,
                Titulo = buscavideo.Titulo,
                Url = buscavideo.Url,
                Vistas = buscavideo.Vistas,
                Categoria=buscavideo.Categoria
            };
            return Ok(video);
        }

        // POST: api/Videos
        [HttpPost]
        public async Task<ActionResult<VideoDto>> CreateVideo([FromBody] CreateVideoDto videoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var video = new Video
            {
                Titulo = videoDto.Titulo,
                FechaSubida = DateTime.Now,
                Url = videoDto.Url,
                Descripcion = videoDto.Descripcion,
                UsuarioId = videoDto.UsuarioId,
                Categoria=videoDto.Categoria
            };
            var respuesta = new VideoDto
            {
                Titulo = videoDto.Titulo,
                FechaSubida = DateTime.Now,
                Url = videoDto.Url,
                Descripcion = videoDto.Descripcion,
                UsuarioId = videoDto.UsuarioId

            };
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return Ok(video);
        }

        // PUT: api/Videos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideo(int id, [FromBody] UpdateVideoDto videoDto)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video==null)
                return BadRequest("El ID del video no coincide.");
            video.Titulo = videoDto.Titulo;
           video.Url= videoDto.Url;
           video.Descripcion= videoDto.Descripcion;
           video.FechaSubida=DateTime.Now;
           video.UsuarioId= videoDto.UsuarioId;
           video.Vistas= videoDto.Vistas;
           video.Categoria = videoDto.Categoria;
                await _context.SaveChangesAsync();
            
            

            return Ok(new { message="Se ha actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
                return NotFound(new { message = "No se encontró el video" });

            var notificaciones = await _context.Notificaciones
                .Where(n => n.VideoId == id)
                .ToListAsync();

            if (notificaciones.Any())
            {
                _context.Notificaciones.RemoveRange(notificaciones);
            }

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Video y notificaciones eliminados correctamente" });
        }

    }
}
