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
            // Configure Enrollment primary key and table
            entity.HasKey(e => e.Id);

            // Status is a standard string
            entity.Property(e => e.Status)
                  .HasMaxLength(50)
                  .IsRequired();

            // Term with default value
            entity.Property(e => e.Term)
                  .HasMaxLength(50)
                  .HasDefaultValue("Fall 2026");

            // Optional Notes
            entity.Property(e => e.Notes)
                  .HasMaxLength(500);

            // Native Postgres array mapping for List<string>
            // Note: EF Core Npgsql provider handles List<string> to text[] automatically
            entity.Property(e => e.BackupCourses)
                  .HasDefaultValueSql("'{}'::text[]");

            // Configure Relationships
            entity.HasOne(e => e.Student)
                  .WithMany()
                  .HasForeignKey(e => e.StudentId);

            entity.HasOne(e => e.Course)
                  .WithMany()
                  .HasForeignKey(e => e.CourseId);
        });
    }
}