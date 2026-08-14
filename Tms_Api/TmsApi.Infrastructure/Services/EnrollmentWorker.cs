using Microsoft.Extensions.DependencyInjection;
using TmsApi.Application.Interfaces;
// ↑ This is the only change needed
// Tells the compiler: "IEnrollmentService lives in TmsApi.Services namespace"
namespace TmsApi.Infrastructure.Services;
public class EnrollmentWorker(IServiceScopeFactory scopeFactory)
{
    public void ProcessBatch()
    {
        using var scope = scopeFactory.CreateScope();
        var svc = scope.ServiceProvider.GetRequiredService<IEnrollmentService>();
        // Now it finds Services/IEnrollmentService.cs correctly
    }
}