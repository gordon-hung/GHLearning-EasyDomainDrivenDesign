using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Pending;

public record PendingAnnouncementCommand(
	Guid Id,
	string Title,
	string Content,
	DateTimeOffset PublishAt,
	DateTimeOffset? ExpireAt
) : IRequest;