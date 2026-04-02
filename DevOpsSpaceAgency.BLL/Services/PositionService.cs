using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services;

public class PositionService : IPositionService
{
    private readonly IIssPositionProvider _issPositionProvider;

    public PositionService(IIssPositionProvider issPositionProvider)
    {
        _issPositionProvider = issPositionProvider;
    }

    public Task<IssPositionRequest> GetCurrentPositionAsync(CancellationToken cancellationToken = default)
        => _issPositionProvider.GetCurrentPositionAsync(cancellationToken);
}
