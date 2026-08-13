using Microsoft.EntityFrameworkCore;
using TmsApi.Data; // Ensure this is your actual DbContext namespace
using TmsApi.Dtos;

namespace TmsApi.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly TmsDbContext _context; // Match your real DbContext class name here

    public EnrollmentService(TmsDbContext context)
    {
        _context = context;
    }

    // FIX CS0738: Note the '?' after EnrollmentResponseDto to allow nulls
    public async Task<EnrollmentResponseDto?> GetByIdAsync(int courseId, int id, CancellationToken ct)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId && e.Id == id)
            .Select(e => new EnrollmentResponseDto(e.Id, e.CourseId, e.StudentId, e.EnrolledAt)) 
            .FirstOrDefaultAsync(ct); 
    }

    // FIX CS0535: Ensure parameters match IEnrollmentService exactly
    public async Task<EnrollmentResponseDto> CreateAsync(int courseId, EnrollStudentRequest request, CancellationToken ct)
    {
        // Replace this throw with your actual enrollment creation code
        throw new NotImplementedException();
    }

    // FIX CS0738: Ensure return type is strictly Task<List<EnrollmentResponseDto>>
    public async Task<List<EnrollmentResponseDto>> GetByCourseAsync(int courseId, CancellationToken ct)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId)
            .Select(e => new EnrollmentResponseDto(e.Id, e.CourseId, e.StudentId, e.EnrolledAt))
            .ToListAsync(ct);
    }
}