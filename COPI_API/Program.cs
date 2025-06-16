using COPI_API.Models;
using COPI_API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Conexão com MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AvaliacaoService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//    var unidadesKPI = await context.UnidadesKPI
//        .Include(uk => uk.Unidade)
//        .Include(uk => uk.KPI)
//        .ToListAsync();

//    foreach (var unidadeKPI in unidadesKPI)
//    {
//        bool existe = await context.ResultadosKPI.AnyAsync(r =>
//            r.KPI!.Id == unidadeKPI.KPIId && r.UnidadeKPIId == unidadeKPI.Id);

//        if (!existe)
//        {
//            context.ResultadosKPI.Add(new ResultadoKPI
//            {
//                KPIId = unidadeKPI.KPIId,
//                UnidadeKPIId = unidadeKPI.Id,
//                Status = "NAO",
//                DataRegistro = DateTime.UtcNow
//            });
//        }
//    }

//    await context.SaveChangesAsync();
//}

app.Run();
