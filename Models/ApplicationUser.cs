using Microsoft.AspNetCore.Identity;

namespace FutbotecaApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Avatar { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
