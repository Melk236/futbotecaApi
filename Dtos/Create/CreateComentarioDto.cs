namespace FutbotecaApi.Dtos.Create
{
    public class CreateComentarioDto
    {
        public required string Contenido { get; set; }
        public int UsuarioId { get; set; }
        public int VideoId { get; set; }
    }
}
