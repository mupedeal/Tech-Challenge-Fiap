using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace ContactRegister.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IContactService, ContactService>();

		string dddApiBaseAddress = configuration.GetValue<string>("DddApiBaseAddress") ?? throw new ArgumentException("Variável 'DddApiBaseAddress' não configurada.");

		services.AddHttpClient("DddApi", o =>
		{
			o.BaseAddress = new Uri(dddApiBaseAddress);
		}).SetHandlerLifetime(TimeSpan.FromMinutes(5)).AddPolicyHandler(GetRetryPolicy());

		services.AddScoped<IDddService, DddService>();

		return services;
    }

	private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
	{
		return HttpPolicyExtensions
			.HandleTransientHttpError()
			.WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
	}
}