using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SecureFileStorage.Core.Dtos;
using SecureFileStorage.Core.Entities;
using SecureFileStorage.Core.Interfaces;

using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;

[Route("api/[controller]")]
[ApiController]
public class UsersController: ControllerBase {
    private readonly IUserRepository _userRepository;
    public UsersController(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    [HttpPost("register")]
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
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddUserAsync(user);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] RegistrationDto request) {
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

        return Ok();
    }

    private string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string passwordHash) {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}