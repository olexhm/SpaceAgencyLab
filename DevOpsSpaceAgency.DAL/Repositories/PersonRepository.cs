using DevOpsSpaceAgency.DAL.Contexts;
using DevOpsSpaceAgency.DAL.Entities;
using DevOpsSpaceAgency.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevOpsSpaceAgency.DAL.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly SpaceAgencyContext _context;

    public PersonRepository(SpaceAgencyContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PersonEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Persons
            .AsNoTracking()
            .OrderBy(person => person.OfficialName)
            .ToListAsync(cancellationToken);
    }

    public async Task<PersonEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Persons
            .FirstOrDefaultAsync(person => person.Id == id, cancellationToken);
    }

    public async Task<PersonEntity?> GetByOfficialNameAsync(string officialName, CancellationToken cancellationToken = default)
    {
        return await _context.Persons
            .FirstOrDefaultAsync(person => person.OfficialName == officialName, cancellationToken);
    }

    public async Task<IReadOnlyList<PersonEntity>> GetByOfficialNamesAsync(IEnumerable<string> officialNames, CancellationToken cancellationToken = default)
    {
        string[] normalizedNames = officialNames
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (normalizedNames.Length == 0)
        {
            return [];
        }

        return await _context.Persons
            .Where(person => normalizedNames.Contains(person.OfficialName))
            .ToListAsync(cancellationToken);
    }

    public async Task<PersonEntity> AddAsync(PersonEntity person, CancellationToken cancellationToken = default)
    {
        _context.Persons.Add(person);
        await _context.SaveChangesAsync(cancellationToken);

        return person;
    }

    public async Task AddRangeAsync(IEnumerable<PersonEntity> persons, CancellationToken cancellationToken = default)
    {
        _context.Persons.AddRange(persons);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PersonEntity> UpdateAsync(PersonEntity person, CancellationToken cancellationToken = default)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync(cancellationToken);
        return person;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        PersonEntity? person = await _context.Persons.FirstOrDefaultAsync(existingPerson => existingPerson.Id == id, cancellationToken);
        if (person is null)
        {
            return false;
        }

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
