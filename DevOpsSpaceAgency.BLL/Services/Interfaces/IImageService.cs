using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services.Interfaces;

public interface IImageService
{
    Task<ImageRequest> GetTodayImageAsync(CancellationToken cancellationToken = default);
}
