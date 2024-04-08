using Microsoft.EntityFrameworkCore;
using Tennishallen.Data.Base;
using Tennishallen.Data.Models;
using Tennishallen.Data.Utils;

namespace Tennishallen.Data.Services;

public class AuthService(ApplicationDbContext context) : BaseRepository<User, Guid>(context)
{
    /// <summary>
    /// Get a user by its email
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <returns>The user with the given email or null.</returns>
    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users.Include(
            u => u.Groups
        ).FirstOrDefaultAsync(u => u.Email == email);
    }


    /// <summary>
    /// Gets the user by its email and return it if the password match.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password to match.</param>
    /// <returns>The user where the email and password match.</returns>
    public async Task<User?> GetUserByEmailAndPassword(string email, string password)
    {
        var user = await GetUserByEmail(email);
        if (user == null || !PasswordHasher.VerifyPassword(password, user.Password) || !user.Active)
            return null;
        return user;
    }
}