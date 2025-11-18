namespace FutbotecaApi.Dtos.Create
{
    public class CreateReporteDTO
    {
         public string Motivo { get; set; }
        public int UsuarioReportadoId { get; set; }
        public int UsuarioReportaId { get; set; }   
        public int ComentarioId { get; set; }

    }
}
