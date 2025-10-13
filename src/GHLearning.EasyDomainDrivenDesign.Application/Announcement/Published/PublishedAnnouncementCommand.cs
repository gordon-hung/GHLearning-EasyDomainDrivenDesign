using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Published;

public record PublishedAnnouncementCommand(
	Guid Id
) : IRequest;