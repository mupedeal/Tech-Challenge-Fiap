using ContactRegister.Application.Ddd.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using DddEntity = ContactRegister.Domain.Entities.Ddd;

namespace ContactRegister.Infrastructure.Ddd.Persistence.Repositories;

public class DddRepository(CosmosDbContext context) : IDddRepository
{
	private readonly CosmosDbContext _context = context;

	public async Task<int> AddDdd(DddEntity ddd)
	{
		ddd.CreatedAt = DateTime.UtcNow;

		await _context.Ddd.AddAsync(ddd);
		return await _context.SaveChangesAsync();
	}

	public async Task<DddEntity?> GetDddByCode(int code)
	{
		var ddd = await _context.Ddd.FirstOrDefaultAsync(ddd => ddd.PartitionKey == code.ToString());
		return ddd;
	}

	public async Task<List<DddEntity>> GetDdds()
	{
		var result = await _context.Ddd.ToListAsync();
		return result;
	}
}
