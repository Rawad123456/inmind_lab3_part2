
using inmind_session5_DDD.Application.DTOs;
using MediatR;

namespace Application.Teachers.Queries;

public class GetTeacherByIdQuery : IRequest<TeacherDto>
{
    public int Id { get; set; }
}