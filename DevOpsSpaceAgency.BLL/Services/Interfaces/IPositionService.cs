using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services.Interfaces;

public interface IPositionService
{
    Task<IssPositionRequest> GetCurrentPositionAsync(CancellationToken cancellationToken = default);
}
