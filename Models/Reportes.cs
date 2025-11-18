using System.ComponentModel.DataAnnotations.Schema;

namespace FutbotecaApi.Models
{
    public class Reportes
    {
        public int Id { get; set; }
        public string Motivo { get; set; }
        // Claves foráneas
        public int UsuarioReportadoId { get; set; }

        [ForeignKey("UsuarioReportadoId")]
        public Usuario UsuarioReportado { get; set; }

        public int UsuarioReportaId { get; set; }

        [ForeignKey("UsuarioReportaId")]
        public Usuario UsuarioReporta { get; set; }

        public int ComentarioId { get; set; }

        [ForeignKey("ComentarioId")]
        public Comentario Comentario { get; set; }
    }
}