using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using EnrollmentStructure.Data;
using EnrollmentStructure.Entities;
using Shared.Messages;
using RabbitMQ.Client;

public class StudentCreatedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public StudentCreatedConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "student-created", durable: false, exclusive: false, autoDelete: false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<StudentCreated>(json);

            if (message != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var student = new Student
                {
                    Id = message.Id,
                    FullName = message.FullName,
                    TenantId = message.TenantId
                };

                db.Students.Add(student);
                await db.SaveChangesAsync();
            }
        };

        channel.BasicConsume(queue: "student-created", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}