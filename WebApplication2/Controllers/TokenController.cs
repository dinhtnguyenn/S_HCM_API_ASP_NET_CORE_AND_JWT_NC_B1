using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly dinhntco_studywithmeContext db;

        public TokenController(IConfiguration config, dinhntco_studywithmeContext context)
        {
            _configuration = config;
            db = context;
        }

        //Đăng nhập để lấy token
        //[HttpPost]
        //public async Task<IActionResult> Post(Account _userData)
        //{

        //    if (_userData != null && _userData.Email != null && _userData.Mssv != null)
        //    {
        //        var user = await GetUser(_userData.Email, _userData.Mssv);

        //        if (user != null)
        //        {
        //            //create claims details based on the user information
        //            var claims = new[] {
        //            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
        //            new Claim("MSSV", user.Mssv.ToString()),
        //            new Claim("FullName", user.FullName),
        //            new Claim("Email", user.Email)
        //           };

        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        //            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddDays(1), signingCredentials: signIn);

        //            return Ok(new Token(new JwtSecurityTokenHandler().WriteToken(token)));
        //        }
        //        else
        //        {
        //            return BadRequest("Invalid credentials");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        //private async Task<Account> GetUser(string email, string mssv)
        //{
        //    return await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email && u.Mssv == mssv);
        //}
        //end - Đăng nhập để lấy token

        [HttpGet]
        public IActionResult GetToken()
        {
            //create claims details based on the user information
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim("ID", "1234567890"),
                    new Claim("Email", "dinhnt.hutech@gmail.com")
                   };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddDays(1), signingCredentials: signIn);


            return Ok(new Token(new JwtSecurityTokenHandler().WriteToken(token)));
        }
    }
}
