using BookStoreRest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace BookStoreRest.Controllers
{
    [Route("/api/{controller}")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly DatabaseContext _context;
        public AuthController(DatabaseContext context) {
            _context = context;
        }
        [HttpHead]
        public ActionResult Head() {
            return Ok();
        }
        [HttpPost("register")]
        public ActionResult<User> RegisterUser(UserDto _user) {
            if (!_context.Users.Any(user => user.Login == _user.Login)) {
                User user = UserDto.DtoToUser(_user);
                _context.Users.Add(user);
                _context.SaveChanges();
                return Created(nameof(RegisterUser), new { id = user.Id, });
            }
            else return Conflict("User with this login already exists");
        }
        [HttpPost("login")]
        public ActionResult<User> LoginUser(UserDto _user) {
            User? u = _context.Users.FirstOrDefault(user => user.Login == _user.Login);
            if (u is null) return Unauthorized("Ivalid login");
            if (!UserDto.VerifyPassword(_user.Password, u.PasswordHash)) return Unauthorized("Ivalid password");

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
                new Claim(ClaimTypes.Name, u.Login),
            };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(new { Token = token });
        }
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me() { 
            return Ok(new {
                username = User.Identity?.Name
            });
        }
    }
}
