
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessaginApp.API.Data;
using MessaginApp.API.Dtos;
using MessaginApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace MessaginApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo =repo;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegister userForRegister ){
            //validate input
            userForRegister.Username=userForRegister.Username.ToLower();

            if(await _repo.UserExists(userForRegister.Username))
            return BadRequest("username already exists");

            var userToCreate=new User(){
                username=userForRegister.Username 
            };

            var cereatedUsed =_repo.Register(userToCreate ,userForRegister.Password);

            return StatusCode(201);
        }


    }
}