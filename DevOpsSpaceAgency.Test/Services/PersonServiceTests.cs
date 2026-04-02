using DevOpsSpaceAgency.BLL.Services;
using DevOpsSpaceAgency.BLL.Services.Interfaces;
using DevOpsSpaceAgency.DAL.Entities;
using DevOpsSpaceAgency.DAL.Repositories.Interfaces;
using DevOpsSpaceAgency.Shared.Models;

namespace DevOpsSpaceAgency.Test.Services;

public class PersonServiceTests
{
    [Fact]
    public async Task SyncIssAstronautsAsync_AddsMissingAstronautsAndFlagsLatestCrew()
    {
        InMemoryPersonRepository repository = new(
        [
            new PersonEntity { Id = 1, OfficialName = "Existing Astronaut", SecondaryName = "Existing Alias", Craft = "ISS", Gender = "Non-binary", Country = "USA" },
            new PersonEntity { Id = 2, OfficialName = "Retired Astronaut", Craft = "Shuttle", Gender = "Female", Country = "Canada" },
        ]);
        FakeIssAstronautProvider provider = new(
        [
            new PersonRequest { Name = "Existing Astronaut", Craft = "ISS" },
            new PersonRequest { Name = "New Astronaut", Craft = "ISS" },
        ]);
        PersonService service = new(repository, provider);

        IReadOnlyList<AstronautStatusRequest> result = await service.SyncIssAstronautsAsync();

        Assert.Collection(
            result,
            astronaut =>
            {
                Assert.Equal("Existing Astronaut", astronaut.Name);
                Assert.Equal("Existing Alias", astronaut.SecondaryName);
                Assert.Equal("Existing Alias", astronaut.DisplayName);
                Assert.True(astronaut.IsCurrentlyOnIss);
            },
            astronaut =>
            {
                Assert.Equal("New Astronaut", astronaut.Name);
                Assert.Equal("New Astronaut", astronaut.DisplayName);
                Assert.True(astronaut.IsCurrentlyOnIss);
            },
            astronaut =>
            {
                Assert.Equal("Retired Astronaut", astronaut.Name);
                Assert.False(astronaut.IsCurrentlyOnIss);
            });
    }

    [Fact]
    public async Task AddIfNotExistsAsync_DoesNotDuplicateExistingName()
    {
        InMemoryPersonRepository repository = new(
        [new PersonEntity { Id = 1, OfficialName = "Existing Astronaut", Craft = "ISS" }]);
        FakeIssAstronautProvider provider = new([]);
        PersonService service = new(repository, provider);

        PersonRequest result = await service.AddIfNotExistsAsync(new PersonRequest
        {
            Name = "Existing Astronaut",
            Craft = "Updated Craft",
            SecondaryName = "Alias",
            Gender = "Male",
            Country = "Japan",
        });

        Assert.Equal("Existing Astronaut", result.Name);
        Assert.Equal("ISS", result.Craft);
        Assert.Single(await repository.GetAllAsync());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesSecondaryNameGenderAndCountry()
    {
        InMemoryPersonRepository repository = new(
        [new PersonEntity { Id = 1, OfficialName = "Official Name", Craft = "ISS" }]);
        FakeIssAstronautProvider provider = new([]);
        PersonService service = new(repository, provider);

        PersonRequest? result = await service.UpdateAsync(1, new PersonRequest
        {
            Name = "Official Name",
            SecondaryName = "Preferred Name",
            Craft = "Dragon",
            Gender = "Female",
            Country = "Italy",
        });

        Assert.NotNull(result);
        Assert.Equal("Preferred Name", result.SecondaryName);
        Assert.Equal("Female", result.Gender);
        Assert.Equal("Italy", result.Country);
        Assert.Equal("Dragon", result.Craft);
    }

    private sealed class FakeIssAstronautProvider : IIssAstronautProvider
    {
        private readonly IReadOnlyList<PersonRequest> _astronauts;

        public FakeIssAstronautProvider(IReadOnlyList<PersonRequest> astronauts)
        {
            _astronauts = astronauts;
        }

        public Task<IReadOnlyList<PersonRequest>> GetCurrentIssAstronautsAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(_astronauts);
    }

    private sealed class InMemoryPersonRepository : IPersonRepository
    {
        private readonly List<PersonEntity> _persons;
        private int _nextId;

        public InMemoryPersonRepository(IEnumerable<PersonEntity> persons)
        {
            _persons = persons.OrderBy(person => person.OfficialName).ToList();
            _nextId = _persons.Count == 0 ? 1 : _persons.Max(person => person.Id) + 1;
        }

        public Task<PersonEntity> AddAsync(PersonEntity person, CancellationToken cancellationToken = default)
        {
            person.Id = _nextId++;
            _persons.Add(Clone(person));
            Sort();
            return Task.FromResult(Clone(person));
        }

        public Task AddRangeAsync(IEnumerable<PersonEntity> persons, CancellationToken cancellationToken = default)
        {
            foreach (PersonEntity person in persons)
            {
                person.Id = _nextId++;
                _persons.Add(Clone(person));
            }

            Sort();
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<PersonEntity>> GetAllAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<PersonEntity>>(_persons.Select(Clone).ToList());

        public Task<PersonEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => Task.FromResult(_persons.Where(person => person.Id == id).Select(Clone).FirstOrDefault());

        public Task<PersonEntity?> GetByOfficialNameAsync(string officialName, CancellationToken cancellationToken = default)
            => Task.FromResult(_persons.Where(person => person.OfficialName == officialName).Select(Clone).FirstOrDefault());

        public Task<IReadOnlyList<PersonEntity>> GetByOfficialNamesAsync(IEnumerable<string> officialNames, CancellationToken cancellationToken = default)
        {
            HashSet<string> names = officialNames.ToHashSet(StringComparer.Ordinal);
            return Task.FromResult<IReadOnlyList<PersonEntity>>(
                _persons.Where(person => names.Contains(person.OfficialName)).Select(Clone).ToList());
        }

        public Task<PersonEntity> UpdateAsync(PersonEntity person, CancellationToken cancellationToken = default)
        {
            int index = _persons.FindIndex(existingPerson => existingPerson.Id == person.Id);
            _persons[index] = Clone(person);
            Sort();
            return Task.FromResult(Clone(person));
        }

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            int removed = _persons.RemoveAll(person => person.Id == id);
            return Task.FromResult(removed > 0);
        }

        private static PersonEntity Clone(PersonEntity person) => new()
        {
            Id = person.Id,
            OfficialName = person.OfficialName,
            SecondaryName = person.SecondaryName,
            Craft = person.Craft,
            Gender = person.Gender,
            Country = person.Country,
        };

        private void Sort() => _persons.Sort((left, right) => string.CompareOrdinal(left.OfficialName, right.OfficialName));
    }
}
