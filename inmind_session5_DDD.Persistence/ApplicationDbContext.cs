
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using inmind_session5_DDD.Common.Tenant;
using inmind_session5_DDD.Domain.Entities;

namespace inmind_session5_DDD.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Use EF's built-in feature for global filters
            modelBuilder.Entity<Student>().HasQueryFilter(s =>
                _tenantProvider.TenantId == null || s.TenantId == _tenantProvider.TenantId);

            modelBuilder.Entity<Teacher>().HasQueryFilter(t =>
                _tenantProvider.TenantId == null || t.TenantId == _tenantProvider.TenantId);

            modelBuilder.Entity<Course>().HasQueryFilter(c =>
                _tenantProvider.TenantId == null || c.TenantId == _tenantProvider.TenantId);

            modelBuilder.Entity<Enrollment>().HasQueryFilter(e =>
                _tenantProvider.TenantId == null || e.TenantId == _tenantProvider.TenantId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(_tenantProvider.TenantId))
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    if (entry.State == EntityState.Added)
                    {
                        var property = entry.Entity.GetType().GetProperty("TenantId", BindingFlags.Public | BindingFlags.Instance);

                        if (property != null && property.PropertyType == typeof(string))
                        {
                            var currentValue = property.GetValue(entry.Entity) as string;

                            if (string.IsNullOrWhiteSpace(currentValue))
                            {
                                property.SetValue(entry.Entity, _tenantProvider.TenantId);
                            }
                        }
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
