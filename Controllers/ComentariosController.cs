using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FutbotecaApi.Context;
using FutbotecaApi.Models;
using FutbotecaApi.Dtos;
using FutbotecaApi.Dtos.Update;
using FutbotecaApi.Dtos.Create;

namespace FutbotecaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComentariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Comentarios
        [HttpGet]
        public async Task<ActionResult> GetComentarios()
        {
            var comentarios = await _context.Comentarios.Select(c => new ComentarioDto_
            {
                Id=c.Id,
                Contenido=c.Contenido,
                Fecha=c.Fecha,
                UsuarioId=c.UsuarioId,
                VideoId=c.VideoId
            }).ToListAsync();

            return Ok(comentarios);
        }

        // GET: api/Comentarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetComentario(int id)
        {
            var encontrar = await _context.Comentarios.FindAsync(id);
            if (encontrar == null)
                return NotFound();

            var comentario = new ComentarioDto_
            {
                Id = encontrar.Id,
                Contenido = encontrar.Contenido,
                Fecha = encontrar.Fecha,
                UsuarioId= encontrar.UsuarioId,
                VideoId=encontrar.VideoId

            };    

            return Ok(comentario);
        }

        // POST: api/Comentarios
        [HttpPost]
        public async Task<ActionResult<ComentarioDto_>> CreateComentario([FromBody] CreateComentarioDto comentarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comentario = new Comentario
            {
                Contenido = comentarioDto.Contenido,
                UsuarioId = comentarioDto.UsuarioId,
                VideoId = comentarioDto.VideoId,
                Fecha = DateTime.Now
            };
            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

          return Ok(comentario);

        }

        // PUT: api/Comentarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComentario(int id, [FromBody] UpdateComentarioDto comentarioDto)
        {
            var comentarioBusqueda = await _context.Comentarios.FindAsync(id);

            if (comentarioBusqueda==null)
                return NotFound("No existe este comentario.");

            comentarioBusqueda.Contenido = comentarioDto.Contenido;
            comentarioBusqueda.UsuarioId = comentarioDto.UsuarioId;
            comentarioBusqueda.VideoId = comentarioDto.VideoId;
            comentarioBusqueda.Fecha = DateTime.Now;

            
                await _context.SaveChangesAsync();
            

            return Ok("El comentario se ha actualizado correctamente");
        }

        // DELETE: api/Comentarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
                return NotFound();

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return Ok(new {message= "Se eliminó correctamente" });
        }

      
      
    }
}
