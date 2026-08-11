using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Dtos;

namespace TmsApi.Services;

public class CourseService : ICourseService
{
    private readonly TmsDbContext _context;

    public CourseService(TmsDbContext context)
    {
        _context = context;
    }


    public async Task<PagedResponse<CourseResponseDto>> GetCoursesAsync(
        PagedRequest request,
        CancellationToken ct = default)
    {
        var query = _context.Courses.AsQueryable();

        var totalCount = await query.CountAsync(ct);

        var courses = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Code = c.Code,
                Title = c.Title,
                MaxCapacity = c.MaxCapacity
            })
            .ToListAsync(ct);

        return new PagedResponse<CourseResponseDto>
        {
            Items = courses,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
public async Task<bool> CodeExistsAsync(
    string code,
    CancellationToken ct = default)
{
    return await _context.Courses
        .AnyAsync(c => c.Code == code, ct);
}

    public async Task<CourseResponseDto?> GetCourseByIdAsync(
        int id,
        CancellationToken ct = default)
    {
        return await _context.Courses
            .Where(c => c.Id == id)
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Code = c.Code,
                Title = c.Title,
                MaxCapacity = c.MaxCapacity
            })
            .FirstOrDefaultAsync(ct);
    }


    public async Task<CourseResponseDto?> GetByIdAsync(
        int id,
        CancellationToken ct = default)
    {
        return await _context.Courses
            .Where(c => c.Id == id)
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Code = c.Code,
                Title = c.Title,
                MaxCapacity = c.MaxCapacity
            })
            .FirstOrDefaultAsync(ct);
    }


    public async Task<CourseResponseDto> CreateAsync(
        CreateCourseRequest request,
        CancellationToken ct = default)
    {
        var course = new Entities.Course
        {
            Code = request.Code,
            Title = request.Title,
            MaxCapacity = request.MaxCapacity
        };

        _context.Courses.Add(course);

        await _context.SaveChangesAsync(ct);

        return new CourseResponseDto
        {
            Id = course.Id,
            Code = course.Code,
            Title = course.Title,
            MaxCapacity = course.MaxCapacity
        };
    }
}