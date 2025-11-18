namespace FutbotecaApi.Dtos
{
    public class ComentarioDto_
    {
        public int Id { get; set; }

        public required string Contenido { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int UsuarioId { get; set; }
        public int VideoId { get; set; }
    }
}
