using ContactRegister.Application.Ddd.Dtos;
using ErrorOr;

namespace ContactRegister.Application.Ddd.Interfaces.Services;

public interface IDddService
{
	Task<ErrorOr<List<DddDto>>> GetDdd();
	Task<ErrorOr<DddDto?>> GetDddByCode(int code);
}
