using inmind_session5_DDD.Domain.Entities;
using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace inmind_session5_DDD.Application.Students.Commands;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Guid>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CreateStudentCommandHandler> _logger;

    public CreateStudentCommandHandler(ApplicationDbContext context, ILogger<CreateStudentCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
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

        // Logging after save
        _logger.LogInformation("Student created with ID {StudentId} and Name {FullName}", student.Id, student.FullName);

        return student.Id;
    }
}