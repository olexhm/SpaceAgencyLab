using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services;

public class ImageService : IImageService
{
    private readonly IApodProvider _apodProvider;

    public ImageService(IApodProvider apodProvider)
    {
        _apodProvider = apodProvider;
    }

    public Task<ImageRequest> GetTodayImageAsync(CancellationToken cancellationToken = default)
        => _apodProvider.GetTodayImageAsync(cancellationToken);
}
