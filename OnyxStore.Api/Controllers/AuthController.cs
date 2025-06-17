using Microsoft.AspNetCore.Mvc;
using OnyxStore.Api.Dtos;
using OnyxStore.Api.Interfaces;

namespace OnyxStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] PostUserDto dto)
        {
            var result = await authService.RegisterAsync(dto);
            if (result.Status == AuthStatusResult.Success)
                return Ok(new { Token = result.Token });
            
            return BadRequest($"Registration failed {result.Status.ToString()}");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var result = await authService.LoginAsync(dto);
            if (result.Status == AuthStatusResult.Success)
                return Ok(new { Token = result.Token });
            
            return BadRequest($"Login failed {result.Status.ToString()}");
        }
    }
}
