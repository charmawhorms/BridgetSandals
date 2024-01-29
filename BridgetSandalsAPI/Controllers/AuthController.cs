using BridgetSandalsAPI.Models;
using BridgetSandalsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BridgetSandalsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await authService.RegisterUser(user))
            {
                return Ok(new { status = "success", message = "Registeration Successful" });
            }

            return Ok(new { status = "fail", message = "Registeration Failed" });

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            var result = await authService.Login(user);

            if (result == true)
            {
                var token = authService.GenerateToken(user);
                return Ok(new { status = "success", message = "Login Successful", data = token });
            }
            return Ok(new { status = "fail", message = "Login Failed" });

        }
    }
}
