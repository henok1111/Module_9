using System.ComponentModel.DataAnnotations;

namespace TmsApi.Dtos;

public record EnrollStudentRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "StudentId must be a positive integer.")]
    // ↑ StudentId must be 1 or higher — 0 and negative numbers are invalid
    public required int StudentId { get; init; }
}