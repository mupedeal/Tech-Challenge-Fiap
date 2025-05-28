using ContactRegister.Application;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Infrastructure;
using ContactRegister.Infrastructure.Cache;
using ContactRegister.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;

namespace ContactRegister.Api.WriteContact;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();
		builder.Services.AddControllers();
		builder.Services.AddMemoryCache();
		builder.Services.AddTransient<ICacheService, MemCacheService>();
		builder.Services.AddApplication(builder.Configuration);
		builder.Services.AddInfrastructure(builder.Configuration);
		builder.Services.AddMemoryCache();

		builder.Services.AddMetrics();
		builder.Services.UseHttpClientMetrics();

		builder.Services.AddHealthChecks()
			.AddCheck("health", () => HealthCheckResult.Healthy("Aplicação funcionando normalmente"));

		var app = builder.Build();

		using var scope = app.Services.CreateScope();
		using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		if (context.Database.GetPendingMigrations().Any())
		{
			context.Database.Migrate();
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

		app.Run();
	}
}
