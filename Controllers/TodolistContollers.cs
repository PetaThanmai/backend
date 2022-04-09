// 
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todoapp.DTOs;
using Todoapp.Models;
using Todoapp.Repositories;
using TodolistTask.Repositories;

namespace TodoTask.Controllers;

[ApiController]
[Route("api/todo")]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodolistRepository _Todo;

    public TodoController(ILogger<TodoController> logger,
    ITodolistRepository Todo)
    {
        _logger = logger;
        _Todo = Todo;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Todolist>>> GetAllTodos()
    {
        var TodosList = await _Todo.GetList();

        // Todo -> Todolist
        // var dtoList = TodosList.Select(x => x.asDto);

        return Ok(TodosList);
    }

    [HttpGet("getmytodo")]
    [Authorize]

    public async Task<ActionResult<List<Todolist>>> GetMyTodo()
    {
        var id = GetCurrentUserId();
        var Todo = await _Todo.GetMyTodolist(Convert.ToInt32(id));
        if (Todo is null)
            return NotFound("No Todo found with given todoid");

        // var dto = Todo.asDto;
        return Ok(Todo);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Todolist>> GetTodoById([FromRoute] long id)
    {
        var Todo = await _Todo.GetById(id);

        if (Todo is null)
            return NotFound("No Todo found with given todoid");

        var dto = Todo.asDto;

        return Ok(dto);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Todolist>> CreateTodo([FromBody] CreateTodolistDTO Data)
    {
        var id = GetCurrentUserId();
        var toCreateTodo = new Todolist
        {
            UserId = Int32.Parse(id),
            Decsription = Data.Decsription,
            IsCompleted = Data.IsCompleted,
        };

        var createdTodo = await _Todo.Create(toCreateTodo);

        return StatusCode(StatusCodes.Status201Created, createdTodo.asDto);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateTodo(long id,
    [FromBody] TodolistUpdateDTO Data)
    {
        var existing = await _Todo.GetById(id);
        var currentUserId = GetCurrentUserId();
        if (Int32.Parse(currentUserId) != existing.UserId)
            return Unauthorized("Your not authorized to update.");
        if (existing is null)
            return NotFound("No Todo found with given todoid");

        var toUpdateTodo = existing with
        {
            IsCompleted = Data.IsCompleted,
        };

        var didUpdate = await _Todo.Update(toUpdateTodo);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update Todo");

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteTodo(long id)
    {

        var todo = await _Todo.GetById(id);
        var currentUserId = GetCurrentUserId();
        if (Int32.Parse(currentUserId) != todo.UserId)
            return Unauthorized("Your not authorized to delete.");

        if (todo == null)
            return NotFound("Todo not found");
        var didDelete = await _Todo.Delete(id);
        if (!didDelete)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete todo");
        return Ok("Deleted");

    }

    private string GetCurrentUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        var userClaims = identity.Claims;

        return (userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

    }

}
