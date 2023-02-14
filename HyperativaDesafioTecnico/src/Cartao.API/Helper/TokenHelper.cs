using Microsoft.IdentityModel.Tokens;
using static Dapper.SqlMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HyperativaDesafio.Domain.Entities;
using HyperativaDesafio.Domain.Services;

namespace HyperativaDesafio.API.Helper
{
    public static class TokenHelper
    {

        public static string GetToken(Usuario usuario)
        {
            var handler = new JwtSecurityTokenHandler();
            var secretApikey = Encoding.ASCII.GetBytes(SecurityService.apiKey);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.nome.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretApikey)
                        , SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(tokenConfig);
            return handler.WriteToken(token);
        }
    }


}

