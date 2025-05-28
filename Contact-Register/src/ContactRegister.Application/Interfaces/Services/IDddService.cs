using ContactRegister.Application.DTOs;
using ErrorOr;

namespace ContactRegister.Application.Interfaces.Services;

public interface IDddService
{
	Task<ErrorOr<DddDto?>> GetDddByCode(int code);
}
