using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services.Interfaces;

public interface ISpaceModuleService
{
    Task<IReadOnlyList<ModuleStatusRequest>> CheckModulesAsync(CancellationToken cancellationToken = default);
}
