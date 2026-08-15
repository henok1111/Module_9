using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TmsApi.Application.Interfaces;
using TmsApi.Application.DTOs;

namespace TmsApi.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/courses/{courseId:int}/enrollments")]
[ApiVersion("2.0")]
public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Enroll(int courseId, [FromBody] EnrollStudentRequest request, CancellationToken ct)
    {
        var result = await enrollmentService.CreateAsync(courseId, request, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { courseId = courseId, id = result.Id }, // Assuming EnrollmentResponseDto has an Id property
            result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int courseId, int id, CancellationToken ct)
    {
        var enrollment = await enrollmentService.GetByIdAsync(courseId, id, ct);
        if (enrollment is null)
        {
            return NotFound();
        }

        return Ok(enrollment);
    }

    [HttpGet]
    public async Task<IActionResult> GetByCourse(int courseId, CancellationToken ct)
    {
        var enrollments = await enrollmentService.GetByCourseAsync(courseId, ct);
        return Ok(enrollments);
    }
}