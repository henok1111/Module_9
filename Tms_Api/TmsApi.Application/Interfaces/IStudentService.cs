using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TmsApi.Application.DTOs;

namespace TmsApi.Application.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentOptionDto>> GetActiveStudentsAsync(CancellationToken ct = default);
}