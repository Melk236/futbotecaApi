namespace FutbotecaApi.Dtos.Create
{
    public class CreateUsuarioDto
    {
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Avatar { get; set; } // Opcional: Si usas avatar

    }
}
