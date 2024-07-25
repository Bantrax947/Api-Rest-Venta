using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ultima_prueba.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;


namespace RESTAPI_CORE.Controllers
{
    [Route("api/[controller]")]
   
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly string Secretkey;

        public AutenticacionController(IConfiguration config)
        {
            Secretkey = config.GetSection("JWT").GetSection("Secretkey").ToString();
        }


        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] UsuarioCommand request)
        {

            if (request.correo == "bantrax654@.com" && request.clave == "123456")
            {

                var keyBytes = Encoding.ASCII.GetBytes(Secretkey);
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.correo));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokencreado = tokenHandler.WriteToken(tokenConfig);


                return StatusCode(StatusCodes.Status200OK, new { token = tokencreado });

            }
            else
            {

                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }



        }



    }
}