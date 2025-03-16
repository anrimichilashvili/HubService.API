using HubService.Application.Dtos;
using HubService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace HubService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserModelDto user)
        {
            var result = await _authService.RegisterUser(user);
            if (result.Success)
                return Ok(result.Data);
            return BadRequest(result.Data);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login(UserModelDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _authService.Login(user);
            if (result.Success)
            {
                var tokenString = _authService.GenerateToeknString(user);
                return Ok(tokenString);
            }
            return BadRequest();
        }
    }
}
