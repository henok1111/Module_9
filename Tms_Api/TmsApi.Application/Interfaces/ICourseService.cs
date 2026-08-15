using TmsApi.Application.DTOs;
using TmsApi.Domain.Entities;

namespace TmsApi.Application.Interfaces;

public interface ICourseService
{
    // Expected by EnrollStudentHandler
    Task<Course?> GetByCodeAsync(string code, CancellationToken ct = default);

    // Updated to accept PagedRequest and return PagedResponse<CourseResponseDto>
    Task<PagedResponse<CourseResponseDto>> GetCoursesAsync(PagedRequest request, CancellationToken ct = default);

    // Standard CRUD
    Task<Course?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Course course, CancellationToken ct = default);
    Task UpdateAsync(Course course, CancellationToken ct = default);
    Task<bool> CodeExistsAsync(string code, CancellationToken ct = default);
    Task<CourseResponseDto> CreateAsync(CreateCourseRequest request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}