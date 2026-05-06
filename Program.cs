using DiagnosticoMedico.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DiagnosticoMedicoContext")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DiagnosticoMedicoContext");

builder.Services.AddDbContext<DiagnosticoMedicoContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ PUERTO CORRECTO
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");

// ✅ BASE DE DATOS (temporal)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DiagnosticoMedicoContext>();
    // Cambia Migrate() por EnsureCreated()
    context.Database.EnsureCreated();
    Console.WriteLine("--> Tablas creadas con EnsureCreated");
}

// ✅ Swagger SIEMPRE activo
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();