using MediatR;

namespace Application.Courses.Commands;

public class CreateCourseCommand : IRequest<int>
{
    public string Title { get; set; }
    public int TeacherId { get; set; }
}