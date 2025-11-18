namespace FutbotecaApi.Dtos.Create
{
    public class CreateNotificacionDto
    {
        public string tipo { get; set; }
        public int UsuarioRecibeId { get; set; }
        public int UsuarioEnviaId { get; set; }
        public int? VideoId { get; set; }
    }
}
