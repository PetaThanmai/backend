using Dapper;
using Todoapp.Models;
using Todoapp.Utilities;

namespace Todoapp.Repositories;
public interface IUserRepository
{
    Task<User> Create(User Item);
    Task<bool> Update(User item);
    // Task<bool> Delete(long UserId);
    Task<User> GetById(long UserId);
    Task<List<User>> GetList();
    Task<User> GetByUsername(string UserName);
    //  Task<List<User>> GetListByGuestId(long GuestId);

    // Task<List<User>> GetUserByRoomId(long User);

}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }
    public async Task<User> Create(User item)
    {


        var query = $@"INSERT INTO ""{TableNames.users}""
        (user_id,user_name,email,password)
        VALUES (@UserId,  @UserName, @Email, @Password) RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<User>(query, item);

            return res;
        }

    }


    public async Task<User> GetById(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" WHERE user_id = @UserId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { UserId });


    }
    public async Task<User> GetByUsername(string UserName)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" WHERE user_name = @UserName";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { UserName });
    }



    public async Task<List<User>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""";
        List<User> res;
        using (var con = NewConnection)
            res = (await con.QueryAsync<User>(query)).AsList();
        return res;
    }



    // public async  Task<List<User>> GetListByGuestId(long GuestId)
    // {
    //      var query = $@"SELECT * FROM {TableNames.guest} 
    //     WHERE guest_id = @GuestId";

    //     using (var con = NewConnection)
    //         return (await con.QueryAsync<User>(query, new { GuestId })).AsList();
    // }    


    // public async Task<List<User>> GetUserByRoomId(long RoomId)
    // {
    //     var query=$@"SELECT *FROM ""{TableNames.room}""

    //    WHERE room_id = @RoomId"; 

    //    using (var con=NewConnection){

    //        var res=(await con.QueryAsync<User>(query, new{RoomId})).AsList();
    //        return res;
    //    }
    // }

    // public Task<List<UserDTO>> GetList(object UserId)
    // {

    //     return null;
    // }

    public async Task<bool> Update(User item)
    {
        var query = $@"UPDATE ""{TableNames.users}"" SET check_in=@CheckIn,check_out=@CheckOut
         WHERE User_id = @UserId";

        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, item);
            return rowCount == 1;
        }
    }
}

