using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FutbotecaApi.Models
{
    public class Video
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Url { get; set; }
        public required string Descripcion { get; set; }
        public int Vistas { get; set; } = 0;
        public DateTime FechaSubida { get; set; } = DateTime.Now;
        public string Categoria { get; set; }
        public int UsuarioId { get; set; }
        
        public Usuario Usuario { get; set; }

        //Relaciones de 1 a muchos(1 video puede tener varios likes y comentarios)
        
        public ICollection<Like> likes { get; set; }
       

        public ICollection<Comentario> comentarios { get; set; }


    }
}
