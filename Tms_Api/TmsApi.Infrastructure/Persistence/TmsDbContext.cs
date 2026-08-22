using Microsoft.EntityFrameworkCore;
using TmsApi.Domain.Entities;

namespace TmsApi.Infrastructure.Persistence;

public class TmsDbContext : DbContext
{
    public TmsDbContext(DbContextOptions<TmsDbContext> options) : base(options)
    {
    }

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
            entity.Property(e => e.BackupCourses)
                .HasDefaultValueSql("'{}'::text[]");

            // Configure Relationships (FIXED: Explicitly bind navigation collections)
            entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}