using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Draft;

public class DraftAnnouncementHandler(
	IAnnouncementRepository repository,
	IAnnouncementDomainEventDispatcher dispatcher) : IRequestHandler<DraftAnnouncementCommand, Guid>
{
	public async Task<Guid> Handle(DraftAnnouncementCommand request, CancellationToken cancellationToken)
	{
		AnnouncementEntity announcement;
		if (request.Id == Guid.Empty)
		{
			announcement = new AnnouncementEntity(title: request.Title, content: request.Content, publishAt: request.PublishAt, expireAt: request.ExpireAt);

			await repository.AddAsync(announcement, cancellationToken).ConfigureAwait(false);
		}
		else
		{
			announcement = await repository.GetAsync(request.Id, cancellationToken).ConfigureAwait(false)
				?? throw new ArgumentNullException(nameof(request), "Announcement not found.");

			announcement.SetDraft(request.Title, request.Content, request.PublishAt, request.ExpireAt);

			await repository.UpdateAsync(announcement, cancellationToken).ConfigureAwait(false);
		}

		if (!request.IsDraft)
		{
			announcement.SetPending();
			await repository.UpdateAsync(announcement, cancellationToken).ConfigureAwait(false);
		}

		await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
		await dispatcher.DispatchAsync(announcement.DomainEvents, cancellationToken).ConfigureAwait(false);

		return announcement.Id;
	}
}
