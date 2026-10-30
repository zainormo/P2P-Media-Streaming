using Common.Contracts.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tracker.Infrastructure.Database;
using Tracker.Infrastructure.Security;

namespace Tracker.Modules.Auth;

public class AuthService(AppDbContext db, JwtTokenService jwt)
{
    readonly PasswordHasher<User> hasher = new();

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(request.Username) || request.Password.Length < 8) return false;
        if (await db.Set<User>().AnyAsync(u => u.Email == email)) return false;

        var user = new User { Username = request.Username.Trim(), Email = email };
        user.PasswordHash = hasher.HashPassword(user, request.Password);
        db.Set<User>().Add(user);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await db.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        if (user is null) return null;
        if (hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed) return null;
        return new AuthResponse(jwt.Create(user.Id, user.Username), user.Username);
    }
}
