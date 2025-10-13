using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Deleted;

internal class DeletedAnnouncementHandler(
	IAnnouncementRepository repository,
	IAnnouncementDomainEventDispatcher dispatcher) : IRequestHandler<DeletedAnnouncementCommand>
{
	public async Task Handle(DeletedAnnouncementCommand request, CancellationToken cancellationToken)
	{
		var announcement = await repository.GetAsync(request.Id, cancellationToken).ConfigureAwait(false)
			?? throw new ArgumentNullException(nameof(request), "Announcement not found.");

		announcement.SetDelete();

		await repository.UpdateAsync(announcement, cancellationToken).ConfigureAwait(false);
		await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
		await dispatcher.DispatchAsync(announcement.DomainEvents, cancellationToken).ConfigureAwait(false);
	}
}