using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Common.Exceptions;
using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace inmind_session5_DDD.Application.Students.Queries
{
    public class GetStudentByIdQuery : IRequest<StudentDto>
    {
        public Guid Id { get; set; }

        public GetStudentByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetStudentByIdQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (student == null)
            {
                throw new NotFoundException($"Student with ID {request.Id} not found.");
            }

            return new StudentDto
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email
            };
        }
    }
}