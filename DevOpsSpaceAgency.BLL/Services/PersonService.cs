using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.DAL.Entities;
using DevOpsSpaceAgency.DAL.Repositories.Interfaces;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.BLL.Services;

public class PersonService : IPersonService
{
    private readonly IIssAstronautProvider _issAstronautProvider;
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository, IIssAstronautProvider issAstronautProvider)
    {
        _personRepository = personRepository;
        _issAstronautProvider = issAstronautProvider;
    }

    public async Task<IReadOnlyList<PersonRequest>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<PersonEntity> persons = await _personRepository.GetAllAsync(cancellationToken);

        return persons
            .Select(MapToRequest)
            .ToList();
    }

    public async Task<PersonRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        PersonEntity? person = await _personRepository.GetByIdAsync(id, cancellationToken);
        return person is null ? null : MapToRequest(person);
    }

    public async Task<PersonRequest> AddIfNotExistsAsync(PersonRequest personRequest, CancellationToken cancellationToken = default)
    {
        string normalizedName = personRequest.Name.Trim();
        string normalizedCraft = personRequest.Craft.Trim();

        PersonEntity? existingPerson = await _personRepository.GetByOfficialNameAsync(normalizedName, cancellationToken);

        if (existingPerson is not null)
        {
            return MapToRequest(existingPerson);
        }

        PersonEntity createdPerson = await _personRepository.AddAsync(
            new PersonEntity
            {
                OfficialName = normalizedName,
                SecondaryName = personRequest.SecondaryName.Trim(),
                Craft = normalizedCraft,
                Gender = personRequest.Gender.Trim(),
                Country = personRequest.Country.Trim(),
            },
            cancellationToken);

        return MapToRequest(createdPerson);
    }

    public async Task<PersonRequest?> UpdateAsync(int id, PersonRequest personRequest, CancellationToken cancellationToken = default)
    {
        PersonEntity? person = await _personRepository.GetByIdAsync(id, cancellationToken);
        if (person is null)
        {
            return null;
        }

        string normalizedName = personRequest.Name.Trim();
        PersonEntity? conflictingPerson = await _personRepository.GetByOfficialNameAsync(normalizedName, cancellationToken);
        if (conflictingPerson is not null && conflictingPerson.Id != id)
        {
            throw new InvalidOperationException("Another astronaut already uses that official name.");
        }

        person.OfficialName = normalizedName;
        person.SecondaryName = personRequest.SecondaryName.Trim();
        person.Craft = personRequest.Craft.Trim();
        person.Gender = personRequest.Gender.Trim();
        person.Country = personRequest.Country.Trim();

        PersonEntity updatedPerson = await _personRepository.UpdateAsync(person, cancellationToken);
        return MapToRequest(updatedPerson);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        => _personRepository.DeleteAsync(id, cancellationToken);

    public async Task<IReadOnlyList<AstronautStatusRequest>> SyncIssAstronautsAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<PersonRequest> currentIssAstronauts = await _issAstronautProvider.GetCurrentIssAstronautsAsync(cancellationToken);
        string[] currentIssNames = currentIssAstronauts
            .Select(person => person.Name.Trim())
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        IReadOnlyList<PersonEntity> existingIssAstronauts = await _personRepository.GetByOfficialNamesAsync(currentIssNames, cancellationToken);
        HashSet<string> existingNames = existingIssAstronauts
            .Select(person => person.OfficialName)
            .ToHashSet(StringComparer.Ordinal);

        List<PersonEntity> astronautsToAdd = currentIssAstronauts
            .Where(person => !existingNames.Contains(person.Name))
            .Select(person => new PersonEntity
            {
                OfficialName = person.Name,
                SecondaryName = person.SecondaryName.Trim(),
                Craft = person.Craft,
                Gender = person.Gender.Trim(),
                Country = person.Country.Trim(),
            })
            .ToList();

        if (astronautsToAdd.Count > 0)
        {
            await _personRepository.AddRangeAsync(astronautsToAdd, cancellationToken);
        }

        HashSet<string> activeIssNames = currentIssNames.ToHashSet(StringComparer.Ordinal);
        IReadOnlyList<PersonEntity> allPersons = await _personRepository.GetAllAsync(cancellationToken);

        return allPersons
            .Select(person => new AstronautStatusRequest
            {
                Id = person.Id,
                Name = person.OfficialName,
                SecondaryName = person.SecondaryName,
                Craft = person.Craft,
                Gender = person.Gender,
                Country = person.Country,
                IsCurrentlyOnIss = activeIssNames.Contains(person.OfficialName),
            })
            .ToList();
    }

    private static PersonRequest MapToRequest(PersonEntity person)
    {
        return new PersonRequest
        {
            Id = person.Id,
            Name = person.OfficialName,
            SecondaryName = person.SecondaryName,
            Craft = person.Craft,
            Gender = person.Gender,
            Country = person.Country,
        };
    }
}
