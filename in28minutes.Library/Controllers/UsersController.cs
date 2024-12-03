using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using in28minutes.Library.DTOs;
using in28minutes.Library.Repository;
using in28minutes.Library.Services;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace in28minutes.Library.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersController> _logger; // Inject the logger>
    public UsersController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UsersController> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _unitOfWork.UserService.GetAllAsync();
        IEnumerable<UserResponse> userResponses = _mapper.Map<IEnumerable<UserResponse>>(users);
        return Ok(userResponses);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        string? token = null;

        // Step 1: Check for the token in the Authorization header
        var authHeader = Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authHeader.Substring("Bearer ".Length).Trim();
        }

        // Step 2: If no token is found in the Authorization header, check the authToken cookie
        if (string.IsNullOrEmpty(token))
        {
            token = Request.Cookies["authToken"];
        }

        // Step 3: If no token is found in both the Authorization header and the cookie, return Unauthorized
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized();
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assuming the userId is stored in the token's claims
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);
            var user = await _unitOfWork.UserService.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Map the user entity to the response DTO
            var userResponse = _mapper.Map<UserResponse>(user);
            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            // Log the exception and return a generic error message
            _logger.LogError(ex, "An error occurred while fetching the current user.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _unitOfWork.UserService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var userResponse = _mapper.Map<UserResponse>(user);
        return Ok(userResponse);
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateUser([FromBody] UserAddRequest userRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var user = _mapper.Map<User>(userRequest);
        var createdUser = await _unitOfWork.UserService.AddAsync(user);
        var reponse = _mapper.Map<UserResponse>(createdUser);
        _unitOfWork.SaveChangesAsync();
        return Ok(reponse);
    }

    [HttpPost("{id}")]
    [Authorize(Roles = "ADMIN") ]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody]UserUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = _mapper.Map<User>(request);
        var updatedUser = await _unitOfWork.UserService.UpdateAsync(id, user);
        var userResult = _mapper.Map<UserResponse>(updatedUser);
        _unitOfWork.SaveChangesAsync();
        return Ok(userResult);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task <IActionResult> DeleteUser(Guid id)
    {
        await _unitOfWork.UserService.DeleteAsync(id);
        _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

}
