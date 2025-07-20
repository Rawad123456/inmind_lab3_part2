using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Queries;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<CourseDto>>
{
    private readonly ApplicationDbContext _context;

    public GetAllCoursesQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _context.Courses.AsNoTracking().ToListAsync(cancellationToken);

        return courses.Select(c => new CourseDto
        {
            Id = c.Id,
            Title = c.Title
        }).ToList();
    }
}