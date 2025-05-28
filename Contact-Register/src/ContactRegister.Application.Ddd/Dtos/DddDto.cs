using System.Text.Json.Serialization;
using DddEntity = ContactRegister.Domain.Entities.Ddd;

namespace ContactRegister.Application.Ddd.Dtos;

public class DddDto
{
	[JsonPropertyName("code")]
	public int Code { get; set; }
	[JsonPropertyName("state")]
	public string State { get; set; } = string.Empty;
	[JsonPropertyName("region")]
	public string Region { get; set; } = string.Empty;

	public DddEntity ToDdd()
	{
		return new DddEntity(Code, State, Region);
	}

	public static DddDto FromEntity(DddEntity entity)
	{
		return new DddDto
		{
			Code = entity.Code,
			State = entity.State,
			Region = entity.Region
		};
	}
}
