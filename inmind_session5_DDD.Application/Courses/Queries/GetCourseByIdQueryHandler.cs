using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Queries;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    private readonly ApplicationDbContext _context;

    public GetCourseByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
            throw new KeyNotFoundException($"Course with ID {request.Id} not found.");

        return new CourseDto
        {
            Id = course.Id,
            Title = course.Title
        };
    }
}