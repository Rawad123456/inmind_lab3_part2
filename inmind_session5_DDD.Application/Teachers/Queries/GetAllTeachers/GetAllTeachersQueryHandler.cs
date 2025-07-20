using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries
{
    public partial class GetAllTeachersQuery : IRequest<List<TeacherDto>> {}

    public class GetAllTeachersQueryHandler : IRequestHandler<GetAllTeachersQuery, List<TeacherDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllTeachersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeacherDto>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
        {
            var teachers = await _context.Teachers.AsNoTracking().ToListAsync(cancellationToken);

            var teacherDtos = teachers.Select(t => new TeacherDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();

            return teacherDtos;
        }
    }
}