using MediatR;

namespace Application.Teachers.Commands
{
    public class CreateTeacherCommand : IRequest<int>
    {
        public string Name { get; set; }
    }
}