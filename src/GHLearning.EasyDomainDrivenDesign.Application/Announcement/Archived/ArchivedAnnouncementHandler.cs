using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Archived;

internal class ArchivedAnnouncementHandler(
	IAnnouncementRepository repository,
	IAnnouncementDomainEventDispatcher dispatcher) : IRequestHandler<ArchivedAnnouncementCommand>
{
	public async Task Handle(ArchivedAnnouncementCommand request, CancellationToken cancellationToken)
	{
		var announcement = await repository.GetAsync(request.Id, cancellationToken).ConfigureAwait(false)
			?? throw new ArgumentNullException(nameof(request), "Announcement not found.");

		announcement.SetArchive();

		await repository.UpdateAsync(announcement, cancellationToken).ConfigureAwait(false);
		await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
		await dispatcher.DispatchAsync(announcement.DomainEvents, cancellationToken).ConfigureAwait(false);
	}
}