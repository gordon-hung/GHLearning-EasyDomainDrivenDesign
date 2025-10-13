using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;
using MassTransit;

namespace GHLearning.EasyDomainDrivenDesign.Infrastructure.Announcement;

internal class AnnouncementDomainEventDispatcher(
	IPublishEndpoint publish) : IAnnouncementDomainEventDispatcher
{
	public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
	{
		foreach (var domainEvent in events)
		{
			switch (domainEvent)
			{
				case AnnouncementPublishedDomainEvent e:
					await publish.Publish(new AnnouncementPublishedDomainEvent(
						announcementId: e.AnnouncementId,
						title: e.Title),
						cancellationToken: cancellationToken)
						.ConfigureAwait(false);
					break;

				case AnnouncementLogDomainEvent e:
					await publish.Publish(new AnnouncementLogDomainEvent(
						id: e.Id,
						announcementId: e.AnnouncementId,
						status: e.Status,
						logAt: e.LogAt),
						cancellationToken: cancellationToken)
						.ConfigureAwait(false);
					break;
			}
		}
	}
}