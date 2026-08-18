using System;
using System.Collections.Generic;

namespace TmsApi.Application.DTOs;

public record EnrollmentResponseDto(
    int Id,
    int CourseId,
    string CourseTitle,
    int StudentId,
    string StudentName,
    DateTime EnrolledAt,
    string Status,
    string Term,
    string? Notes,
    List<string> BackupCourses
);