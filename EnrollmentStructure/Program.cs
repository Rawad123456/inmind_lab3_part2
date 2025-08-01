using EnrollmentStructure.Data;

using EnrollmentStructure.Tenant;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHostedService<StudentCreatedConsumer>();
builder.Services.AddHostedService<CourseCreatedConsumer>();



builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<ITenantProvider, TenantProvider>(); 




builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();





if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
   
}

app.UseMiddleware<EnrollmentStructure.Middleware.TenantMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();