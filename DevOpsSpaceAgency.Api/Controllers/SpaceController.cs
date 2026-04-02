using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsSpaceAgency.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpaceController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly IPositionService _positionService;
    private readonly ISpaceModuleService _spaceModuleService;

    public SpaceController(IImageService imageService, IPositionService positionService, ISpaceModuleService spaceModuleService)
    {
        _imageService = imageService;
        _positionService = positionService;
        _spaceModuleService = spaceModuleService;
    }

    [HttpGet("iss-position")]
    public async Task<ActionResult<IssPositionRequest>> GetIssPosition(CancellationToken cancellationToken)
        => Ok(await _positionService.GetCurrentPositionAsync(cancellationToken));

    [HttpGet("apod")]
    public async Task<ActionResult<ImageRequest>> GetAstronomyPictureOfTheDay(CancellationToken cancellationToken)
        => Ok(await _imageService.GetTodayImageAsync(cancellationToken));

    [HttpGet("modules/status")]
    public async Task<ActionResult<IReadOnlyList<ModuleStatusRequest>>> CheckModules(CancellationToken cancellationToken)
        => Ok(await _spaceModuleService.CheckModulesAsync(cancellationToken));
}
