using DevOpsSpaceAgency.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevOpsSpaceAgency.DAL.Contexts;

public class SpaceAgencyContext : DbContext
{
    public DbSet<PersonEntity> Persons => Set<PersonEntity>();

    public SpaceAgencyContext(DbContextOptions<SpaceAgencyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonEntity>(entity =>
        {
            entity.ToTable("Persons");
            entity.Property(person => person.OfficialName).HasMaxLength(200).IsRequired();
            entity.Property(person => person.SecondaryName).HasMaxLength(200);
            entity.Property(person => person.Craft).HasMaxLength(100).IsRequired();
            entity.Property(person => person.Gender).HasMaxLength(50);
            entity.Property(person => person.Country).HasMaxLength(100);
            entity.HasIndex(person => person.OfficialName).IsUnique();
        });
    }
}
