using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Services;
using ErrorOr;
using System.Text.Json;

namespace ContactRegister.Application.Services;

public class DddService(IHttpClientFactory clientFactory) : IDddService
{
	private readonly IHttpClientFactory _clientFactory = clientFactory;

	public async Task<ErrorOr<DddDto?>> GetDddByCode(int code)
	{
		var client = _clientFactory.CreateClient("DddApi");
		var response = await client.GetAsync($"/ddd/getddd/{code}");
		var responseContent = await response.Content.ReadAsStringAsync();

		return response.IsSuccessStatusCode ?
			JsonSerializer.Deserialize<DddDto>(responseContent) :
			JsonSerializer.Deserialize<Error>(responseContent);
	}
}
