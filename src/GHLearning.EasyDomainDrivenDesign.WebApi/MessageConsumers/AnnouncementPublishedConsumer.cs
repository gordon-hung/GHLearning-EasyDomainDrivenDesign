using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MassTransit;

namespace GHLearning.EasyDomainDrivenDesign.WebApi.MessageConsumers;

public class AnnouncementPublishedConsumer(
	ILogger<AnnouncementPublishedConsumer> logger) : IConsumer<AnnouncementPublishedDomainEvent>
{
	public Task Consume(ConsumeContext<AnnouncementPublishedDomainEvent> context)
	{
		logger.LogInformation("Received AnnouncementPublishedEvent: {Id}, {Title}", context.Message.AnnouncementId, context.Message.Title);

		return Task.CompletedTask;
	}
}