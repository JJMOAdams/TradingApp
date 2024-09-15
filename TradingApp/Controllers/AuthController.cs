using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TradingApp.Contexts;
using TradingApp.DTOs;
using TradingApp.Entities;
using TradingApp.Interfaces;

namespace TradingApp.Controllers
{
    public class AuthController(ITokenService tokenService, IMemoryCache cache, UserManager<AppUser> userManager) : BaseController
    {
        private readonly IMemoryCache _cache = cache;
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<AppUser> _userManager = userManager;

        private void CreateChallengeCode(AppUser user)
        {
            // challenge code is a random string that the user must provide to login
            // this is to prevent brute force attacks
            // will contain digits and letters of length 10
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[10];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var challengeCode = new string(stringChars);

            _cache.Set(challengeCode, user, TimeSpan.FromMinutes(10));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null) return Unauthorized("Invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return Unauthorized("Invalid password");

            // create a challenge code for the user to store in the cache
            CreateChallengeCode(user);

            // send the challenge code to the user's email

            

            return Ok();
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerifyDto verifyDto)
        {
            var user = await _userManager.FindByNameAsync(verifyDto.Username);
            if (user == null) return Unauthorized("Invalid username");

            // check if the challenge code is correct
            if (!_cache.TryGetValue(verifyDto.ChallengeCode, out AppUser? cacheUser))
            {
                return Unauthorized("Invalid challenge code");
            }

            if (user != cacheUser) return Unauthorized("Invalid challenge code");

            // remove the challenge code from the cache
            _cache.Remove(verifyDto.ChallengeCode);

            return Ok(new UserDto
            {
                Token = _tokenService.CreateToken(user)
            });
        }
    }
}