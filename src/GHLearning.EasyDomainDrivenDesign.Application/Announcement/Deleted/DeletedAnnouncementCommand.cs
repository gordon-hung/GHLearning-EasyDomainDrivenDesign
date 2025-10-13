using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Deleted;

public record DeletedAnnouncementCommand(
	Guid Id
) : IRequest;