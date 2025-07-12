using Microsoft.EntityFrameworkCore;

using AutoMapper;
using lab3_inmind_part2.Data;


var builder = WebApplication.CreateBuilder(args);

// DB connection
builder.Services.AddDbContext<UniversityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();