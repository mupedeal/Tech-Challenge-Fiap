using ContactRegister.Application.Ddd.Interfaces.Repositories;
using MassTransit;
using DddEntity = ContactRegister.Domain.Entities.Ddd;

namespace ContactRegister.Api.Ddd.Consumers
{
	public class DddConsumer : IConsumer<DddEntity>
	{
		private readonly ILogger<DddConsumer> _logger;
		private readonly IDddRepository _dddRepository;

		public DddConsumer(ILogger<DddConsumer> logger, IDddRepository dddRepository)
		{
			_logger = logger;
			_dddRepository = dddRepository;
		}

		public async Task Consume(ConsumeContext<DddEntity> context)
		{
			try
			{
				_ = await _dddRepository.AddDdd(context.Message);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
			}
		}
	}
}
