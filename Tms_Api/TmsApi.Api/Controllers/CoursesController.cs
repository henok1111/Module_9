using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TmsApi.Application.DTOs;
using TmsApi.Application.Interfaces;
using TmsApi.Infrastructure.Services;

namespace TmsApi.Api;

[ApiController]
[Route("api/courses")]
[Tags("Courses")] // Groups all endpoints inside this controller under "Courses" in Scalar
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class CoursesController(
    ICourseService courseService,
    LinkGenerator linkGenerator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<CourseResponseDto>), StatusCodes.Status200OK)]
    [EndpointSummary("List courses with pagination")]
    [EndpointDescription("Returns a paginated, optionally filtered list of TMS courses. PageSize is capped at 50.")]
    public async Task<IActionResult> GetCourses(
        [FromQuery] PagedRequest request,
        CancellationToken ct)
    {
        var result = await courseService.GetCoursesAsync(request, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = nameof(GetCourseById))]
    [ProducesResponseType(typeof(CourseDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Get a course by ID")]
    [EndpointDescription("Returns course details with HATEOAS links. Returns 404 if the course does not exist.")]
    public async Task<ActionResult<CourseDetailDto>> GetCourseById(int id, CancellationToken ct)
    {
        // 1. Fetch the course from the database and handle null verification
        var course = await courseService.GetByIdAsync(id, ct);
        if (course == null)
        {
            return NotFound();
        }

        // 2. Generate URI paths safely via LinkGenerator
        var selfUrl = linkGenerator.GetPathByName(HttpContext, nameof(GetCourseById), new { id }) ?? "";
        var enrollmentsUrl = linkGenerator.GetPathByAction(
            HttpContext,
            action: "GetEnrollments",
            controller: "Enrollments",
            values: new { courseId = id }
        ) ?? "";

        // 3. Build HATEOAS links using the exact constructor positioning: (Href, Rel, Method)
        var links = new List<LinkDto>
        {
            new LinkDto(selfUrl, "self", "GET"),
            new LinkDto(selfUrl, "update", "PUT"),
            new LinkDto(selfUrl, "delete", "DELETE"),
            new LinkDto(enrollmentsUrl, "enrollments", "GET")
        };

        // 4. Dynamic Business Logic Rule: Add the 5th link conditionally
        if (course.EnrollmentCount < course.MaxCapacity)
        {
            links.Add(new LinkDto(enrollmentsUrl, "enroll", "POST"));
        }

        // 5. Construct and return mapped target CourseDetailDto
        var response = new CourseDetailDto
        {
            Id = course.Id,
            Code = course.Code,
            Title = course.Title,
            MaxCapacity = course.MaxCapacity,
            EnrollmentCount = course.EnrollmentCount,
            Links = links.AsReadOnly()
        };

        return Ok(response);
    }

    [HttpPost]
    [EndpointSummary("Create a new course")]
    [EndpointDescription(
        "Creates a course with a unique code. Validates incoming payload. Returns 409 if the course code already exists.")]
    [ProducesResponseType(typeof(CourseResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)] // Explicit schema mapping for validation errors in Scalar
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCourse(
        [FromBody] CreateCourseRequest request,
        CancellationToken ct)
    {
        if (await courseService.CodeExistsAsync(request.Code, ct))
        {
            return Conflict(new ProblemDetails
            {
                Title = "Course code already exists",
                Detail = $"A course with code '{request.Code}' is already registered.",
                Status = StatusCodes.Status409Conflict
            });
        }

        var result = await courseService.CreateAsync(request, ct);

        return CreatedAtAction(
            nameof(GetCourseById),
            new { id = result.Id },
            result);
    }
}