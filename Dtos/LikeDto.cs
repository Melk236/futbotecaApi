namespace FutbotecaApi.Dtos
{
    public class LikeDto
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public int UsuarioId { get; set; }
        public int VideoId { get; set; }
    }
}
