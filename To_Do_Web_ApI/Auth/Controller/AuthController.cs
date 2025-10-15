using Microsoft.AspNetCore.Mvc;
using To_Do_Web_ApI.Model.Dto;
using To_Do_Web_ApI.Users.Service;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService userService;
    
    public AuthController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserDto user)
    {
        var existingUser = await this.userService.FindUserByUsernameAsync(user);
        
        return existingUser is null?BadRequest("User already exists"):Ok("User registered!");
        
    }

}