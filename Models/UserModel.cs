// using Postdb.DTOs;
// using SCHOOL.DTOs;

using Todoapp.DTOs;

namespace Todoapp.Models;
public record User
{
    public long UserId { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public UserDTO asDto => new UserDTO
    {
        UserId = UserId,
        Username = Username,
        Email = Email,
        Password = Password,

    };

}
