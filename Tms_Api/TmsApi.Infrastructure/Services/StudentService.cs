using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TmsApi.Application.DTOs;
using TmsApi.Application.Interfaces;
using TmsApi.Infrastructure.Persistence;

namespace TmsApi.Infrastructure.Services;

public class StudentService(TmsDbContext context) : IStudentService
{
    public async Task<IEnumerable<StudentOptionDto>> GetActiveStudentsAsync(CancellationToken ct = default)
    {
        return await context.Students
            .Where(s => s.IsActive)
            .Select(s => new StudentOptionDto(s.Id, s.RegistrationNumber, s.Name))
            .ToListAsync(ct);
    }
}