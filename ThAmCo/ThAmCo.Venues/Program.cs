using Microsoft.EntityFrameworkCore;
using ThAmCo.Venues.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Service Cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


// Register database context with the framework
builder.Services.AddDbContext<VenuesDbContext>();


var app = builder.Build();

//app Cors
app.UseCors("corsapp");

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
