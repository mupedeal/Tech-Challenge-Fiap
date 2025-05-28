using ContactRegister.Application.Ddd.Interfaces.Services;
using ContactRegister.Application.Ddd.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace ContactRegister.Application.Ddd;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IDddService, DddService>();

		string brasilApiBaseAddress = "https://brasilapi.com.br";

		services.AddHttpClient("BrasilApi", o =>
		{
			o.BaseAddress = new Uri(brasilApiBaseAddress);
		}).SetHandlerLifetime(TimeSpan.FromMinutes(5)).AddPolicyHandler(GetRetryPolicy());

		services.AddScoped<IDddApiService, BrasilApiService>();

		return services;
	}

	private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
	{
		return HttpPolicyExtensions
			.HandleTransientHttpError()
			.WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
	}
}
