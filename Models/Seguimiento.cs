using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace FutbotecaApi.Models

{
    
    public class Seguimiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
     

         
        public int SeguidorId { get; set; }
        
        public Usuario Seguidor { get; set; }

        public int SeguidoId { get; set; }
       
        public Usuario Seguido { get; set; }

        
    }
}
