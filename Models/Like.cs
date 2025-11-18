using System.Text.Json.Serialization;

namespace FutbotecaApi.Models
{
    public class Like
    {
        public int Id { get; set; }
        
        public DateTime Fecha { get; set; } = DateTime.Now;

        // Relaciones
        public int UsuarioId { get; set; }
        
        public Usuario Usuario { get; set; }

        public int VideoId { get; set; }
        
        public Video Video { get; set; }
    }
}
