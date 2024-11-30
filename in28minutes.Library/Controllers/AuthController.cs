using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using in28minutes.Library.Repository;
using Library.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace in28minutes.Library.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpGet("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = await _unitOfWork.UserService.GetByEmailAsync(request.Email);
        bool isVerify = BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password);
        if (user == null || !isVerify)
        {
            return Unauthorized();
        }
        if (user?.IsBanned ?? false)
        {
            return BadRequest("User is banned");
        }
        if ((bool)(user?.IsLocked ?? false))
        {
            return BadRequest("User is locked");
        }
        var token = GenerateJWTToken(user!.Email, user.Id, user.UserRole);
        return Ok(new {token});
    }
    private string GenerateJWTToken(string? email, Guid id, UserRole userRole)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
        }

        string role = userRole switch
        {
            UserRole.ADMIN => "ADMIN",
            UserRole.MEMBER => "MEMBER",
            UserRole.LIBRARIAN => "LIBRARIAN",
            _ => "MEMBER"
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        // Check if JWT key is available
        var jwtKey = _configuration["JWT:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT Key is not configured.");
        }

        var key = Encoding.ASCII.GetBytes(jwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                new Claim("userId", id.ToString()),
                new Claim("userEmail", email),
                new Claim(ClaimTypes.Role, role)
                }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
