using TmsApi.Application.DTOs;
using TmsApi.Domain.Entities;

namespace TmsApi.Application.Interfaces;

public interface IEnrollmentService
{
    Task<bool> ExistsAsync(int studentId, string courseCode, CancellationToken ct = default);
    Task AddAsync(Enrollment enrollment, CancellationToken ct = default);

    Task<EnrollmentResponseDto> CreateAsync(int courseId, EnrollStudentRequest request, CancellationToken ct = default);
    Task<EnrollmentResponseDto?> GetByIdAsync(int courseId, int id, CancellationToken ct = default);
    Task<IEnumerable<EnrollmentResponseDto>> GetByCourseAsync(int courseId, CancellationToken ct = default);

    Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync(CancellationToken ct = default);
    Task<EnrollmentResponseDto?> ApproveAsync(int id, CancellationToken ct = default);
}