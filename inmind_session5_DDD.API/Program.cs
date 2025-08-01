using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Application.Enrollments.Commands;
using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using inmind_session5_DDD.API.Filters;
using inmind_session5_DDD.API.Logging;
using inmind_session5_DDD.API.Middleware;
using inmind_session5_DDD.API.Services;
using inmind_session5_DDD.Application.Students.Commands;
using inmind_session5_DDD.Application.Students.Queries;
using inmind_session5_DDD.Application.Validators;
using inmind_session5_DDD.Common.Tenant;
using inmind_session5_DDD.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);


LoggingConfiguration.ConfigureSerilog(builder);


builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));

builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

builder.Services.AddHttpClient();



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
    options.Filters.Add<GlobalExceptionFilter>();
})
.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<CreateStudentCommandValidator>();
});


builder.Services.AddScoped<ObjectMapperService>();


builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddHostedService<StudentCountLoggerService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ITenantProvider, TenantProvider>();




builder.Services.AddMemoryCache();


builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "PostgreSQL",
        timeout: TimeSpan.FromSeconds(5),
        tags: new[] { "db", "sql", "postgres" });


JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080/realms/inmind_system";
        options.RequireHttpsMetadata = false;
        options.Audience = "inmindapi"; // must match "azp" claim
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:8080/realms/inmind_system",
            ValidateAudience = true,
            ValidAudience = "inmindapi",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = "roles" 
        };
    
        
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

                var realmAccess = context.Principal.FindFirst("realm_access");
                if (realmAccess != null)
                {
                    var parsed = JsonDocument.Parse(realmAccess.Value);
                    if (parsed.RootElement.TryGetProperty("roles", out var roles))
                    {
                        foreach (var role in roles.EnumerateArray())
                        {
                            claimsIdentity.AddClaim(new Claim("roles", role.GetString()));
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };
    });






builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "InMind API", Version = "v1" });

    // Add JWT Bearer support
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGci...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


app.UseMiddleware<TenantMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InMind API V1");
        c.RoutePrefix = string.Empty; // Swagger UI at app root
    });
}


app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<TenantMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();




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
