using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LibrarySystemWebAPI.Dtos.AccountDtos;
using LibrarySystemWebAPI.Entities;
using LibrarySystemWebAPI.Results;
using LibrarySystemWebAPI.Services;

namespace LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            UserRegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(
                user,
                request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            UserLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(
                request.Email);

            if (user is null)
                return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                false);

            if (!result.Succeeded)
                return Unauthorized();

            var token = _jwtService.GenerateToken(user);

            var response = GenericResult<string>.Success(token);

            return Ok(response);
        }
    }
}