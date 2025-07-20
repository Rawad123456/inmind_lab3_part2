using Application.Teachers.Queries;
using inmind_session5_DDD.Application.DTOs;
using inmind_session5_DDD.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Handlers;

public class GetTeacherByIdQueryHandler : IRequestHandler<GetTeacherByIdQuery, TeacherDto>
{
    private readonly ApplicationDbContext _context;

    public GetTeacherByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TeacherDto> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
    {
        var teacher = await _context.Teachers
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (teacher == null)
        {
            throw new KeyNotFoundException($"Teacher with ID {request.Id} not found.");
        }

        return new TeacherDto
        {
            Id = teacher.Id,
            Name = teacher.Name
        };
    }
}