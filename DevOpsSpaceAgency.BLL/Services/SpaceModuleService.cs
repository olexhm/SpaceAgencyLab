using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services;

public class SpaceModuleService : ISpaceModuleService
{
    private readonly IModuleStatusProvider _moduleStatusProvider;

    public SpaceModuleService(IModuleStatusProvider moduleStatusProvider)
    {
        _moduleStatusProvider = moduleStatusProvider;
    }

    public Task<IReadOnlyList<ModuleStatusRequest>> CheckModulesAsync(CancellationToken cancellationToken = default)
        => _moduleStatusProvider.CheckModulesAsync(cancellationToken);
}
