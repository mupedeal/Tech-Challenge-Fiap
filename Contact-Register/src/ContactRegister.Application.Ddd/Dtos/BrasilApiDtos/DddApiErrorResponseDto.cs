using System.Text.Json.Serialization;

namespace ContactRegister.Application.Ddd.Dtos.BrasilApiDtos;

public class DddApiErrorResponseDto
{
	[JsonPropertyName("message")]
	public string Message { get; set; } = string.Empty;
	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}
