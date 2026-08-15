using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TmsApi.Api.Hubs;
using TmsApi.Application.Hubs;
using TmsApi.Application.Interfaces;

namespace TmsApi.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/enrollments")]
[ApiVersion("2.0")]
public class EnrollmentsFlatController(
    IEnrollmentService enrollmentService,
    IHubContext<TmsHub, ITmsHubClient> hubContext,
    ILogger<EnrollmentsFlatController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var enrollments = await enrollmentService.GetAllAsync(ct);
        return Ok(enrollments);
    }

    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id, CancellationToken ct)
    {
        var updated = await enrollmentService.ApproveAsync(id, ct);
        if (updated is null)
        {
            return NotFound();
        }

        logger.LogInformation("Broadcasting enrollment approval for ID: {Id}", id);

        // Broadcast status update live to all connected clients
        await hubContext.Clients.All
            .ReceiveEnrollmentStatusUpdated(id.ToString(), "Approved");

        return Ok(updated);
    }
}