// using Postdb.DTOs;
// using SCHOOL.DTOs;

using Todoapp.DTOs;

namespace Todoapp.Models;
public record Todolist
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Decsription { get; set; }

    public bool IsCompleted { get; set; }

    public TodolistDTO asDto => new TodolistDTO
    {
        Id = Id,
        UserId = UserId,
        Decsription = Decsription,
        IsCompleted = IsCompleted,


    };

}
