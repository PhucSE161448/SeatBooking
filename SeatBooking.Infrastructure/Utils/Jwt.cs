using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using SeatBooking.Domain.Common;
using SeatBooking.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace SeatBooking.Infrastructure.Utils
{
    public static class Jwt
    {
        public static string GenerateJwtToken(User account)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey secretKey =
                new(Encoding.UTF8.GetBytes(AppConfiguration.JWTSection.SecretKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                new Claim("Id", account.Id.ToString()),
            };
            var expires = DateTime.Now.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: AppConfiguration.JWTSection.ValidIssuer,
                audience: AppConfiguration.JWTSection.ValidAudience, 
                claims: claims, 
                notBefore: DateTime.Now, 
                expires: expires, 
                signingCredentials: credentials);
            return jwtHandler.WriteToken(token);
        }
    }
}
