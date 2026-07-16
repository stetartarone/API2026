using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace Api.Extensions;

public static class AppUserExtensions
{
    // Extension method to convert AppUser to UserDto
    public static UserDto ToDto(this AppUser user, ITokenService tokenService)
    {
        var token = tokenService.CreateToken(user);
        return new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = token
        };
    }
}