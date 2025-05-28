using ContactRegister.Application.Inputs;
using ContactRegister.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContactRegister.Api.WriteContact.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
	private readonly IContactService _contactService;

	public ContactController(IContactService contactService)
	{
		_contactService = contactService;
	}

	/// <summary>
	/// Cria um novo contato.
	/// </summary>
	/// <response code="201">Criado com sucesso</response>
	/// <response code="400">
	/// Exemplo de erro ao criar um contato:
	/// 
	///     POST /Contact/UpdateContact
	///     [
	///       {
	///         "code": "Ddd.ExternalApi",
	///         "description": "DDD não encontrado",
	///         "type": 0,
	///         "numericType": 0,
	///         "metadata": null
	///       }
	///     ]
	/// </response>
	[HttpPost("[action]")]
	public async Task<IActionResult> CreateContact([FromBody] ContactInput contact)
	{
		var result = await _contactService.AddContactAsync(contact);

		if (result.IsError)
			return BadRequest(result.Errors);

		return Created(HttpContext.Request.GetDisplayUrl(), new { Id = Guid.NewGuid() });
	}

	/// <summary>
	/// Atualiza um Contato a partir do seu ID único informado.
	/// </summary>
	/// <param name="id">Identificador único (ID) do contato a ser excluído.</param>
	/// <response code="204">Atualização realizada com sucesso</response>
	/// <response code="400">
	/// Exemplo de erro ao atualizar o contato:
	/// 
	///     PUT /Contact/UpdateContact/{id}
	///     [
	///       {
	///         "code": "Ddd.ExternalApi",
	///         "description": "DDD não encontrado",
	///         "type": 0,
	///         "numericType": 0,
	///         "metadata": null
	///       }
	///     ]
	/// </response>
	[HttpPut("[action]/{id:int}")]
	public async Task<IActionResult> UpdateContact([FromRoute] int id, [FromBody] ContactInput contact)
	{
		var result = await _contactService.UpdateContactAsync(id, contact);

		if (result.IsError)
			return BadRequest(result.Errors);

		return NoContent();
	}

	/// <summary>
	/// Exclui um Contato a partir do seu ID único informado.
	/// </summary>
	/// <param name="id">Identificador único (ID) do contato a ser excluído.</param>
	/// <returns>A confirmação de exclusão do Contato, ou uma lista de erros.</returns>
	/// <response code="204">Exclusão realizada com sucesso</response>
	/// <response code="400">
	/// Erro ao excluir contato. Exemplo de retorno:
	/// 
	///		DELETE /Contact/DeleteContact/{id}
	///		[
	///			{
	///				"code": "Contact.NotFound",
	///				"description": "Contact 3 not found",
	///				"type": 4,
	///				"numericType": 4,
	///				"metadata": null
	///			}
	///		]
	/// </response>
	[HttpDelete("[action]/{id:int}")]
	public async Task<IActionResult> DeleteContact([FromRoute] int id)
	{
		var result = await _contactService.DeleteContactAsync(id);

		if (result.IsError)
			return BadRequest(result.Errors);

		return NoContent();
	}
}
