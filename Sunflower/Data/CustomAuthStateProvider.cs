using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Sunflower.Data;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;

    public CustomAuthStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Called by Blazor to get the current auth state.
    /// This reads the JWT from localStorage on every page load.
    /// </summary>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // ✅ Read token from localStorage
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                // No token found -> anonymous user
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Extract claims
            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "customer";

            if (string.IsNullOrWhiteSpace(username))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // ✅ Build ClaimsPrincipal
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            }, "jwt_auth");

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            // If anything fails, treat as anonymous
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    /// <summary>
    /// Call this when login succeeds
    /// </summary>
    public void NotifyUserAuthentication(User user)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role ?? "customer")
        }, "jwt_auth");

        var principal = new ClaimsPrincipal(identity);

        // ✅ Notify Blazor UI of updated auth state
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    /// <summary>
    /// Call this when logging out
    /// </summary>
    public void NotifyUserLogout()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        // ✅ Notify Blazor UI
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}


// using Microsoft.AspNetCore.Components.Authorization;
// using Microsoft.JSInterop;
// using System.Security.Claims;
// using System.IdentityModel.Tokens.Jwt;
// using Sunflower.Data;

// public class CustomAuthStateProvider : AuthenticationStateProvider
// {
//     private readonly IJSRuntime _jsRuntime;

//     public CustomAuthStateProvider(IJSRuntime jsRuntime)
//     {
//         _jsRuntime = jsRuntime;
//     }

//     /// <summary>
//     /// Called by Blazor to get the current auth state.
//     /// This reads the JWT from localStorage on every page load.
//     /// </summary>
//     public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//     {
//         try
//         {
//             // ✅ Read token from localStorage
//             var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");

//             if (string.IsNullOrWhiteSpace(token))
//             {
//                 // No token found -> anonymous user
//                 return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//             }

//             var handler = new JwtSecurityTokenHandler();
//             var jwtToken = handler.ReadJwtToken(token);

//             // Extract claims
//             var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
//             var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "customer";

//             if (string.IsNullOrWhiteSpace(username))
//             {
//                 return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//             }

//             // ✅ Build ClaimsPrincipal
//             var identity = new ClaimsIdentity(new[]
//             {
//                 new Claim(ClaimTypes.Name, username),
//                 new Claim(ClaimTypes.Role, role)
//             }, "jwt_auth");

//             var user = new ClaimsPrincipal(identity);

//             return new AuthenticationState(user);
//         }
//         catch
//         {
//             // If anything fails, treat as anonymous
//             return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//         }
//     }

//     /// <summary>
//     /// Call this when login succeeds
//     /// </summary>
//     public void NotifyUserAuthentication(User user)
//     {
//         var identity = new ClaimsIdentity(new[]
//         {
//             new Claim(ClaimTypes.Name, user.Username),
//             new Claim(ClaimTypes.Role, user.Role ?? "customer")
//         }, "jwt_auth");

//         var principal = new ClaimsPrincipal(identity);

//         // ✅ Notify Blazor UI of updated auth state
//         NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
//     }

//     /// <summary>
//     /// Call this when logging out
//     /// </summary>
//     public void NotifyUserLogout()
//     {
//         var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

//         // ✅ Notify Blazor UI
//         NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
//     }
// }
