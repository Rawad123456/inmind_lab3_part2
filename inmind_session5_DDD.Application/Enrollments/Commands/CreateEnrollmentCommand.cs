using MediatR;

namespace Application.Enrollments.Commands;

public class CreateEnrollmentCommand : IRequest<int>
{
    public Guid StudentId { get; set; }
    public int CourseId { get; set; }
}