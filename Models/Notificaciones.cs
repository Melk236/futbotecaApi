using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbotecaApi.Models
{
    public class Notificaciones
    {
        [Key]
        public int Id { get; set; }
        public string Tipo { get; set; }
        public bool Leida { get; set; } = false;
        public int UsuarioRecibeId { get; set; }
        public int UsuarioEnviaId { get; set; }
        public int? VideoId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        // Propiedades de navegación
        [ForeignKey("UsuarioRecibeId")]
        public Usuario UsuarioRecibe { get; set; }

        [ForeignKey("UsuarioEnviaId")]
        public Usuario UsuarioEnvia { get; set; }
        // Navegación a Video
        [ForeignKey("VideoId")]
        public Video Video { get; set; }
    }
}
