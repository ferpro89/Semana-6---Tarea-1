using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Semana5.Data;

var builder = WebApplication.CreateBuilder(args);
const string DevCors = "DevCors";
// Add services to the container.

var conexion = builder.Configuration.GetConnectionString("cn")
    ?? throw new InvalidOperationException("No existe la base de datos");

builder.Services.AddDbContext<ServerDbContext>(
    op => op.UseMySql(conexion, ServerVersion.Parse("5.7.24")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>

{
    options.AddPolicy("MyPolicy",
       builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Especifica los orígenes permitidos
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
