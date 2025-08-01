using Microsoft.EntityFrameworkCore;
using EnrollmentStructure.Entities;
using EnrollmentStructure.Tenant;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EnrollmentStructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            var tenantId = _tenantProvider.GetTenantId();
          
            
            modelBuilder.Entity<Student>().HasQueryFilter(s => s.TenantId == _tenantProvider.GetTenantId());
            modelBuilder.Entity<Course>().HasQueryFilter(c => c.TenantId == _tenantProvider.GetTenantId());
            modelBuilder.Entity<Enrollment>().HasQueryFilter(e => e.TenantId == _tenantProvider.GetTenantId());



            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var tenantId = _tenantProvider.GetTenantId();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    var hasTenantId = entry.Entity.GetType().GetProperty("TenantId");
                    if (hasTenantId != null)
                    {
                        var currentValue = hasTenantId.GetValue(entry.Entity) as string;
                        if (string.IsNullOrWhiteSpace(currentValue))
                        {
                            hasTenantId.SetValue(entry.Entity, tenantId);
                        }
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

      
    }
}
