using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TmsApi.Application.DTOs;
using TmsApi.Application.Interfaces;

namespace TmsApi.Controllers.V2;

[ApiController]
[Route("api/v{version:apiVersion}/courses")]
[ApiVersion("2.0")]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<CourseResponseDto>), StatusCodes.Status200OK)]
    [EndpointSummary("List courses with pagination")]
    [EndpointDescription("Returns a paginated, optionally filtered list of TMS courses. PageSize is capped at 50.")]
    public async Task<IActionResult> GetCourses(
        [FromQuery] PagedRequest request,
        CancellationToken ct)
    {
        // Fixed: Called GetCoursesAsync and passed both 'request' and 'ct'
        var result = await courseService.GetCoursesAsync(request, ct);
        return Ok(result);
    }
}

[ApiController]
[Route("api/v{version:apiVersion}/transcripts")]
[ApiVersion("2.0")]
public class TranscriptsController : ControllerBase
{
    [HttpPost]
    [EnableRateLimiting("transcripts")]
    public IActionResult RequestTranscript([FromBody] object? _)
    {
        // Stub: Exercise 5 swaps this for enqueue + 202 + Location.
        return Ok();
    }
}