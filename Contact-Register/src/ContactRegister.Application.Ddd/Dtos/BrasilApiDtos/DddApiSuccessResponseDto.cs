using System.Text.Json.Serialization;

namespace ContactRegister.Application.Ddd.Dtos.BrasilApiDtos;

public class DddApiSuccessResponseDto
{
	[JsonPropertyName("state")]
	public string State { get; set; } = string.Empty;
	[JsonPropertyName("cities")]
	public ICollection<string> Cities { get; set; } = [];
}
