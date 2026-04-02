using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services.Interfaces;

public interface IModuleStatusProvider
{
    Task<IReadOnlyList<ModuleStatusRequest>> CheckModulesAsync(CancellationToken cancellationToken = default);
}
