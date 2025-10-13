using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Get;

internal class GetAnnouncementHandler(
	IAnnouncementRepository repository) : IRequestHandler<GetAnnouncementQuery, AnnouncementEntity?>
{
	public Task<AnnouncementEntity?> Handle(GetAnnouncementQuery request, CancellationToken cancellationToken)
		=> repository.GetAsync(request.Id, cancellationToken);
}