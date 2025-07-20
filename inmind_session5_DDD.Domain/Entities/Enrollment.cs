namespace inmind_session5_DDD.Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }

    public Guid StudentId { get; set; }
    public Student Student { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }
}