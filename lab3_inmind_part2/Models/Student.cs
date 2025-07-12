namespace lab3_inmind_part2.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}