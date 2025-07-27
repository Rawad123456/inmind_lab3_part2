namespace inmind_session5_DDD.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public string? ProfileImageUrl { get; set; }

}