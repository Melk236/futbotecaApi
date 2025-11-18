namespace FutbotecaApi.Dtos
{
    public class VideoDto
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Url { get; set; }
        public required string Descripcion { get; set; }
        public int Vistas { get; set; } 
        public DateTime FechaSubida { get; set; } = DateTime.Now;
        public string Categoria { get; set; }
        public int UsuarioId { get; set; }
    }
}
