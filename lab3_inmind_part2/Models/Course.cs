namespace lab3_inmind_part2.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}