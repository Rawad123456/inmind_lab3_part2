namespace lab3_inmind_part2.Models;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}