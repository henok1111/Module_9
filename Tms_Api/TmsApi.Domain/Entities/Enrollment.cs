using System;
using System.Collections.Generic;

namespace TmsApi.Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal? Grade { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;

    // --- NEW COLUMNS TO MATCH FORM ---
    public string Term { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<string> BackupCourses { get; set; } = new();

    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}