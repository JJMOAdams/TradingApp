using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TradingApp.Contexts;
using TradingApp.DTOs;
using TradingApp.Entities;
using TradingApp.Interfaces;

namespace TradingApp.Controllers
{
    public class AuthController(IConfiguration config, ITokenService tokenService, IToTPService toTPService, UserManager<AppUser> userManager) : BaseController
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IToTPService _toTPService = toTPService;
        private readonly IConfiguration _config = config;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null) return Unauthorized("Invalid username");

            if (!_toTPService.ValidateTotp(user.ToTPSecret, loginDto.ToTPCode))
            {
                return Unauthorized("Invalid ToTP code");
            };

            return Ok(new
            {
                token = _tokenService.CreateToken(user)
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (registerDto.RegistrationCode != _config["API:registerKey"]) return Unauthorized("Invalid registration code");

            var user = _userManager.FindByNameAsync(registerDto.Username);
            if (user != null) return BadRequest("Username is taken");

            var newUser = new AppUser
            {
                UserName = registerDto.Username,
                ToTPSecret = _toTPService.GenerateSecret()
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!result.Succeeded) return BadRequest("Failed to register");

            return Ok();
        }
    }
}