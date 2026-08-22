using System;
using System.Collections.Generic;
using TmsApi.Domain.Entities.Enum;

namespace TmsApi.Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal? Grade { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = EnrollmentStatus.Pending;

    public string Term { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<string> BackupCourses { get; set; } = new();

    // Navigation properties
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}