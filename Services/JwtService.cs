using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FutbotecaApi.Services
{
    public class JwtService
    {       
        //Se usa la interfaz IConfiguration para acceder al appsettings.
         private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerarToken(string nombreUsuario) {

            var claims = new[]//Contiene la información de los usuarios
            {
                new Claim(ClaimTypes.Name,nombreUsuario),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())//Un identificador unico para cada usuario
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));//Se convierte en bytes la firma para luego hashearla
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//Aqui se hashea con esto se validará que la firma es correcta.
            var expiracion = int.Parse(_config["Jwt:ExpireMinutes"]);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiracion),
                signingCredentials: creds
            );
            var tokenHandler = new JwtSecurityTokenHandler();//Para pasrlo bien
            return tokenHandler.WriteToken(token);//En string se pasa
        }
    }
}
