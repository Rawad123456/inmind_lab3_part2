namespace inmind_session5_DDD.Application.Students.Commands;

using MediatR;



public class CreateStudentCommand : IRequest<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
