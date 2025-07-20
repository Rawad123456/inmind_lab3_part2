
using inmind_session5_DDD.Application.DTOs;
using MediatR;

namespace Application.Teachers.Queries;

public partial class GetAllTeachersQuery : IRequest<List<TeacherDto>>
{
}