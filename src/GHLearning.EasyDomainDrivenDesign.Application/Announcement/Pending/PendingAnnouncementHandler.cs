using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Pending;

internal class PendingAnnouncementHandler(
    IAnnouncementRepository repository) : IRequestHandler<PendingAnnouncementCommand>
{
    public async Task Handle(PendingAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var announcement = await repository.GetAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ArgumentNullException(nameof(request), "Announcement not found.");

        announcement.SetPending(request.Title, request.Content, request.PublishAt, request.ExpireAt);

        await repository.UpdateAsync(announcement, cancellationToken).ConfigureAwait(false);
        await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
    }
}