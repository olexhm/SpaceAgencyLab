using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services.Interfaces;

public interface IPersonService
{
    Task<PersonRequest> AddIfNotExistsAsync(PersonRequest personRequest, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PersonRequest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PersonRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PersonRequest?> UpdateAsync(int id, PersonRequest personRequest, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AstronautStatusRequest>> SyncIssAstronautsAsync(CancellationToken cancellationToken = default);
}
