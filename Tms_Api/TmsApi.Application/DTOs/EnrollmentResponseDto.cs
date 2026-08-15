namespace TmsApi.Application.DTOs;

public record EnrollmentResponseDto(
    int Id,
    int CourseId,
    string CourseName,
    int StudentId,
    string StudentName,
    DateTime EnrolledAt,
    string Status);