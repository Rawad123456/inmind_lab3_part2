using Application.Enrollments.Commands;
using FluentValidation;
using FluentValidation.AspNetCore;
using inmind_session5_DDD.API.Filters;
using inmind_session5_DDD.API.Services;
using inmind_session5_DDD.Application.Students.Commands;
using inmind_session5_DDD.Application.Students.Queries;
using inmind_session5_DDD.Application.Validators;
using inmind_session5_DDD.Infrastructure;
using inmind_session5_DDD.Infrastructure.Students.Handlers;
using MediatR;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<CreateStudentCommandHandler>();
    config.RegisterServicesFromAssemblyContaining<GetAllStudentsQueryHandler>();
    config.RegisterServicesFromAssemblyContaining<CreateEnrollmentCommandHandler>();
});





builder.Services.AddValidatorsFromAssembly(typeof(CreateStudentCommandValidator).Assembly);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); 
});


builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>(); 
    })
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<CreateStudentCommandValidator>();
    });

builder.Services.AddScoped<ObjectMapperService>();




builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentCommandValidator>();


builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<inmind_session5_DDD.API.Middleware.RequestLoggingMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();