using Microsoft.EntityFrameworkCore;
using TmsApi.Application.DTOs;
using TmsApi.Application.Interfaces;
using TmsApi.Domain.Entities;
using TmsApi.Infrastructure.Persistence;

namespace TmsApi.Infrastructure.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly TmsDbContext _context;

    public EnrollmentService(TmsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(int studentId, string courseCode, CancellationToken ct = default)
    {
        return await _context.Enrollments
            .AnyAsync(e => e.StudentId == studentId && e.Course.Code == courseCode, ct);
    }

    public async Task AddAsync(Enrollment enrollment, CancellationToken ct = default)
    {
        await _context.Enrollments.AddAsync(enrollment, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<EnrollmentResponseDto> CreateAsync(int courseId, EnrollStudentRequest request, CancellationToken ct = default)
    {
        var course = await _context.Courses.FindAsync([courseId], ct)
            ?? throw new InvalidOperationException($"Course {courseId} was not found.");

        var student = await _context.Students.FindAsync([request.StudentId], ct)
            ?? throw new InvalidOperationException($"Student {request.StudentId} was not found.");

        var alreadyEnrolled = await _context.Enrollments
            .AnyAsync(e => e.CourseId == courseId && e.StudentId == request.StudentId, ct);

        if (alreadyEnrolled)
        {
            throw new InvalidOperationException("This student is already enrolled in this course.");
        }

        var enrollment = new Enrollment
        {
            CourseId = courseId,
            StudentId = request.StudentId,
            Term = request.Term,
            Notes = request.Notes,
            BackupCourses = request.BackupCourses ?? new List<string>(),
            EnrolledAt = DateTime.UtcNow,
            Status = "Pending",
        };

        await _context.Enrollments.AddAsync(enrollment, ct);
        await _context.SaveChangesAsync(ct);

        return new EnrollmentResponseDto(
            enrollment.Id, enrollment.CourseId, course.Title,
            enrollment.StudentId, student.Name,
            enrollment.EnrolledAt, enrollment.Status.ToString(),
            enrollment.Term, enrollment.Notes, enrollment.BackupCourses);
    }

    public async Task<EnrollmentResponseDto?> GetByIdAsync(int courseId, int id, CancellationToken ct = default)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId && e.Id == id)
            .Select(e => new EnrollmentResponseDto(
                e.Id, e.CourseId, e.Course.Title, e.StudentId, e.Student.Name,
                e.EnrolledAt, e.Status.ToString(),
                e.Term, e.Notes, e.BackupCourses))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<EnrollmentResponseDto>> GetByCourseAsync(int courseId, CancellationToken ct = default)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId)
            .Select(e => new EnrollmentResponseDto(
                e.Id, e.CourseId, e.Course.Title, e.StudentId, e.Student.Name,
                e.EnrolledAt, e.Status.ToString(),
                e.Term, e.Notes, e.BackupCourses))
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Select(e => new EnrollmentResponseDto(
                e.Id, e.CourseId, e.Course.Title, e.StudentId, e.Student.Name,
                e.EnrolledAt, e.Status.ToString(),
                e.Term, e.Notes, e.BackupCourses))
            .ToListAsync(ct);
    }

    public async Task<EnrollmentResponseDto?> ApproveAsync(int id, CancellationToken ct = default)
    {
        var enrollment = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (enrollment is null)
        {
            return null;
        }

        enrollment.Status = "Approved";
        await _context.SaveChangesAsync(ct);

        return new EnrollmentResponseDto(
            enrollment.Id, enrollment.CourseId, enrollment.Course.Title,
            enrollment.StudentId, enrollment.Student.Name,
            enrollment.EnrolledAt, enrollment.Status.ToString(),
            enrollment.Term, enrollment.Notes, enrollment.BackupCourses);
    }
}