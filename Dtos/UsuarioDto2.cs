using FutbotecaApi.Migrations;

namespace FutbotecaApi.Dtos
{
    public class UsuarioDto2
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Avatar { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? Descripcion { get; set; }
    }
}
