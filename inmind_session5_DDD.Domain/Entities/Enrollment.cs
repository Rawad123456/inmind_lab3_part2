using inmind_session5_DDD.Domain.Interfaces;

namespace inmind_session5_DDD.Domain.Entities;

public class Enrollment: ITenantEntity
{
    public int Id { get; set; }

    public Guid StudentId { get; set; }
    public Student Student { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public Course Course { get; set; }
}