namespace inmind_session5_DDD.Domain.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}