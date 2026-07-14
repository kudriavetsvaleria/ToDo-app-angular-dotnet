using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDoApp.DataAccess;
using ToDoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Register services in the DI container ---

// Registers controller support (MVC controllers without views).
builder.Services.AddControllers();

// Data-access layer: EF Core DbContext bound to the SQL Server connection string.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDataAccess(connectionString);

// Business-logic layer.
builder.Services.AddServices();

// Read the "Jwt" config section (Issuer/Audience/ExpiryMinutes from appsettings,
// Key from user-secrets) into a strongly-typed object, and register it so the
// TokenService can use it.
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Jwt settings are missing.");
builder.Services.AddSingleton(jwtSettings);

// Authentication: teach the app to READ and VALIDATE incoming JWTs.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // check the signature with our secret
            ValidateIssuer = true,           // check it was issued by us
            ValidateAudience = true,         // check it was meant for us
            ValidateLifetime = true,         // check it hasn't expired
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

// Swagger / OpenAPI: generates an interactive API explorer at /swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Adds the "Authorize" button in Swagger so we can send the JWT when testing.
    var jwtScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Paste the JWT here (Swagger adds the 'Bearer ' prefix)."
    };
    options.AddSecurityDefinition("Bearer", jwtScheme);
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

// CORS policy so the Angular dev server (http://localhost:4200) can call this API.
const string AngularCorsPolicy = "AngularDevClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularCorsPolicy, policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// --- Configure the HTTP request pipeline (order matters) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AngularCorsPolicy);

app.UseAuthentication(); // WHO are you? (reads + validates the JWT)
app.UseAuthorization();  // are you ALLOWED? (checks [Authorize])

app.MapControllers();

app.Run();
