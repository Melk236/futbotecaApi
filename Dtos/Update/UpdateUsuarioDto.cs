namespace FutbotecaApi.Dtos.Update
{
    public class UpdateUsuarioDto
    {   
        public string? NombreUsuario { get; set; }
        public IFormFile? Avatar { get; set; }
        public string Descripcion { get; set; }
    }
}
