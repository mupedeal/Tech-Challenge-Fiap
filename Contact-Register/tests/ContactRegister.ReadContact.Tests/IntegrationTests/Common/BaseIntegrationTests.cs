using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ContactProgram = ContactRegister.Api.ReadContact.Program;

namespace ContactRegister.ReadContact.Tests.IntegrationTests.Common;

public abstract class BaseIntegrationTests : IDisposable
{
	private IServiceScope? _scoped;
	private readonly WebApplicationFactory<ContactProgram> _contactFactory;
	private readonly HttpClient _contactClient;

	protected BaseIntegrationTests(WebApplicationFactory<ContactProgram> contactFactory)
	{
		_contactFactory = contactFactory;
		_contactClient = _contactFactory.CreateClient();
	}

	protected TServiceType InjectServiceInstance<TServiceType>() where TServiceType : class
	{
		_scoped = _contactFactory.Services.CreateScope();

		return _scoped.ServiceProvider.GetRequiredService<TServiceType>();
	}

	public HttpClient GetContactClient()
	{
		return _contactClient;
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		_scoped?.Dispose();
	}
}
