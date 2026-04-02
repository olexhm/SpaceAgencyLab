using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsSpaceAgency.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonsController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PersonRequest>>> GetPersons(CancellationToken cancellationToken)
    {
        IReadOnlyList<PersonRequest> persons = await _personService.GetAllAsync(cancellationToken);
        return Ok(persons);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonRequest>> GetPerson(int id, CancellationToken cancellationToken)
    {
        PersonRequest? person = await _personService.GetByIdAsync(id, cancellationToken);
        return person is null ? NotFound() : Ok(person);
    }

    [HttpGet("iss")]
    public async Task<ActionResult<IReadOnlyList<AstronautStatusRequest>>> SyncIssAstronauts(CancellationToken cancellationToken)
    {
        IReadOnlyList<AstronautStatusRequest> astronauts = await _personService.SyncIssAstronautsAsync(cancellationToken);
        return Ok(astronauts);
    }

    [HttpPost]
    public async Task<ActionResult<PersonRequest>> AddPerson([FromBody] PersonRequest personRequest, CancellationToken cancellationToken)
    {
        if (!IsValid(personRequest, out string? validationMessage))
        {
            return BadRequest(validationMessage);
        }

        PersonRequest savedPerson = await _personService.AddIfNotExistsAsync(personRequest, cancellationToken);
        return Ok(savedPerson);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PersonRequest>> UpdatePerson(int id, [FromBody] PersonRequest personRequest, CancellationToken cancellationToken)
    {
        if (!IsValid(personRequest, out string? validationMessage))
        {
            return BadRequest(validationMessage);
        }

        try
        {
            PersonRequest? updatedPerson = await _personService.UpdateAsync(id, personRequest, cancellationToken);
            return updatedPerson is null ? NotFound() : Ok(updatedPerson);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePerson(int id, CancellationToken cancellationToken)
    {
        bool deleted = await _personService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private static bool IsValid(PersonRequest personRequest, out string? validationMessage)
    {
        if (string.IsNullOrWhiteSpace(personRequest.Name) || string.IsNullOrWhiteSpace(personRequest.Craft))
        {
            validationMessage = "Official name and craft are required.";
            return false;
        }

        validationMessage = null;
        return true;
    }
}
