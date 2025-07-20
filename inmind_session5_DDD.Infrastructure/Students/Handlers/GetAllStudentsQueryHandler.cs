using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Application.Students.Queries;
using inmind_session5_DDD.Common.Exceptions;
using MediatR;

namespace inmind_session5_DDD.Infrastructure.Students.Handlers;


using inmind_session5_DDD.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using inmind_session5_DDD.Persistence;



public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentDto>>
{
    private readonly ApplicationDbContext _context;

    public GetAllStudentsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await _context.Students.ToListAsync(cancellationToken);

        
        
        return students.Select(s => new StudentDto
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email
        }).ToList();
    }
}
