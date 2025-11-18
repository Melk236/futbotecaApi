using FutbotecaApi.Context;
using FutbotecaApi.Dtos.Create;
using FutbotecaApi.Migrations;
using FutbotecaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FutbotecaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            var reportes = await _context.Reportes.Select(x => new
            {
                x.Id,
                x.Motivo,
                x.ComentarioId,
                x.UsuarioReportadoId,
                x.UsuarioReportaId,
            }).ToListAsync();

            return Ok(reportes);

        }
        [HttpPost]
        public async Task<IActionResult> Crear(CreateReporteDTO dto)
        {
            var reporte = new Reportes
            {
                Motivo = dto.Motivo,
                ComentarioId = dto.ComentarioId,
                UsuarioReportadoId = dto.UsuarioReportadoId,
                UsuarioReportaId = dto.UsuarioReportaId
            };

            _context.Reportes.Add(reporte);

            await _context.SaveChangesAsync();
            return Ok(reporte);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);

            if (reporte==null)
            {
                return NotFound(new {message="No se ha encontrado el reporte"});
            }
            _context.Reportes.Remove(reporte);
            await _context.SaveChangesAsync();

            return Ok(reporte);
        }
    }
}
      
