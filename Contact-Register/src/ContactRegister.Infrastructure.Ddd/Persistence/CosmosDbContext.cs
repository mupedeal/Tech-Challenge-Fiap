using ContactRegister.Domain.Entities.Abstractions;
using ContactRegister.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using DddEntity = ContactRegister.Domain.Entities.Ddd;

namespace ContactRegister.Infrastructure.Ddd.Persistence;

public class CosmosDbContext(DbContextOptions<CosmosDbContext> options) : DbContext(options)
{
	public DbSet<DddEntity> Ddd { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DddEntity>().ToContainer("ddds")
										 .HasNoDiscriminator()
										 .HasPartitionKey(a => a.PartitionKey)
										 .HasKey(a => a.Id);
	}

	public override int SaveChanges()
	{
		UpdateTimestamps();
		return base.SaveChanges();
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		UpdateTimestamps();
		return await base.SaveChangesAsync(cancellationToken);
	}

	private void UpdateTimestamps()
	{
		var entries = ChangeTracker
			.Entries()
			.Where(e => e.Entity.GetType().IsSubclassOfRawGeneric(typeof(AbstractEntity<>)))
			.ToList();

		foreach (var entry in entries)
		{
			if (entry.Entity is IAbstractEntity entity)
			{
				if (entry.State == EntityState.Added)
				{
					entity.CreatedAt = DateTime.UtcNow;
					entity.UpdatedAt = DateTime.UtcNow;
				}
				else if (entry.State == EntityState.Modified)
				{
					entry.Property("CreatedAt").IsModified = false;
					entity.UpdatedAt = DateTime.UtcNow;
				}
			}
		}
	}
}
