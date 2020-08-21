using System;
using LmFrotas.Models;
using LmFrotas.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LmFrotas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public LoginController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<User> Login([FromBody]User user)
        {
            try
            {
                return Ok(_tokenService.GenerateToken(user));
            }
            catch (Exception e)
            {
                Console.WriteLine($"LoginController -> Login: Erro - {e.Message}");
                return this.StatusCode(StatusCodes.Status400BadRequest, "Usuário ou senha invalido");
            }
        }
    }
}
