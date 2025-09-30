using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Log;

internal class LogAnnouncementHandler(
	IAnnouncementLogRepository repository) : INotificationHandler<LogAnnouncementNotification>
{
	public async Task Handle(LogAnnouncementNotification request, CancellationToken cancellationToken)
	{
		var entity = await repository.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);

		if (entity is not null)
		{
			return;
		}

		entity = new AnnouncementLogEntity(
			id: request.Id,
			announcementId: request.AnnouncementId,
			status: request.Status,
			logdAt: DateTimeOffset.UtcNow);

		await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
		await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
	}
}
