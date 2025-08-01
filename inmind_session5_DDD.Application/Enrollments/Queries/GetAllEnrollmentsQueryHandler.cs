using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Persistence;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Enrollments.Queries;

public class GetAllEnrollmentsQueryHandler : IRequestHandler<GetAllEnrollmentsQuery, List<EnrollmentDto>>
{
    private readonly ApplicationDbContext _context;

    public GetAllEnrollmentsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EnrollmentDto>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Enrollments
            .Select(e => new EnrollmentDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                TenantId = e.TenantId
            })
            .ToListAsync(cancellationToken);
    }
}