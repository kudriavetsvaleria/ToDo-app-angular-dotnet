var builder = WebApplication.CreateBuilder(args);

// --- Register services in the DI container ---

// Registers controller support (MVC controllers without views).
builder.Services.AddControllers();

// Swagger / OpenAPI: generates an interactive API explorer at /swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
