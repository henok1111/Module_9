// File: TmsApi.Application/Hubs/ITmsHubClient.cs
namespace TmsApi.Application.Hubs;

public interface ITmsHubClient
{
    Task ReceiveEnrollmentStatusUpdated(string enrollmentId, string status);
}