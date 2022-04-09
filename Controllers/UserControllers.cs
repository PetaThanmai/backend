using Todoapp.DTOs;
using Todoapp.Models;
using Todoapp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Todoapp.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _user;
    private readonly IConfiguration _config;


    public UserController(ILogger<UserController> logger, IUserRepository User, IConfiguration config)

    {
        _logger = logger;
        _user = User;
        _config = config;


    }
    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetList()
    {
        var res = await _user.GetList();
        return Ok(res.Select(x => x.asDto));
    }



    [HttpGet("{user_id}")]
    [Authorize]

    public async Task<ActionResult> GetById([FromRoute] long user_id)
    {
        var res = await _user.GetById(user_id);
        if (res == null)
            return NotFound("No Product found with given employee number");
        var dto = res.asDto;
        // dto.Schedules = (await _schedule.GetListByUsersId(Users_id))
        //                 .Select(x => x.asDto).ToList();
        // dto.Rooms = (await _room.GetListByUsersId(Users_id)).Select(x => x.asDto).ToList();

        return Ok(dto);
    }

    [HttpPost("{user}")]
    [Authorize]

    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUsersDTO Data)
    {
        // if (!(new string[] { "male", "female" }.Contains(Data.Gender.Trim().ToLower())))
        // return BadRequest("Gender value is not recognized");

        // var subtractDate = DateTimeOffset.Now - Data.DateOfBirth;
        // if (subtractDate.TotalDays / 365 < 18.0)
        // return BadRequest("Employee must be at least 18 years old");/

        var toCreateUser = new User
        {

            UserId = Data.UserId,
            Username = Data.Username,
            Email = Data.Email,
            Password = Data.Password
        };
        var createdUser = await _user.Create(toCreateUser);

        return StatusCode(StatusCodes.Status201Created, createdUser.asDto);


    }

    [HttpPut("{user_id}")]
    [Authorize]
    public async Task<ActionResult> UpdateUser([FromRoute] long user_id,
    [FromBody] UserUpdateDTO Data)
    {
        var existing = await _user.GetById(user_id);
        if (existing is null)
            return NotFound("No Product found with given customer number");

        var toUpdateUser = existing with
        {
            // Email = Data.Email?.Trim()?.ToLower() ?? existing.Email,
            // LastName = Data.LastName?.Trim() ?? existing.LastName,
            // Mobile = Data.Mobile ?? existing.Mobile,
            // DateOfBirth = existing.DateOfBirth.UtcDateTime,
        };

        var didUpdate = await _user.Update(toUpdateUser);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update");
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [Authorize]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
    {
        var user = await Authenticate(userLogin);
        if (user != null)
        {
            var token = Generate(user);
            return Ok(token);

        }
        return NotFound("User not found");
    }

    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new  Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new  Claim(ClaimTypes.Name,user.Username),
                new  Claim(ClaimTypes.Email,user.Email),
                new  Claim(ClaimTypes.GivenName,user.Password),
                // new  Claim(ClaimTypes.Surname,user.Surname),
                // new  Claim(ClaimTypes.Role,user.Role),

            };
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Audience"],
         claims,
         expires: DateTime.Now.AddMinutes(15),
         signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    [HttpPost]
    [Route("login")]
    [Authorize]
    private async Task<User> Authenticate(UserLogin userLogin)
    {
        // var currentUser = UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() ==
        //  userLogin.Username.ToLower() && o.Password == userLogin.Password);

        var currentUser = await _user.GetByUsername(userLogin.Username);
        if (currentUser == null)
        {
            return null;
        }
        if (currentUser.Password != userLogin.Password)
        {
            return null;
        }
        return currentUser;
    }

}


