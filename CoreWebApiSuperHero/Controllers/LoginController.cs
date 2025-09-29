using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CoreWebApiSuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _IConfiguration;

        public LoginController(IConfiguration configuration)
        {
            _IConfiguration = configuration;
        }
        
        [HttpPost]
        public ActionResult Login(LoginDTO loginDTO)
        {
            LoginResponseDTO responseDTO = new LoginResponseDTO() { userName = loginDTO.userName };

            if (!ModelState.IsValid)
            {
                return BadRequest(" Please provide user name and password .");
            }
            string localIssuer = "";
            string localAudience = "";
            byte[] Key = null;
            if (loginDTO.userName == "prachi" && loginDTO.password == "Sarvada" )//&& loginDTO.userRole == "Admin"
            {
                if (loginDTO.policy == "Local")
                {
                     Key = Encoding.ASCII.GetBytes(_IConfiguration.GetValue<string>("JWTSecretKey"));

                    localIssuer = _IConfiguration.GetValue<string>("LocalIssuer");
                    localAudience = _IConfiguration.GetValue<string>("LocalAudience");
                }
                else if (loginDTO.policy == "Google")
                {
                     Key = Encoding.ASCII.GetBytes(_IConfiguration.GetValue<string>("GoogleJWTSecretKey"));

                    localIssuer = _IConfiguration.GetValue<string>("GoogleIssuer");
                    localAudience = _IConfiguration.GetValue<string>("GoogleAudience");
                }                
                
                var tokenDescriptor = new SecurityTokenDescriptor() //// create security token descriptor
                {
                    Issuer = localIssuer,
                    Audience = localAudience,
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, loginDTO.userName),
                        //new Claim(ClaimTypes.Role,"Admin") //Role check
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256)
                };
                var tokenHandler = new JwtSecurityTokenHandler(); // create token handler
                var token = tokenHandler.CreateToken(tokenDescriptor);
                responseDTO.jwtToken = tokenHandler.WriteToken(token);                
            }
            else 
            {
                return Ok("Invalid user name and password");
            }
            return Ok(responseDTO);
        }
    }
}
