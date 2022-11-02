using authentication.Models;

using Jose;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class JwtController : ControllerBase
    {
        public IConfiguration _configuration;
        private object user1;
        public readonly JwtContext _context;
        public JwtController(IConfiguration configuration, JwtContext context)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Post(UserModel user)
        {
            if (user != null && user.UserName != null && user.Password != null)
            {
                var user1 = await GetUser(user.UserName, user.Password);
                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        //new Claim("Id", user.UserId.ToString()),
                        new Claim("UserName", user1.UserName),
                        new Claim("Password", user1.Password)

                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                       jwt.Issuer,
                       jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signIn
                    );
                    TokenModel model = new TokenModel();
                    model.JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }


            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }
        [HttpGet]
        private async Task<UserTb> GetUser(string username, string password)
        {
            return await _context.UserTbs.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
    }
}
//user