using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SecureFileStorage.Core.Dtos;
using SecureFileStorage.Core.Entities;
using SecureFileStorage.Core.Interfaces;
using SecureFileStorage.Web.Data;
using SecureFileStorage.Web.Services;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController: ControllerBase {
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    public UsersController(IUserRepository userRepository, ITokenService tokenService) {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = users.Select(MapToUserDto).ToList();
            return Ok(userDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegistrationDto request) {
        if (request.Email == null || request.Password == null) {
            return BadRequest("Email i lozinka su obavezni!");
        }

        if (await _userRepository.UserExistsAsync(request.Email)) {
            return BadRequest("Korisnik s emailom već postoji!");
        }

        var user = new User {
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            UserTypeId = request.UserTypeId,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddUserAsync(user);
        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto request) {
        if (request.Email == null || request.Password == null) {
            return BadRequest("Email i lozinka su obavezni!");
        }

        if (!await _userRepository.UserExistsAsync(request.Email)) {
            return BadRequest("Korisnik s emailom ne postoji!");
        }

        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (!VerifyPassword(request.Password, user.PasswordHash)) {
            return BadRequest("Pogrešna lozinka!");
        }

        var token = _tokenService.GenerateToken(user.Id);
        return Ok(new {token});
    }

    private string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string passwordHash) {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    private UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserTypeId = user.UserTypeId,
            CreatedAt = user.CreatedAt,
            LastLogin = user.LastLogin,
            UserType = user.UserType
        };
    }
}