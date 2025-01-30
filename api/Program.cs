using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Domain.Models;
using API.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using api.Infrastructure.Config;
using Microsoft.AspNetCore.Identity;
using API.Domain.Interfaces;
using API.Infrastructure.Persistence.Repositories;
using api.Infrastructure.Persistence.Repositories;
using api.Domain.Interfaces;
using api.Application.Services.ServiceContracts;
using api.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using api.Application.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        // Configure password options
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6; // Set your desired minimum length
        options.Password.RequiredUniqueChars = 1;
    }
)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole<Guid>>();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddScoped<IRevokedTokenRepository, RevokedTokenRepository>();


// Register the Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRevokedTokenService, RevokedTokenService>();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add JWT authentication
var jwtSection = builder.Configuration.GetSection("JWTBearerTokenSettings");
builder.Services.Configure<JWTBearerTokenSettings>(jwtSection);

var jwtBearerTokenSettings = jwtSection.Get<JWTBearerTokenSettings>();

if (jwtBearerTokenSettings == null || string.IsNullOrEmpty(jwtBearerTokenSettings.SecretKey))
{
    throw new InvalidOperationException("SecretKey is not configured properly.");
}

var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtBearerTokenSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtBearerTokenSettings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var tokenRevocationService = context.HttpContext.RequestServices.GetRequiredService<IRevokedTokenService>();
            var token = context.SecurityToken as JwtSecurityToken;
            if (token != null && await tokenRevocationService.IsTokenRevokedAsync(token.RawData))
            {
                context.Fail("This token has been revoked.");
            }
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Enable serving static files

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();