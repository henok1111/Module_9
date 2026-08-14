
using TmsApi.Application.DTOs;

namespace TmsApi.Application.Interfaces;

public interface ICourseService
{
    Task<PagedResponse<CourseResponseDto>> GetCoursesAsync(
        PagedRequest request,
        CancellationToken ct = default);

    Task<CourseResponseDto?> GetByIdAsync(
        int id,
        CancellationToken ct = default);

    Task<bool> CodeExistsAsync(
        string code,
        CancellationToken ct = default);

    Task<CourseResponseDto> CreateAsync(
        CreateCourseRequest request,
        CancellationToken ct = default);
}