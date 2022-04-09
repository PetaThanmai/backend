using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Todoapp.DTOs;
public record UserDTO
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }


    [JsonPropertyName("user_name")]
    public string Username { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }

}

public record CreateUsersDTO

{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }


    [JsonPropertyName("user_name")]
    public string Username { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}

public record UserUpdateDTO
{

    [JsonPropertyName("user_name")]
    public string Username { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }


}
