using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using sistema_saude.Data;
using sistema_saude.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace sistema_saude.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _context;

        public AuthService(IConfiguration configuration, MyDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string Authenticate(LoginModel login)
        {
            // Busca o usuário no banco de dados
            var user = _context.Usuario.SingleOrDefault(u => u.Email == login.Email);

            // Verifica se o usuário existe e se a senha está correta
            if (user == null || user.Senha != login.Senha)
            {
                return null; // Usuário não encontrado ou senha incorreta
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Tipo.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Permissoes", user.Permissoes)
                }),

                Expires = DateTime.UtcNow.AddHours(_configuration.GetValue<int>("Jwt:ExpireHours")),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}