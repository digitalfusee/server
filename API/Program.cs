using Data.Contexts;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("LiveDbString");

// Configure database connections
builder.Services.AddDbContext<CourseDataContext>(
    options => options.UseNpgsql(connectionString));

// For InvalidCastException
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors(options =>
    options.WithOrigins("*")
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseAuthorization();
app.MapControllers();
app.Run();
