using Microsoft.EntityFrameworkCore;

using AutoMapper;
using FluentValidation.AspNetCore;
using lab3_inmind_part2.Data;
using lab3_inmind_part2.Filters;
using lab3_inmind_part2.Middlewares;
using lab3_inmind_part2.Validators;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<UniversityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>(); 
    
});

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudentDtoValidator>());

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); 
});



builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();



app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();