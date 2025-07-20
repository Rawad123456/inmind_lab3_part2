using inmind_session5_DDD.Application.DTOs;
using MediatR;

namespace Application.Courses.Queries;

public class GetCourseByIdQuery : IRequest<CourseDto>
{
    public int Id { get; set; }
}