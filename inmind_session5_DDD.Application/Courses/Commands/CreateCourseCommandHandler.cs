using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Domain.Entities;
using inmind_session5_DDD.Persistence;
using MainAPI.Messaging;
using MediatR;
using Shared.Messages;
using Microsoft.AspNetCore.Http;

namespace Application.Courses.Commands
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCourseCommandHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = new Course
            {
                Title = request.Title,
                TeacherId = request.TeacherId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync(cancellationToken);

            // Get tenant ID from HTTP header X-Tenant-ID
            var tenantId = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-ID"].ToString() ?? string.Empty;

            var publisher = new RabbitMqPublisher();
            publisher.Publish(new CourseCreated
            {
                Id = course.Id,
                Title = course.Title,
                TenantId = tenantId  // Pass tenantId explicitly
            }, "course-created");

            return course.Id;
        }
    }
}