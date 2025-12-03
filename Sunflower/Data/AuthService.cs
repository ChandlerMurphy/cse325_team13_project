using MongoDB.Driver;
using Microsoft.AspNetCore.Identity;
using Sunflower.Data; // This allows AuthService to see User

public class AuthService
{
    private readonly MongoDbContext _ctx;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(MongoDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<bool> RegisterUser(string username, string email, string password)
    {
        // Check if username already exists
        var existing = await _ctx.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
        if (existing != null) return false;

        var user = new User
        {
            Username = username,
            Email = email
        };

        user.PasswordHash = _hasher.HashPassword(user, password);

        await _ctx.Users.InsertOneAsync(user);
        return true;
    }

    public async Task<User?> ValidateUser(string username, string password)
    {
        var user = await _ctx.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
        if (user == null) return null;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success ? user : null;
    }
}




// using MongoDB.Driver;
// using Microsoft.AspNetCore.Identity;
// using Sunflower.Data;

// public class AuthService
// {
//     private readonly MongoDbContext _ctx;
//     private readonly PasswordHasher<User> _hasher = new();

//     public AuthService(MongoDbContext ctx)
//     {
//         _ctx = ctx;
//     }

//     public async Task<bool> RegisterUser(string username, string email, string password)
//     {
//         var existing = await _ctx.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
//         if (existing != null) return false;

//         var user = new User
//         {
//             Username = username,
//             Email = email,
//             Role = "customer" // default role
//         };

//         user.PasswordHash = _hasher.HashPassword(user, password);
//         await _ctx.Users.InsertOneAsync(user);
//         return true;
//     }

//     public async Task<User?> ValidateUser(string username, string password)
//     {
//         var user = await _ctx.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
//         if (user == null) {
//             Console.WriteLine($"User not found: {username}"); // Testing
//             return null;
//         }

//         var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
//         Console.WriteLine($"Password verification result: {result}"); // Testing
//         return result == PasswordVerificationResult.Success ? user : null;
//     }
// }
