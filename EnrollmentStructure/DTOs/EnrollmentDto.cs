namespace EnrollmentStructure.DTOs;

public class EnrollmentDto
{
    public int Id { get; set; }
    public Guid StudentId { get; set; }
    public int CourseId { get; set; }
    public string TenantId { get; set; } = null!;
}