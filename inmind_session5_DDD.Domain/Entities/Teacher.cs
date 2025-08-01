using inmind_session5_DDD.Domain.Interfaces;

namespace inmind_session5_DDD.Domain.Entities;

public class Teacher: ITenantEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string TenantId { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}