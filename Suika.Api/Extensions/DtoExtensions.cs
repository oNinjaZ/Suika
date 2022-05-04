using Suika.Api.Dtos;
using Suika.Api.Models;

namespace Suika.Api.Extensions;
public static class DtoExtensions
{
    public static UserDto AsDto(this User user)
    {
        return new UserDto
        {
            Username = user.Username,
            Email = user.Email
        };
    }

    public static User AsUser(this UserDto userDto)
    {
        return new User
        {
            Username = userDto.Username,
            Email = userDto.Email
        };
    }

}