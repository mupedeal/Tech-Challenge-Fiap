using DddEntity = ContactRegister.Domain.Entities.Ddd;

namespace ContactRegister.Application.Ddd.Interfaces.Repositories;

public interface IDddRepository
{
	Task<int> AddDdd(DddEntity ddd);
	Task<List<DddEntity>> GetDdds();
	Task<DddEntity?> GetDddByCode(int code);
}
