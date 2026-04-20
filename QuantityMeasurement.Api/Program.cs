using System.Text;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantityMeasurement.BusinessLayer.Auth;
using QuantityMeasurement.BusinessLayer.Interfaces;
using QuantityMeasurement.BusinessLayer.Services;
using QuantityMeasurement.Repository;
using QuantityMeasurement.Repository.Data;
using QuantityMeasurement.Repository.EF;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Redis;
using QuantityMeasurement.Api.Middleware;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

var repoType = builder.Configuration["App:RepositoryType"] ?? "cache";

// AppDbContext is always registered – the users table lives in PostgreSQL.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("QuantityMeasurement.Api")));

// Measurement repository – swap via App:RepositoryType in appsettings.json
if (repoType.Equals("database", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<IQuantityMeasurementRepository>(sp =>
        new EfQuantityMeasurementRepository(sp.GetRequiredService<AppDbContext>()));
}
else if (repoType.Equals("redis", StringComparison.OrdinalIgnoreCase))
{
    var redisConn = builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379";
    builder.Services.AddSingleton<IQuantityMeasurementRepository>(
        new QuantityMeasurementRedisRepository(redisConn));
}
else
{
    builder.Services.AddSingleton<IQuantityMeasurementRepository, QuantityMeasurementCacheRepository>();
}

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IQuantityService, QuantityService>();

//  Authentication 
var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = "ExternalCookie"; // Required for Google
    })
    .AddCookie("ExternalCookie")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    })
    // UC18: Google OAuth2
    // Requires Google:ClientId and Google:ClientSecret in appsettings.json 
    .AddGoogle(options =>
    {
        options.ClientId     = builder.Configuration["Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Quantity Measurement API",
        Version     = "v1",
        Description = "REST API for quantity measurements - UC18.\n\n" +
                      "How to authenticate:\n" +
                      "1. POST /api/v1/auth/register to create an account\n" +
                      "2. POST /api/v1/auth/login with your credentials\n" +
                      "3. Copy the returned token\n" +
                      "4. Click the Authorize button and paste it\n\n" +
                      "Google OAuth2 (UC18):\n" +
                      "Open GET /api/v1/auth/google in your browser to sign in with Google."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Paste your JWT token here."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Ensure tables exist on startup.
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        Console.WriteLine("[UC18] Database tables ready.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[UC18] DB not available ({ex.Message}). Running in {repoType} mode.");
    }
}

app.UseMiddleware<GlobalExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
