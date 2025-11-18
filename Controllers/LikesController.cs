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
    public class LikesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LikesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Likes
        [HttpGet]
        public async Task<ActionResult> GetLikes()
        {
            var likes = await _context.Likes.Select(c => new LikeDto
            {
                Id = c.Id,
                Fecha = c.Fecha,
                UsuarioId = c.UsuarioId,
                VideoId = c.VideoId
            }).ToListAsync();
                

            return Ok(likes);
        }

        // GET: api/Likes/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetLike(int id)
        {
            var buscaLike = await _context.Likes.FindAsync(id);
                

            if (buscaLike == null)
                return NotFound("No se ha encontrado el like especificado");

            var like = new LikeDto
            {
                Id = buscaLike.Id,
                Fecha = buscaLike.Fecha,
                UsuarioId = buscaLike.UsuarioId,
                VideoId = buscaLike.VideoId
            };
            return Ok(like);
        }

        // POST: api/Likes
        [HttpPost]
        public async Task<ActionResult<LikeDto>> CreateLike([FromBody] CreateLikeDto likeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var like = new Like
            {
                UsuarioId = likeDto.UsuarioId,
                VideoId = likeDto.VideoId,
                Fecha = DateTime.Now

            };
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok(like);
        }

        // PUT: api/Likes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLike(int id, [FromBody] UpdateLikeDto likeDto)
        {
            var like = await _context.Likes.FindAsync(id);
            
            if (like==null)
                return NotFound("ID del like no coincide.");

            like.Fecha = DateTime.Now;
            like.UsuarioId = likeDto.UsuarioId;
            like.VideoId = likeDto.VideoId;

                await _context.SaveChangesAsync();

            return Ok(new {message="Se ha ctualizado correctamente"});
        }

        // DELETE: api/Likes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);

            if (like == null)
                return NotFound();

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return Ok(new {message= "Se ha eliminado el like correctamente"});
        }

        
    }
}
