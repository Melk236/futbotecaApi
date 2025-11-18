using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FutbotecaApi.Context;
using FutbotecaApi.Models;
using FutbotecaApi.Dtos;
using FutbotecaApi.Dtos.Create;
using FutbotecaApi.Dtos.Update;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FutbotecaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguimientosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeguimientosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Seguimientos
        [HttpGet]
        public async Task<ActionResult> GetSeguimientos()
        {
            var seguimientos = await _context.Seguimientos.Select(c => new SeguimientoDto
            {
                Id = c.Id,
                SeguidoId = c.SeguidoId,
                SeguidorId = c.SeguidorId
            }).ToListAsync();

            return Ok(seguimientos);
        }

        // GET: api/Seguimientos/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSeguimiento(int id)
        {
            var buscaSeguimiento = await _context.Seguimientos.FindAsync(id);

            if (buscaSeguimiento == null)
                return NotFound("No se ha encontrado el seguimiento");

            var seguimiento = new SeguimientoDto
            {
                Id = buscaSeguimiento.Id,
                SeguidoId = buscaSeguimiento.SeguidoId,
                SeguidorId = buscaSeguimiento.SeguidorId
            };

            return Ok(seguimiento);
        }

        // POST: api/Seguimientos
        [HttpPost]
        public async Task<ActionResult<SeguimientoDto>> CreateSeguimiento([FromBody] CreateSeguimientoDto seguimientoDto)
        {
            if (seguimientoDto.SeguidorId == seguimientoDto.SeguidoId)
                return BadRequest("Un usuario no puede seguirse a sí mismo.");

            var yaExiste = await _context.Seguimientos
                .AnyAsync(s => s.SeguidorId == seguimientoDto.SeguidorId && s.SeguidoId == seguimientoDto.SeguidoId);

            if (yaExiste)
                return Conflict("Ya existe ese seguimiento.");

            var seguimiento = new Seguimiento
            {
                SeguidoId = seguimientoDto.SeguidoId,
                SeguidorId = seguimientoDto.SeguidorId
            };
            _context.Seguimientos.Add(seguimiento);
            await _context.SaveChangesAsync();

            return Ok(seguimiento);
        }

        // PUT: api/Seguimientos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeguimiento(int id, [FromBody] UpdateSeguimientoDto seguimientoDto)
        {
            var seguimiento = await _context.Seguimientos.FindAsync(id); 
            if (seguimiento==null)
                return NotFound("El seguimiento especificado no existe");

            seguimiento.SeguidorId = seguimientoDto.SeguidorId;
            seguimiento.SeguidoId = seguimiento.SeguidoId;

           
                await _context.SaveChangesAsync();
            
          
            

            return Ok(new { message = "El seguimiento se ha actualizado correctamente" });
        }

        // DELETE: api/Seguimientos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguimiento(int id)
        {
            var seguimiento = await _context.Seguimientos.FindAsync(id);
            if (seguimiento == null)
                return NotFound("No se ha encontrado el seguimiento especificado");

            _context.Seguimientos.Remove(seguimiento);
            await _context.SaveChangesAsync();

            return Ok(new {message= "Se ha eliminado correctamente"});
        }

     
    }
}
