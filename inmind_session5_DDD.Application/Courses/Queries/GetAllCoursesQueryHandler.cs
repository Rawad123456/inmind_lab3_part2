using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Common.Extensions;

namespace Application.Courses.Queries;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<CourseDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllCoursesQueryHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _httpContextAccessor.HttpContext?.GetTenantId();

        var query = _context.Courses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            query = query.Where(c => c.TenantId == tenantId);
        }

        var courses = await query.ToListAsync(cancellationToken);

        return courses.Select(c => new CourseDto
        {
            Id = c.Id,
            Title = c.Title,
            TenantId = c.TenantId
        }).ToList();
    }
}