namespace FutbotecaApi.Dtos.Update
{
    public class UpdateVideoDto
    {
        public required string Titulo { get; set; }
        public required string Url { get; set; }
        public required string Descripcion { get; set; }
        public int Vistas { get; set; }
        public string Categoria { get; set; }
        public int UsuarioId { get; set; }
    }
}
