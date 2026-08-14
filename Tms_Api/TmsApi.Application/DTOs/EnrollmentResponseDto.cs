namespace TmsApi.Application.DTOs;

// What the client sees after enrolling a student
// Clean — no navigation properties, no internal fields
public record EnrollmentResponseDto(
    int Id,
    int CourseId,
    int StudentId,
    DateTime EnrolledAt);