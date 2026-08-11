using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Dtos;
using TmsApi.Entities;

namespace TmsApi.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly TmsDbContext _context;

    public EnrollmentService(TmsDbContext context)
    {
        _context = context;
    }

    public async Task<EnrollmentResponseDto?> GetByIdAsync(int courseId, int id, CancellationToken ct)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId && e.Id == id)
            .Select(e => new EnrollmentResponseDto(
                e.Id, e.CourseId, e.Course.Title, e.StudentId, e.Student.Name,
                e.EnrolledAt, e.Status.ToString()))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<EnrollmentResponseDto> CreateAsync(int courseId, EnrollStudentRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<List<EnrollmentResponseDto>> GetByCourseAsync(int courseId, CancellationToken ct)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId)
            .Select(e => new EnrollmentResponseDto(
                e.Id, e.CourseId, e.Course.Title, e.StudentId, e.Student.Name,
                e.EnrolledAt, e.Status.ToString()))
            .ToListAsync(ct);
    }

    public async Task<List<EnrollmentResponseDto>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Select(e => new EnrollmentResponseDto(
                e.Id, e.CourseId, e.Course.Title, e.StudentId, e.Student.Name,
                e.EnrolledAt, e.Status.ToString()))
            .ToListAsync(ct);
    }

    public async Task<EnrollmentResponseDto?> ApproveAsync(int id, CancellationToken ct)
    {
        var enrollment = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (enrollment is null)
        {
            return null;
        }

        enrollment.Status = EnrollmentStatus.Approved;
        await _context.SaveChangesAsync(ct);

        return new EnrollmentResponseDto(
            enrollment.Id, enrollment.CourseId, enrollment.Course.Title,
            enrollment.StudentId, enrollment.Student.Name,
            enrollment.EnrolledAt, enrollment.Status.ToString());
    }
}