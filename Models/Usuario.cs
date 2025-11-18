using System.Text.Json.Serialization;

namespace FutbotecaApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Avatar { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        
        public ICollection<Video> Videos { get; set; } //El usuario puede tener varios videos.
       
        public ICollection<Comentario> comentarios { get; set; }
        
        public ICollection<Like> likes { get; set; }

        // Relación de seguimiento
        
        public ICollection<Seguimiento> Siguiendo { get; set; }     // A quién sigo yo
        
        public ICollection<Seguimiento> Seguidores { get; set; }    // Quién me sigue

    }
}
