namespace inmind_session5_DDD.Application.DTOs;



public class StudentDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
}
