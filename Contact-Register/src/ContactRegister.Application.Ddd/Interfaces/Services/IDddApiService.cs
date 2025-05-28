using ContactRegister.Application.Ddd.Dtos.BrasilApiDtos;

namespace ContactRegister.Application.Ddd.Interfaces.Services;

public interface IDddApiService
{
	Task<DddApiResponseDto> GetByCode(int code);
}
