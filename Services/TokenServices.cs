using API.NETCore3._1.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.NETCore3._1.Services
{
    public class TokenServices
    {
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler(); //criar um token handler -> responsavel por gerar
            var key = Encoding.ASCII.GetBytes(Settings.Secret); //chave 
            var tokenDescriptor = new SecurityTokenDescriptor // descrição do que vai ter neste token
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Username.ToString()),
                    new Claim(ClaimTypes.Role,user.Role.ToString()),

                }),
                Expires = DateTime.UtcNow.AddHours(2),//expiração(2horas)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256) //criptografando a credencial
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); //criando meu token;

            return tokenHandler.WriteToken(token);//transformando  
        }


    }
}
