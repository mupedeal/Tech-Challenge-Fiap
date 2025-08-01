﻿using ContactRegister.Application.Ddd.Dtos.BrasilApiDtos;
using ContactRegister.Application.Ddd.Interfaces.Services;
using System.Text.Json;

namespace ContactRegister.Application.Ddd.Services;

public class BrasilApiService(IHttpClientFactory clientFactory) : IDddApiService
{
	private readonly IHttpClientFactory _clientFactory = clientFactory;

	public async Task<DddApiResponseDto> GetByCode(int code)
	{
		var client = _clientFactory.CreateClient("BrasilApi");
		var response = await client.GetAsync($"/api/ddd/v1/{code}");
		var responseContent = await response.Content.ReadAsStringAsync();

		return response.IsSuccessStatusCode ?
			new DddApiResponseDto(JsonSerializer.Deserialize<DddApiSuccessResponseDto>(responseContent), null) :
			new DddApiResponseDto(null, JsonSerializer.Deserialize<DddApiErrorResponseDto>(responseContent));
	}
}