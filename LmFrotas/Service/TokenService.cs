using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LmFrotas.Models;
using LmFrotas.Service.IService;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LmFrotas.Service {
    public class TokenService : ITokenService {
        public readonly IConfiguration _configuration;
        public TokenService (IConfiguration configuration) {
            _configuration = configuration;
        }

        public User GenerateToken(User user) 
        {
            if(user == null)
                throw new ArgumentException("Login inválido");

            if(user.Password.Length <= 0 || user.Username.Length <= 0)
                throw new ArgumentException("Login inválido");

            user.Token = CreateToken(user);
            user.Password = null;

            return user;
        }

        private string CreateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKeyJwt")));

                        Console.WriteLine(_configuration.GetValue<string>("SecretKeyJwt"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: "LMFrotas",
                    audience: "LMFrotas",
                    claims: claims,
                    expires: DateTime.Now.AddHours(12),
                    signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}