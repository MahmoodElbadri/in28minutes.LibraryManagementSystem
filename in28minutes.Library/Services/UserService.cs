using Azure.Core;
using in28minutes.Library.Repository;
using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace in28minutes.Library.Services;

public class UserService : IUserService<User>
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<UserService> _logger;
    public UserService(ApplicationDbContext db, ILogger<UserService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<User> AddAsync(User entity)
    {
        try
        {
            entity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(entity.Password, workFactor: 11);
            entity.DateJoined = DateTime.Now;
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the user");
            throw new Exception("An error occurred while creating the user", ex);
        }
        finally
        {
            // This block will always execute
            _logger.LogInformation("Finished attempting to create a user, success or failure.");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        try
        {
            return await _db.Users.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting all users");
            throw new Exception("An error occurred while getting all users", ex);
        }
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with email {email} not found");
        }
        return user;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {

        var user = await _db.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }
        return user;
    }

    public async Task<User> UpdateAsync(Guid id, User entity)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }
        try
        {
            if (!string.IsNullOrEmpty(entity.UserName))
            {
                entity.UserName = user.UserName;
            }
            if (!string.IsNullOrEmpty(entity.Email))
            {
                entity.Email = user.Email;
            }
            if (!string.IsNullOrEmpty(entity.Password))
            {
                entity.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(entity.Password, workFactor: 11);
            }
            if (entity.ProfilePicture != null)
            {
                entity.ProfilePicture = user.ProfilePicture;
            }
            if (!string.IsNullOrEmpty(entity.UserRole.ToString()))
            {
                entity.UserRole = user.UserRole;
            }
            if (entity.IsBanned.HasValue)
            {
                user.IsBanned = entity.IsBanned;
            }

            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the user");
            throw new Exception("An error occurred while updating the user", ex);
        }
        finally
        {
            // This block will always execute
            _logger.LogInformation("Finished attempting to update a user, success or failure.");

        }
    }
}
