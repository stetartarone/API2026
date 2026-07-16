using System;
using Api.Data;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using Api.Extensions;

namespace Api.Controllers
{
    public class AccountController(AppDbContext context, ITokenService tokenService ) : BaseApiController
    {
        [HttpPost("register")] //api/account/register
        // This method handles user registration by creating a new AppUser entity, hashing the password, and saving it to the database. It returns a UserDto with a generated token upon successful registration.
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await EmailExists(registerDto.Email))
            {
                return BadRequest("Email è presente");
            }
            using var hmac = new HMACSHA512();
            // Create a new AppUser entity with the provided registration details and hashed password
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            // Return a UserDto with a generated token for the newly registered user
            return user.ToDto(tokenService);
        }
        [HttpPost("login")] //api/account/login
        // This method handles user login by validating the provided email and password. If the credentials are valid, it returns a UserDto with a generated token.
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Retrieve the user from the database based on the provided email
            var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Email non valida");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            // Compare the computed hash with the stored password hash to validate the password
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Password non valida");
                }
            }
            return user.ToDto(tokenService);
        }
        private async Task<bool> EmailExists(string email)
        {
            return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }
    }
}