using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services.Interfaces;

public interface IIssAstronautProvider
{
    Task<IReadOnlyList<PersonRequest>> GetCurrentIssAstronautsAsync(CancellationToken cancellationToken = default);
}
