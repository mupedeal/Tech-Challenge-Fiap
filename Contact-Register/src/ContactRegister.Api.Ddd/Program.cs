using ContactRegister.Application.Ddd;
using ContactRegister.Application.Ddd.Interfaces.Services;
using ContactRegister.Infrastructure.Ddd;
using ContactRegister.Infrastructure.Ddd.Cache;
using ContactRegister.Infrastructure.Ddd.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;

namespace ContactRegister.Api.Ddd;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		builder.Services.AddControllers();
		builder.Services.AddMemoryCache();
		builder.Services.AddTransient<ICacheService, MemCacheService>();
		builder.Services.AddApplication();
		builder.Services.AddInfrastructure(builder.Configuration);
		builder.Services.AddMemoryCache();

		builder.Services.AddMetrics();
		builder.Services.UseHttpClientMetrics();

		builder.Services.AddHealthChecks()
			.AddCheck("health", () => HealthCheckResult.Healthy("Aplicação funcionando normalmente"));

		var app = builder.Build();

		using (var scope = app.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<CosmosDbContext>();
			await context.Database.EnsureCreatedAsync();
		}

		app.UseSwagger();
		app.UseSwaggerUI();

		app.UseRouting();
		app.UseHttpMetrics();
		app.UseMetricServer();
		app.UseHttpMetrics();

		app.MapControllers();
		app.MapMetrics();
		app.MapHealthChecks("/health");

		app.UseHttpsRedirection();

		await app.RunAsync();
	}
}
