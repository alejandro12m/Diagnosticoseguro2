using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DiagnosticoMedico.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DiagnosticoMedicoContext")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DiagnosticoMedicoContext");

builder.Services.AddDbContext<DiagnosticoMedicoContext>(options =>
    options.UseNpgsql(connectionString));
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();
// Cambia 8080 por 10000
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DiagnosticoMedicoContext>();
    context.Database.Migrate();
}
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


// app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();