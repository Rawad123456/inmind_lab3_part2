using inmind_session5_DDD.Domain.Entities;
using inmind_session5_DDD.Persistence;

using MediatR;

namespace Application.Enrollments.Commands;

public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, int>
{
    private readonly ApplicationDbContext _context;

    public CreateEnrollmentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = new Enrollment
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync(cancellationToken);

        return enrollment.Id;
    }
}