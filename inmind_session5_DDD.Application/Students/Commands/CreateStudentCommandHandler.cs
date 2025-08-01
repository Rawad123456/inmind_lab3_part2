using inmind_session5_DDD.Domain.Entities;
using inmind_session5_DDD.Persistence;
using MainAPI.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Shared.Messages;

namespace inmind_session5_DDD.Application.Students.Commands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Guid>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateStudentCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateStudentCommandHandler(ApplicationDbContext context, ILogger<CreateStudentCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            
            var student = new Student
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync(cancellationToken);

            // Get tenant ID from HTTP header X-Tenant-ID
            var tenantId = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-ID"].ToString() ?? string.Empty;

            var publisher = new RabbitMqPublisher();
            publisher.Publish(new StudentCreated
            {
                Id = student.Id,
                FullName = student.FullName,
                TenantId = tenantId  // Pass tenantId explicitly
            }, "student-created");

            _logger.LogInformation("Student created with ID {StudentId} and Name {FullName}", student.Id, student.FullName);

            return student.Id;
        }
    }
}