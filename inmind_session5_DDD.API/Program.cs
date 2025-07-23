using Application.Enrollments.Commands;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using inmind_session5_DDD.API.Filters;
using inmind_session5_DDD.API.Logging;
using inmind_session5_DDD.API.Services;
using inmind_session5_DDD.Application.Students.Commands;
using inmind_session5_DDD.Application.Students.Queries;
using inmind_session5_DDD.Application.Validators;
using inmind_session5_DDD.Infrastructure;
//using inmind_session5_DDD.Infrastructure.Students.Handlers;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

LoggingConfiguration.ConfigureSerilog(builder);



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

builder.Services.AddHostedService<StudentCountLoggerService>();


builder.Services.AddMemoryCache();



builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "PostgreSQL",
        timeout: TimeSpan.FromSeconds(5),
        tags: new[] { "db", "sql", "postgres" });





var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<inmind_session5_DDD.API.Middleware.RequestLoggingMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseStaticFiles();

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets")),
    RequestPath = "/assets"
});


app.Run();