namespace FutbotecaApi.Dtos.Create
{
    public class CreateVideoDto
    {
        public required string Titulo { get; set; }
        public required string Url { get; set; }
        public required string Descripcion { get; set; }
        public required int UsuarioId { get; set; }
        public required string Categoria { get; set; }
    }
}
