using inmind_session5_DDD.Domain.Interfaces;

namespace inmind_session5_DDD.Domain.Entities;

public class Course: ITenantEntity
{
    public int Id { get; set; }
    public string Title { get; set; }

    public string TenantId { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}