using lab3_inmind_part2.Models;

namespace lab3_inmind_part2.Data;
using Microsoft.EntityFrameworkCore;




public class UniversityDbContext : DbContext
{
    public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options) {}

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    
    
    
}
