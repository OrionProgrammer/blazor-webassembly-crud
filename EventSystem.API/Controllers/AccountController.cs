using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using EventSystem.Domain;
using EventSystem.Model;
using System.Data;

namespace EventSystem.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(string.IsNullOrWhiteSpace(userModel.Password))
            {
                ModelState.AddModelError(string.Empty, "Password cannot be empty");
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = userModel.Email,
                Email = userModel.Email,
                FirstName = userModel.Name,
                LastName = userModel.Surname
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            RegisterResponseModel registerResponseModel = new();

            if (result.Succeeded)
            {
                //add role
                await _userManager.AddToRoleAsync(user, userModel.Role);

                registerResponseModel.IsSuccess = true;
                registerResponseModel.Message = "Registration successful!";

                return Ok(registerResponseModel);
            }

            registerResponseModel.IsSuccess = false;
            registerResponseModel.Message = "Registration unsuccessful!";

            return BadRequest(registerResponseModel);
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);

            //create LoginResponseModel to use for either success of failed login
            LoginResponseModel loginResponseModel = new();

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                var userRoles = await _userManager.GetRolesAsync(user);
                var role = userRoles.Select(u => u).First(); //i am just allowing 1 role for this application.
                var token = GenerateJwtToken(user, role);
                var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(30);

                loginResponseModel.UserId = user.Id;
                loginResponseModel.FullName = user.FirstName + " " + user.LastName;
                loginResponseModel.Token = token;
                loginResponseModel.IsSuccess = true;
                loginResponseModel.Role = role;
                loginResponseModel.ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds;

                return Ok(loginResponseModel);
            }

            loginResponseModel.IsSuccess = false;

            if (result.IsLockedOut)
            {
                loginResponseModel.Message = "Account is locked out.";
                return Unauthorized(loginResponseModel);
            }

            loginResponseModel.Message = "Invalid login attempt.";
            return Unauthorized(loginResponseModel);
        }

        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Unauthorized(new { Message = "User not found" });

            var profile = new UserModel
            {
                Email = user.Email,
                Name = user.FirstName,
                Surname = user.LastName
            };

            return Ok(profile);
        }

        private string GenerateJwtToken(ApplicationUser user, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:Jwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
