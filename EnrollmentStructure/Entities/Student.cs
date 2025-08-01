namespace EnrollmentStructure.Entities;

public class Student
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    
    public string TenantId { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
}