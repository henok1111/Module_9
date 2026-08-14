using Microsoft.EntityFrameworkCore;
using TmsApi.Domain.Entities;
using TmsApi.Infrastructure.Persistence;
namespace TmsApi.Infrastructure.Persistence;
public class TmsDbContext(DbContextOptions<TmsDbContext> options) : DbContext(options)
{public DbSet<Student> Students => Set<Student>();
public DbSet<Course> Courses => Set<Course>();
public DbSet<Enrollment> Enrollments => Set<Enrollment>();
}
