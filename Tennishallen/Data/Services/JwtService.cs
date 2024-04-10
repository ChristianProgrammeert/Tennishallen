using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Tennishallen.Data.Models;

namespace Tennishallen.Data.Services;

public class JwtService
{

    public readonly string? Token;
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    /// <summary>
    /// Construct JwtService from the token contained in a HttpRequest cookies.
    /// </summary>
    /// <param name="request">The Request to take the cookies of.</param>
    public JwtService(HttpRequest request)
    {
        Token = request.Cookies["Token"];
    }

    
    /// <summary>
    /// Construct JwtService directly from a token;
    /// </summary>
    /// <param name="token">The token to generate JwtService from.</param>
    public JwtService(string token)
    {
        Token = token;
    }

    /// <summary>
    /// Construct the JwtService based on a user.
    /// </summary>
    /// <param name="user">The user to base JwtService on.</param>
    public JwtService(User user)
    {
        Token = GenerateToken(user);
    }

    /// <summary>
    /// How lng it takes for a token to expire.
    /// </summary>
    private static readonly TimeSpan ExpireTimeSpan = TimeSpan.FromMinutes(20);
    /// <summary>
    /// The secret key of the token;
    /// TODO: move to appsettings.json
    /// </summary>
    private static readonly byte[] SecretKey = "A-VERY-SAFE-SECRET-KEY-THATS-TOTALLY-NOT-PREDICTABLE"u8.ToArray();
    
    /// <summary>
    /// Generate a JWT token that holds user claims.
    /// </summary>
    /// <param name="user">The user to take the claims from.</param>
    /// <returns>The token of the user.</returns>
    private static string GenerateToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.Fullname),
                    .. user.Groups.Select(g => new Claim(ClaimTypes.Role, g.Name.ToString())).ToList(),
                ]
            ),
            Expires = DateTime.UtcNow + ExpireTimeSpan,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(SecretKey),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };
        var token = TokenHandler.CreateToken(tokenDescriptor);
        return TokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Validate a JWT token.
    /// </summary>
    /// <returns>True if the token is valid.</returns>
    public bool ValidateToken()
    {
        if (Token == null) return false;
        try
        {
            TokenHandler.ValidateToken(Token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            }, out _);

            return true;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
    }
    
    /// <summary>
    /// Get the claims identity from the token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public ClaimsPrincipal GetClaimsIdentity()
    {
        var jsonToken = new JwtSecurityTokenHandler().ReadToken(Token) as JwtSecurityToken;
        var identity = new ClaimsIdentity(jsonToken?.Claims, "Bearer");
        return new ClaimsPrincipal(identity);
    }
    
    /// <summary>
    /// Get the user id from the Token
    /// </summary>
    /// <returns>The user id in the token or null</returns>
    public Guid? GetUserId()
    {
        if (!ValidateToken()) return null;
        var jsonToken = TokenHandler.ReadToken(Token) as JwtSecurityToken;
        var nameClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "unique_name");
        if (nameClaim == null) return null; 
        return Guid.Parse(nameClaim.Value);
    }
    
    /// <summary>
    /// The email contained in the token
    /// </summary>
    /// <returns>The users email in the token or null</returns>
    public string? GetUserEmail()
    {
        if (!ValidateToken()) return null;
        var jsonToken = TokenHandler.ReadToken(Token) as JwtSecurityToken;
        var nameClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "email");
        return nameClaim?.Value;
    }

    /// <summary>
    /// The full name contained in the token
    /// </summary>
    /// <returns>The users Full Name in the token or null</returns>
    public string? GetUserName()
    {
        if (!ValidateToken()) return null;
        var jsonToken = TokenHandler.ReadToken(Token) as JwtSecurityToken;
        var nameClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "given_name");
        return nameClaim?.Value;
    }

    /// <summary>
    /// The groups contained in the token
    /// </summary>
    /// <returns>The users groups in the token or null</returns>
    public IEnumerable<Group.GroupName>? GetUserGroups()
    {
        if (!ValidateToken()) return null;
        var jsonToken = TokenHandler.ReadToken(Token) as JwtSecurityToken;
        var nameClaim = jsonToken?.Claims.Where(c => c.Type == "role");
        return nameClaim?.Select(
            claim => Enum.Parse<Group.GroupName>(claim.Value)
        );
    }
}