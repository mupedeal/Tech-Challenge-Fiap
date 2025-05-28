using ContactRegister.Application.Ddd.Interfaces.Repositories;
using ContactRegister.Infrastructure.Ddd.Persistence;
using ContactRegister.Infrastructure.Ddd.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ContactRegister.Infrastructure.Ddd;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetValue<string>("CosmosConnection:ConnectionString")
			?? throw new InvalidOperationException("Connection string 'CosmosConnection:ConnectionString' not found.");
		var cosmosDatabase = configuration.GetValue<string>("CosmosConnection:Database")
			?? throw new InvalidOperationException("Connection string 'CosmosConnection:Database' not found.");

		services.AddDatabase(connectionString, cosmosDatabase);
		services.AddServices();

		return services;
	}

	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.TryAddScoped<IDddRepository, DddRepository>();

		return services;
	}

	private static void AddDatabase(this IServiceCollection services, string connectionString, string database)
	{
		services.AddCosmos<CosmosDbContext>(connectionString, database);
	}
}
