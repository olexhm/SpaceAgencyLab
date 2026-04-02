using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services.Interfaces;

public interface IApodProvider
{
    Task<ImageRequest> GetTodayImageAsync(CancellationToken cancellationToken = default);
}
