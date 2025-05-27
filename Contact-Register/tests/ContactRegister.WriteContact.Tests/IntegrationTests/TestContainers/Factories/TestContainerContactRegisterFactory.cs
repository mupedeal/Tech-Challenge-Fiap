﻿using ContactRegister.Domain.Entities;
using ContactRegister.Domain.ValueObjects;
using ContactRegister.Infrastructure.Persistence;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;

namespace ContactRegister.WriteContact.Tests.IntegrationTests.TestContainers.Factories;

public class TestContainerContactRegisterFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
		.WithName($"sql-server-test-{Guid.NewGuid()}")
		.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
		.WithPassword("Password@1234")
		.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
		.Build();

	public async Task InitializeAsync()
	{
		await _msSqlContainer.StartAsync();
	}

	public new async Task DisposeAsync()
	{
		await _msSqlContainer.StopAsync();
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Testing");
		builder.ConfigureTestServices(services =>
		{
			var descriptor = services.SingleOrDefault(
				d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
			if (descriptor is not null)
			{
				services.Remove(descriptor);
			}
			services.AddDbContext<AppDbContext>(b =>
			{
				var connectionString = _msSqlContainer.GetConnectionString();
				b.UseSqlServer(connectionString).UseSeeding((context, _) =>
				{
					var ddd = new Ddd(68, "AC", "PORTO ACRE, XAPURI, TARAUACÁ, SENA MADUREIRA, SENADOR GUIOMARD, SANTA ROSA DO PURUS, RODRIGUES ALVES, RIO BRANCO, PORTO WALTER, PLÁCIDO DE CASTRO, MARECHAL THAUMATURGO, MANOEL URBANO, MÂNCIO LIMA, JORDÃO, FEIJÓ, EPITACIOLÂNDIA, CRUZEIRO DO SUL, CAPIXABA, BUJARI, BRASILÉIA, ASSIS BRASIL, ACRELÂNDIA");
					var dddEntity = context.Set<Ddd>().Add(ddd);
					context.SaveChanges();

					var contato1 = new Contact("John", "Doe", "john.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "BRASILÉIA", "AC", "012345-678"), new Phone("11111111"), new Phone("922222222"), dddEntity.Entity);
					context.Set<Contact>().Add(contato1);

					var contato2 = new Contact("Jane", "Doe", "jane.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "BRASILÉIA", "AC", "012345-678"), new Phone("11111111"), new Phone("922222222"), dddEntity.Entity);
					context.Set<Contact>().Add(contato2);

					context.SaveChanges();
				});
			});
		});
	}
}
