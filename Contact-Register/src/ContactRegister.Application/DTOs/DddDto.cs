using ContactRegister.Domain.Entities;
using System.Text.Json.Serialization;

namespace ContactRegister.Application.DTOs;

public class DddDto
{
	[JsonPropertyName("code")]
	public int Code { get; set; }
	[JsonPropertyName("state")]
	public string State { get; set; } = string.Empty;
	[JsonPropertyName("region")]
	public string Region { get; set; } = string.Empty;

    public Ddd ToDdd()
    {
        return new Ddd(Code, State, Region);
    }

    public static DddDto FromEntity(Ddd entity)
    {
        return new DddDto
        {
            Code = entity.Code,
            State = entity.State,
            Region = entity.Region
        };
    }
}