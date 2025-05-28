﻿using ContactRegister.Application.Ddd.Dtos;
using ContactRegister.Application.Ddd.Interfaces.Services;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace ContactRegister.Api.Ddd.Controllers;

[ApiController]
[Route("[controller]")]
public class DddController(IDddService dddService, ICacheService cacheService) : ControllerBase
{
	private readonly IDddService _dddService = dddService;
	private readonly ICacheService _cacheService = cacheService;

	/// <summary>
	/// Busca a lista com todos os DDDs cadastrados na base de dados.
	/// </summary>
	/// <returns>A lista com todos os DDDs, ou uma lista de erros.</returns>
	/// <response code="200">
	///	Busca realizada com sucesso. Exemplo de retorno:
	///		
	///		GET /Ddd/GetDdd
	///		[
	///			{
	///				"code": 69,
	///				"state": "RO",
	///				"region": "ALTO ALEGRE DO PARECIS, VALE DO PARAÍSO, ..."
	///			},
	///			{
	///				"code": 68,
	///				"state": "AC",
	///				"region": "PORTO ACRE, XAPURI, TARAUACÁ, SENA MADUREIRA, ..."
	///			}
	///		]
	/// </response>
	/// <response code="400">
	/// Erro ao efetuar a busca:
	/// 
	///		GET /Ddd/GetDdd/
	///		[
	///			{
	///				"code": "Ddd.Get.Exception",
	///				"description": "Invalid object",
	///				"type": 0,
	///				"numericType": 0,
	///				"metadata": null
	///			}
	///		]
	/// </response>
	[HttpGet("[action]")]
	public async Task<IActionResult> GetDdd()
	{
		var result = await _dddService.GetDdd();

		if (result.IsError)
			return BadRequest(result.Errors);

		return Ok(result.Value);
	}

	/// <summary>
	/// Busca as informações regionais (estado e lista de cidades) a partir de um DDD informado.
	/// </summary>
	/// <param name="code">Código DDD a ser pesquisado.</param>
	/// <returns>A informação sobre o DDD, ou uma lista de erros.</returns>
	/// <response code="200">
	///	Busca realizada com sucesso. Exemplo de retorno:
	///		
	///		GET /Ddd/GetDdd/{code}
	///		{
	///			"code": 68,
	///			"state": "AC",
	///			"region": "PORTO ACRE, XAPURI, TARAUACÁ, ..."
	///		}
	/// </response>
	/// <response code="400">
	/// Erro ao efetuar a busca. Exemplo de retorno:
	/// 
	///		GET /Ddd/GetDdd/{code}
	///		[
	///			{
	///				"code": "Ddd.ExternalApi",
	///				"description": "DDD não encontrado",
	///				"type": 0,
	///				"numericType": 0,
	///				"metadata": null
	///			}
	///		]
	/// </response>
	[HttpGet("[action]/{code:int}")]
	public async Task<IActionResult> GetDdd(int code)
	{
		var cachedResult = _cacheService.Get(code.ToString());

		if (cachedResult != null)
			return Ok(cachedResult);

		var result = await _dddService.GetDddByCode(code);

		if (result.IsError)
			return BadRequest(result.Errors);

		_cacheService.Set(code.ToString(), result.Value!);

		return Ok(result.Value);
	}
}
