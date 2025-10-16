using Microsoft.AspNetCore.Mvc;
using To_Do_Web_ApI.Auth.Service;
using To_Do_Web_ApI.Model.Dto;
using To_Do_Web_ApI.Users.Service;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly AuthService _authService;
    
    public AuthController(UserService userService, AuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto userDto)
    {
        var existingUser = await _userService.FindUserByUsernameAsync(userDto.username);
        if (existingUser is not null)
        {
            return BadRequest("Username already exists");
        }
        
        string username = await _userService.CreateUserAsync(userDto);

        return Created("Registered user",new{username});
    }

    [HttpPost("login")]
    
    public async Task<IActionResult> LogIn([FromBody] CreateUserDto userDto)
    {
        var authenticatedUser = await _userService.AuthenticateUserAsync(userDto);

        if (authenticatedUser is null)
            return Unauthorized("Invalid username or password");

        string token = this._authService.GenerateJwtToken(authenticatedUser);
        return Ok(authenticatedUser);
    }

}