using inmind_session5_DDD.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class StudentCountLoggerService : BackgroundService
{
    private readonly ILogger<StudentCountLoggerService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public StudentCountLoggerService(
        ILogger<StudentCountLoggerService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StudentCountLoggerService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var count = await context.Students.CountAsync(stoppingToken);
                _logger.LogInformation("Total number of students: {Count}", count);
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        _logger.LogInformation("StudentCountLoggerService stopped.");
    }
}