using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Draft;

public record DraftAnnouncementCommand(
    Guid Id,
    string Title,
    string Content,
    DateTimeOffset PublishAt,
    DateTimeOffset? ExpireAt,
	bool IsDraft
) : IRequest<Guid>;
