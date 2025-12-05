using Sunflower.Components;
using Sunflower.Data;
using DotNetEnv;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// ==========================
// Razor / Interactive
// ==========================
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ==========================
// MongoDB Settings
// ==========================
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = Environment.GetEnvironmentVariable("MONGO_URL")
        ?? throw new InvalidOperationException("MONGO_URL not set.");
    options.DatabaseName = "Sunflower";
});

builder.Services.AddSingleton<MongoDbContext>();

// ==========================
// JWT Authentication
// ==========================
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true
        };

        // Allow reading token from Authorization header
        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                // Custom: read from localStorage if using WebSockets
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// ==========================
// Add HttpClient for Login page
// ==========================
builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServerBaseUrl"] ?? "http://localhost:5241/");
});

// ==========================
// Custom Auth Provider
// ==========================
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddScoped<AuthService>();

builder.Services.AddHttpContextAccessor();

// ==========================
// Controllers
// ==========================
builder.Services.AddControllers();
builder.Services.AddScoped<PlantService>();


// ==========================
// Favorite Flowers
// ==========================
builder.Services.AddSingleton<FavoriteFlowersService>();

var app = builder.Build();
// ==========================
// Middleware
// ==========================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// ==========================
// Routes
// ==========================
app.MapStaticAssets();
app.MapControllers();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();





// using Sunflower.Components;
// using Sunflower.Data;
// using DotNetEnv;
// using Microsoft.AspNetCore.Components.Authorization;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;

// Env.Load();

// var builder = WebApplication.CreateBuilder(args);

// // ==========================
// // Razor / Interactive
// // ==========================
// builder.Services.AddRazorComponents()
//     .AddInteractiveServerComponents();

// // ==========================
// // MongoDB Settings
// // ==========================
// builder.Services.Configure<MongoDbSettings>(options =>
// {
//     options.ConnectionString = Environment.GetEnvironmentVariable("MONGO_URL")
//         ?? throw new InvalidOperationException("MONGO_URL not set.");
//     options.DatabaseName = "Sunflower";
// });

// builder.Services.AddSingleton<MongoDbContext>();

// // ==========================
// // JWT Authentication
// // ==========================
// var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
// builder.Services.Configure<JwtSettings>(jwtSettingsSection);

// var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
// var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

// builder.Services.AddAuthentication("Bearer")
//     .AddJwtBearer("Bearer", options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidIssuer = jwtSettings.Issuer,

//             ValidateAudience = true,
//             ValidAudience = jwtSettings.Audience,

//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(key),

//             ValidateLifetime = true
//         };

//         options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
//         {
//             OnMessageReceived = ctx =>
//             {
//                 // Custom: read from localStorage if using WebSockets
//                 return Task.CompletedTask;
//             }
//         };
//     });

// builder.Services.AddAuthorization();

// // ==========================
// // Add JWT Authorization Handler
// // ==========================
// builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// // ==========================
// // Add HttpClient for Login page (with JWT handler)
// // ==========================
// builder.Services.AddHttpClient("ServerAPI", client =>
// {
//     client.BaseAddress = new Uri(builder.Configuration["ServerBaseUrl"] ?? "http://localhost:5241/");
// })
// .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

// // ==========================
// // Custom Auth Provider
// // ==========================
// builder.Services.AddScoped<CustomAuthStateProvider>();
// builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
//     sp.GetRequiredService<CustomAuthStateProvider>());

// builder.Services.AddScoped<AuthService>();
// builder.Services.AddHttpContextAccessor();

// // ==========================
// // Controllers
// // ==========================
// builder.Services.AddControllers();

// var app = builder.Build();

// // ==========================
// // Middleware
// // ==========================
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error", createScopeForErrors: true);
//     app.UseHsts();
// }

// app.UseHttpsRedirection();

// app.UseAuthentication();
// app.UseAuthorization();

// app.UseAntiforgery();

// // ==========================
// // Routes
// // ==========================
// app.MapStaticAssets();
// app.MapControllers();

// app.MapRazorComponents<App>()
//    .AddInteractiveServerRenderMode();

// app.Run();
