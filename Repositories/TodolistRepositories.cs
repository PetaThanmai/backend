// 
using Dapper;
using Todoapp.DTOs;
using Todoapp.Models;
using Todoapp.Repositories;
using Todoapp.Utilities;

namespace TodolistTask.Repositories;


public interface ITodolistRepository
{
    Task<Todolist> Create(Todolist Item);
    Task<bool> Update(Todolist Item);
    Task<bool> Delete(long Id);
    Task<Todolist> GetById(long Id);
    Task<List<Todolist>> GetList();
    Task<List<Todolist>> GetMyTodolist(long UserId);



}
public class TodolistRepository : BaseRepository, ITodolistRepository
{
    public TodolistRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Todolist> Create(Todolist item)
    {
        var query = $@"INSERT INTO ""{TableNames.todolist}""
        (user_id, decsription, is_completed)
        VALUES (@UserId, @Decsription, @IsCompleted) RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Todolist>(query, item);
            return res;
        }


    }


    public async Task<bool> Delete(long Id)
    {
        var query = $@"DELETE FROM ""{TableNames.todolist}"" 
        WHERE id = @Id";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { Id });
            return res > 0;
        }
    }

    public async Task<Todolist> GetById(long Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.todolist}""
        WHERE id = @Id";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Todolist>(query,
            new
            {
                Id = Id
            });
        // User can have more than one todo
    }

    public async Task<List<Todolist>> GetList()
    {
        // Query
        var query = $@"SELECT * FROM ""{TableNames.todolist}""";

        List<Todolist> res;
        using (var con = NewConnection) // Open connection
            res = (await con.QueryAsync<Todolist>(query)).AsList(); // Execute the query
        // Close the connection

        // Return the result
        return res;
    }



    public async Task<List<Todolist>> GetMyTodolist(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.todolist}"" WHERE user_id = @UserId;";
        using (var con = NewConnection)
            return (await con.QueryAsync<Todolist>(query, new { UserId })).ToList();
    }



    public async Task<bool> Update(Todolist Item)
    {
        var query = $@"UPDATE ""{TableNames.todolist}"" SET   decsription = @Decsription , is_completed = @IsCompleted 
        WHERE id = @Id";

        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);
            return rowCount == 1;
        }
    }
}

