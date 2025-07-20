namespace inmind_session5_DDD.Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}