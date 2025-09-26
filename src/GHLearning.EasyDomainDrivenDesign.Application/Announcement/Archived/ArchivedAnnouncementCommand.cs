using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Archived;

public record ArchivedAnnouncementCommand(
    Guid Id
) : IRequest;