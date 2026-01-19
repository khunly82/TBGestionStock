using GestionStock.API.Dto;
using GestionStock.API.Services;
using GestionStock.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace GestionStock.API.Controllers
{
    [ApiController]
    public class UserController(UserService userService, JwtManager jwtManager) : ControllerBase
    {
        [HttpPost("api/login")]
        public IActionResult Login([FromBody]LoginDto dto)
        {
            try
            {
                User u = userService.Login(dto.Username, dto.Password);
                string token = jwtManager.GenerateToken(u.Username, u.Roles);
                return Ok(new TokenDto(token));
            }
            catch(AuthenticationException)
            {
                return Unauthorized();
            }
        }
    }
}
