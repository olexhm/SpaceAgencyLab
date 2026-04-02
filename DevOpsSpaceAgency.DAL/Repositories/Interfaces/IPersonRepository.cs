using DevOpsSpaceAgency.DAL.Entities;

namespace DevOpsSpaceAgency.DAL.Repositories.Interfaces;

public interface IPersonRepository
{
    Task<PersonEntity> AddAsync(PersonEntity person, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<PersonEntity> persons, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PersonEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PersonEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PersonEntity?> GetByOfficialNameAsync(string officialName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PersonEntity>> GetByOfficialNamesAsync(IEnumerable<string> officialNames, CancellationToken cancellationToken = default);
    Task<PersonEntity> UpdateAsync(PersonEntity person, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
