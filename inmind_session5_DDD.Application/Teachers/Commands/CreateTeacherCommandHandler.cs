using Application.Teachers.Commands;
using inmind_session5_DDD.Domain.Entities;
using inmind_session5_DDD.Persistence;
using MediatR;

namespace inmind_session5_DDD.Application.Teachers.Commands
{
    public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateTeacherCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = new Teacher
            {
                Name = request.Name
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync(cancellationToken);

            return teacher.Id; // Auto-incremented int
        }
    }
}