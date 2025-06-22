using Casino.API.Infrastructure.JWT;
using Casino.API.Infrastructure.Validators;
using Casino.Application.ModelsDTO;
using Casino.Application.Services;
using Casino.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Casino.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<JWTConfig> _options;
        public UserController(IUserService userService, IOptions<JWTConfig> options)
        {
            _userService = userService;
            _options = options;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUsetById(id, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterModel user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }
            var validator = new UserRequestValidator();
            var validationResult = await validator.ValidateAsync(user, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var createdUser = await _userService.CreateUser(user, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Loging([FromBody] UserLoginModel loginModel, CancellationToken cancellationToken)
        {
            if (loginModel == null)
            {
                return BadRequest("Login data is required.");
            }
            var result = await _userService.AuthenticationAsync(loginModel.UserName, loginModel.Password, cancellationToken);

            if (result == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            string role = "User";
            var token = JwtService.GenerateSecurityToken(loginModel.UserName, role, _options);
            return Ok(new { Token = token });
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id:int}/balance")]
        public async Task<IActionResult> GetUserBalance(int id, CancellationToken cancellationToken)
        {
            var balance = await _userService.GetUserBalance(id, cancellationToken);
            if (balance < 0)
            {
                return NotFound("User not found or balance is negative.");
            }
            return Ok(new { Balance = balance });
        }

        [Authorize(Roles = "User")]
        [HttpPost("{id:int}/fill-balance")]
        public async Task<IActionResult> FillBalance(int id, [FromBody] decimal amount, CancellationToken cancellationToken)
        {
            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than zero.");
            }

            var result = await _userService.FillBalance(id, amount, cancellationToken);
            if (!result)
            {
                return NotFound("User not found.");
            }
            return Ok("Balance filled successfully.");
        }

        [Authorize(Roles = "User")]
        [HttpPost("{id:int}/withdraw-balance")]
        public async Task<IActionResult> WithdrawBalance(int id, [FromBody] decimal amount, CancellationToken cancellationToken)
        {
            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than zero.");
            }

            var result = await _userService.WithdrawBalance(id, amount, cancellationToken);
            if (!result)
            {
                return NotFound("User not found or insufficient balance.");
            }
            return Ok("Balance withdrawn successfully.");
        }
    }
}
