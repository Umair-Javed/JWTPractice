using JWTPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace JWTPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("TestMe")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestMe()
        {
            return Ok("You are authenticated");
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            var user = Authenticate(loginModel);

            ArgumentNullException.ThrowIfNull(user);

            var token = Generate(user);
            return Ok(token);
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private UserModel Authenticate(LoginModel loginModel)
        {
            var user = UserModel.GetUserModels().SingleOrDefault(x => x.Username == loginModel.UserName && x.Password == loginModel.Password);
            
            if(user != null)
                return user;

            return null;
        }

    }
}
