using Microsoft.EntityFrameworkCore;
using TmsApi.Application.DTOs;
using TmsApi.Application.Interfaces;
using TmsApi.Domain.Entities;
using TmsApi.Infrastructure.Persistence;

namespace TmsApi.Infrastructure.Services;

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

    // Required by ICourseService (returns entity Course instead of DTO)
    public async Task<Course?> GetByIdAsync(
        int id,
        CancellationToken ct = default)
    {
        return await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    // Required by ICourseService
    public async Task<Course?> GetByCodeAsync(
        string code,
        CancellationToken ct = default)
    {
        return await _context.Courses
            .FirstOrDefaultAsync(c => c.Code == code, ct);
    }

    // Required by ICourseService
    public async Task AddAsync(
        Course course,
        CancellationToken ct = default)
    {
        await _context.Courses.AddAsync(course, ct);
        await _context.SaveChangesAsync(ct);
    }

    // Required by ICourseService
    public async Task UpdateAsync(
        Course course,
        CancellationToken ct = default)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync(ct);
    }

    // Required by ICourseService
    public async Task DeleteAsync(
        int id,
        CancellationToken ct = default)
    {
        var course = await _context.Courses.FindAsync(new object[] { id }, ct);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<CourseResponseDto> CreateAsync(
        CreateCourseRequest request,
        CancellationToken ct = default)
    {
        var course = new Course
        {
            Code = request.Code,
            Title = request.Title,
            MaxCapacity = request.MaxCapacity,
            

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