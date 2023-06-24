using Data.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure database connections
builder.Services.AddDbContext<CourseDataContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("LocalDbString")));

// For InvalidCastException
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors(options =>
            options.WithOrigins("*").
            AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();
app.MapControllers();
app.Run();
