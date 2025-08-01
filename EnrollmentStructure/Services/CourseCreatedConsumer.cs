using System.Text;
using System.Text.Json;
using EnrollmentStructure.Data;
using EnrollmentStructure.Entities;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messages;

public class CourseCreatedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CourseCreatedConsumer> _logger;
    private IConnection _connection;
    private IModel _channel;

    public CourseCreatedConsumer(IServiceProvider serviceProvider, ILogger<CourseCreatedConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        
        _logger.LogInformation("CourseCreatedConsumer constructor invoked.");

        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: "course-created",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CourseCreatedConsumer started and listening to 'course.created' queue");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Received message from 'course.created': {Message}", message);

                var courseEvent = JsonSerializer.Deserialize<CourseCreated>(message);
                if (courseEvent is null)
                {
                    _logger.LogWarning("Received null or invalid CourseCreated event.");
                    return;
                }

                _logger.LogInformation("Parsed CourseCreated event: Id={Id}, Title={Title}", courseEvent.Id, courseEvent.Title);

                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var courseExists = await dbContext.Courses.AnyAsync(c => c.Id == courseEvent.Id);
                if (!courseExists)
                {
                    var newCourse = new Course
                    {
                        Id = courseEvent.Id,
                        Name = courseEvent.Title,
                        TenantId = courseEvent.TenantId 
                        
                        
                    };

                    dbContext.Courses.Add(newCourse);
                    await dbContext.SaveChangesAsync();

                    _logger.LogInformation("New course saved to DB: Id={Id}, Name={Name}", newCourse.Id, newCourse.Name);
                }
                else
                {
                    _logger.LogInformation("Course already exists in DB: Id={Id}", courseEvent.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the 'course.created' message.");
            }
        };

        _channel.BasicConsume(
            queue: "course-created",
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
