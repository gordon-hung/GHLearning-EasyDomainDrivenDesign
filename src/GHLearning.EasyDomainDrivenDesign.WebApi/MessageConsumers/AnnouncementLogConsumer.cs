using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Log;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MassTransit;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.WebApi.MessageConsumers;

public class AnnouncementLogConsumer(
	ILogger<AnnouncementLogConsumer> logger,
	IMediator mediator) : IConsumer<AnnouncementLogDomainEvent>
{
	public Task Consume(ConsumeContext<AnnouncementLogDomainEvent> context)
	{
		logger.LogInformation("Received AnnouncementPublishedEvent: {Id}, {Title}", context.Message.AnnouncementId, context.Message.Status);

		return mediator.Publish(new LogAnnouncementNotification(
			Id: context.Message.Id,
			AnnouncementId: context.Message.AnnouncementId,
			Status: AnnouncementStatus.FromName(context.Message.Status),
			LogdAt: context.Message.LogAt));
	}
}