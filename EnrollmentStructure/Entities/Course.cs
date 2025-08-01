namespace EnrollmentStructure.Entities;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string TenantId { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
}