using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Domain.Entities;
using ContactRegister.Domain.ValueObjects;
using ContactRegister.Infrastructure.Persistence;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Testcontainers.MsSql;
using Xunit;
using ContactProgram = ContactRegister.Api.WriteContact.Program;

namespace ContactRegister.WriteContact.Tests.IntegrationTests.TestContainers.Factories;

public class TestContainerContactRegisterFactory : WebApplicationFactory<ContactProgram>, IAsyncLifetime
{
	private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
		.WithName($"sql-server-test-{Guid.NewGuid()}")
		.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
		.WithPassword("Password@1234")
		.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
		.Build();
	public Mock<IDddService> DddServiceMock { get; } = new();

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
					var contato1 = new Contact("John", "Doe", "john.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "BRASILÉIA", "AC", "012345-678"), new Phone("11111111"), new Phone("922222222"), 68);
					context.Set<Contact>().Add(contato1);

					var contato2 = new Contact("Jane", "Doe", "jane.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "BRASILÉIA", "AC", "012345-678"), new Phone("11111111"), new Phone("922222222"), 68);
					context.Set<Contact>().Add(contato2);

					context.SaveChanges();
				});
			});

			services.RemoveAll(typeof(IDddService));
			services.AddSingleton(DddServiceMock.Object);
		});
	}
}
