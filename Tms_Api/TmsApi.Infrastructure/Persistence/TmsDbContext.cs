using Microsoft.EntityFrameworkCore;
using TmsApi.Domain.Entities;

namespace TmsApi.Infrastructure.Persistence;

public class TmsDbContext : DbContext
{
    public TmsDbContext(DbContextOptions<TmsDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Enrollment>(entity =>
        {
            // Default value for existing rows in 'Term'
            entity.Property(e => e.Term)
                .HasDefaultValue("Fall 2026");

            // Native Postgres array default value without JSON conversion
            entity.Property(e => e.BackupCourses)
                .HasDefaultValueSql("'{}'::text[]");
        });
    }
}