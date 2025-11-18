using FutbotecaApi.Models;

namespace FutbotecaApi.Dtos.Update
{
    public class UpdateComentarioDto
    {
        public required string Contenido { get; set; }
        public int UsuarioId { get; set; }

        public int VideoId { get; set; }

    
    }
}
