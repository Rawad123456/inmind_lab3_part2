using inmind_session5_DDD.Domain.Entities;
using inmind_session5_DDD.Persistence;

namespace inmind_session5_DDD.Application.Students.Commands;


using MediatR;




public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public CreateStudentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync(cancellationToken);

        return student.Id;
    }
}
