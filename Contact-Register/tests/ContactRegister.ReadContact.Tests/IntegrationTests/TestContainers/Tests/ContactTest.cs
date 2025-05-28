using ContactRegister.Application.DTOs;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.ReadContact.Tests.IntegrationTests.Common;
using ContactRegister.ReadContact.Tests.IntegrationTests.TestContainers.Factories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ContactRegister.ReadContact.Tests.IntegrationTests.TestContainers.Tests;

public class ContactTest : BaseIntegrationTests, IClassFixture<TestContainerContactRegisterFactory>
{
	private readonly string resource = "/Contact";
	public ContactTest(TestContainerContactRegisterFactory testContainerFactory) : base(testContainerFactory)
	{
		var testContainerContext = testContainerFactory.Services.GetRequiredService<AppDbContext>();
		if (testContainerContext.Database.GetPendingMigrations().Any())
			testContainerContext.Database.Migrate();
	}

	[Fact]
	public async Task GetContactShouldReturnContactDto()
	{
		// Arrange
		var contactClient = GetContactClient();
		int request = 1;

		// Act
		var response = await contactClient.GetAsync($"{resource}/GetContact/{request}");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var contactDto = await response.Content.ReadFromJsonAsync<ContactDto>();
		contactDto?.Id.Should().Be(request);
	}

	[Fact]
	public async Task GetContactsByDddCodes_ShouldReturn_List()
	{
		// Arrange
		var contactClient = GetContactClient();
		int[] request = [68];

		// Act
		var response = await contactClient.PostAsJsonAsync($"{resource}/GetContactsByDddCodes", request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var contactDtos = await response.Content.ReadFromJsonAsync<List<ContactDto>>();
		contactDtos?.Should().NotBeEmpty();
		contactDtos?.Should().Contain(c => c.Ddd == request[0]);
	}
}
