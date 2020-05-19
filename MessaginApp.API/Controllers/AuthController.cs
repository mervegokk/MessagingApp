
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessaginApp.API.Data;
using MessaginApp.API.Dtos;
using MessaginApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace MessaginApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config= config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegister userForRegister)
        {
            //validate input ,, [apicontroller] olduğu için aşağıdaki kontrole gerek yok
            if (!ModelState.IsValid)
            {
                return BadRequest("girmiş olduğunuz şifre veya username hatalı");

            }



            userForRegister.Username = userForRegister.Username.ToLower();

            if (await _repo.UserExists(userForRegister.Username))
                return BadRequest("username already exists");

            var userToCreate = new User()
            {
                username = userForRegister.Username
            };

            var cereatedUsed = _repo.Register(userToCreate, userForRegister.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDtos userForLogin)
        {
            var userFromRepo = await _repo.Login(userForLogin.Username, userForLogin.Password);
            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.id.ToString()),
                new Claim(ClaimTypes.Name , userFromRepo.username)};

  
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds= new SigningCredentials(key,SecurityAlgorithms.HmacSha512);

            var tokenDescriptor= new SecurityTokenDescriptor(){
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),//1 gün 
                SigningCredentials=creds
                };

            var tokenHandler= new JwtSecurityTokenHandler();    
            var token=tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {
                token=tokenHandler.WriteToken(token)
                });
        }
    }
        
}