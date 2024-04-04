namespace Tennishallen.Data.Utils;

public class PasswordHasher
{
    /// <summary>
    /// Hash a password using BCrypt.
    /// </summary>
    /// <param name="raw">The raw unhashed password.</param>
    /// <returns>A Bcrypt hash of raw.</returns>
    public static string HashPassword(string raw)
    {
        return BCrypt.Net.BCrypt.HashPassword(raw, BCrypt.Net.BCrypt.GenerateSalt());
    }

    /// <summary>
    /// Verify a raw password against a hash.
    /// </summary>
    /// <param name="raw">The raw unhashed password.</param>
    /// <param name="password">The hashed password.</param>
    /// <returns>True when password is a hash of raw. Else false.</returns>
    public static bool VerifyPassword(string raw, string password)
    {
        return BCrypt.Net.BCrypt.Verify(raw, password);
    }
}