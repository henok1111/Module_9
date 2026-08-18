using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TmsApi.Application.DTOs;

public record EnrollStudentRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "StudentId must be a positive integer.")]
    public required int StudentId { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "CourseId must be a positive integer.")]
    public required int CourseId { get; init; }

    [Required(ErrorMessage = "Term is required.")]
    public required string Term { get; init; }

    public string? Notes { get; init; }

    public List<string> BackupCourses { get; init; } = new();
}