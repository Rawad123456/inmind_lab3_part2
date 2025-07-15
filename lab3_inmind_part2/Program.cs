using Microsoft.EntityFrameworkCore;

using AutoMapper;
using lab3_inmind_part2.Data;
using lab3_inmind_part2.Filters;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<UniversityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>(); 
});

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();