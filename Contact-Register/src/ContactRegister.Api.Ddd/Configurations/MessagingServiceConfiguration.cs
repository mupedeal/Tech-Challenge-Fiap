using ContactRegister.Api.Ddd.Consumers;
using MassTransit;

namespace ContactRegister.Api.Ddd.Configurations;

public static class MessagingServiceConfiguration
{
	public static IServiceCollection AddMessagingService(this IServiceCollection services, IConfiguration configuration)
	{
		var fila = configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
		var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
		var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
		var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

		services.AddMassTransit(x =>
		{
			x.UsingRabbitMq((context, cfg) =>
			{
				cfg.Host(servidor, "/", h =>
				{
					h.Username(usuario);
					h.Password(senha);
				});

				cfg.ReceiveEndpoint(fila, e =>
				{
					e.Consumer<DddConsumer>(context);
				});

				cfg.ConfigureEndpoints(context);
			});

			x.AddConsumer<DddConsumer>();
		});

		return services;
	}
}
