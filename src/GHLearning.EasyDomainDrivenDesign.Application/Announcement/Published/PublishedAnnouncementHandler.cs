using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Published;

internal class PublishedAnnouncementHandler(
	IAnnouncementRepository repository,
	IAnnouncementDomainEventDispatcher dispatcher) : IRequestHandler<PublishedAnnouncementCommand>
{
	public async Task Handle(PublishedAnnouncementCommand request, CancellationToken cancellationToken)
	{
		var announcement = await repository.GetAsync(request.Id, cancellationToken).ConfigureAwait(false)
			?? throw new ArgumentNullException(nameof(request), "Announcement not found.");

		announcement.SetPublish();

		await repository.UpdateAsync(announcement, cancellationToken).ConfigureAwait(false);
		await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
		await dispatcher.DispatchAsync(announcement.DomainEvents, cancellationToken).ConfigureAwait(false);
	}
}