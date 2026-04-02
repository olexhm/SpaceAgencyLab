using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services.Interfaces;

public interface IIssPositionProvider
{
    Task<IssPositionRequest> GetCurrentPositionAsync(CancellationToken cancellationToken = default);
}
