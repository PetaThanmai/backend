using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Todoapp.DTOs;
public record TodolistDTO
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }
    [JsonPropertyName("decsription")]
    public string Decsription { get; set; }
    [JsonPropertyName("is_completed")]
    public bool IsCompleted { get; set; }

}

public record CreateTodolistDTO

{

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }
    [JsonPropertyName("decsription")]
    public string Decsription { get; set; }
    [JsonPropertyName("is_completed")]
    public bool IsCompleted { get; set; }
}

public record TodolistUpdateDTO
{
    [JsonPropertyName("decsription")]
    public string Decsription { get; set; }
    [JsonPropertyName("is_completed")]
    public bool IsCompleted { get; set; }
}
