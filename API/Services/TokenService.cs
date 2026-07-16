using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser user)
        {
            var tokenkey = config["TokenKey"] ?? throw new Exception("La chiave del Token deve essere lunga almeno 64 caratteri"); // Retrieve the token key from configuration
            if(tokenkey.Length < 64) 
            {
                throw new Exception("La chiave del Token deve essere lunga almeno 64 caratteri");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey)); // Create a symmetric security key using the token key
            var claims = new List<Claim> // Create a list of claims for the token
            {
                new Claim(ClaimTypes.Email, user.Email), // Add the user's email as a claim
                new Claim(ClaimTypes.NameIdentifier, user.Id), // Add the user's ID as a claim
            };
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // Create signing credentials using the key and HMAC SHA-512 algorithm
            var tokenDescriptor = new SecurityTokenDescriptor // Create a token descriptor with claims, expiration, and signing credentials
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), // Set the token to expire in 7 days
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler(); // Create a token handler to generate the token
            var token = tokenHandler.CreateToken(tokenDescriptor); // Generate the token using the token descriptor
            return tokenHandler.WriteToken(token); // Return the serialized token as a string
        }
    }
}