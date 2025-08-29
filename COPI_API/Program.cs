using COPI_API;
using COPI_API.Models;
using COPI_API.Models.PIBPEntities;
using COPI_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Conexão com MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// 2. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// 3. Controllers e JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// 4. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 5. Injeção de Serviços
builder.Services.AddScoped<AvaliacaoService>();

// 6. Autenticação JWT
var chaveJwt = builder.Configuration["Jwt:Key"] ?? "sua_chave_super_secreta";
var key = Encoding.ASCII.GetBytes(chaveJwt);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RoleClaimType = ClaimTypes.Role // <-- Adicione esta linha!
    };
});

// 7. Autorização baseada em Roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Gestor", policy => policy.RequireRole("Gestor"));
    options.AddPolicy("UsuarioPlus", policy => policy.RequireRole("UsuarioPlus"));
    options.AddPolicy("Usuario", policy => policy.RequireRole("Usuario"));
    options.AddPolicy("UsuarioPIBP", policy => policy.RequireRole("UsuarioPIBP"));
    options.AddPolicy("GestorPIBP", policy => policy.RequireRole("GestorPIBP"));
    options.AddPolicy("UsuarioCFCI", policy => policy.RequireRole("UsuarioCFCI"));
    options.AddPolicy("GestorCFCI", policy => policy.RequireRole("GestorCFCI"));
    options.AddPolicy("UsuarioDPE", policy => policy.RequireRole("UsuarioDPE"));
    options.AddPolicy("GestorDPE", policy => policy.RequireRole("GestorDPE"));
    options.AddPolicy("UsuarioDTA", policy => policy.RequireRole("UsuarioDTA"));
    options.AddPolicy("GestorDTA", policy => policy.RequireRole("GestorDTA"));
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication(); // Importante
app.UseAuthorization();

app.MapControllers();

app.Run();

