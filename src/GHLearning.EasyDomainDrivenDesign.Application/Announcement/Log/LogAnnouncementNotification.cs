using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Log;

public record LogAnnouncementNotification(
	Guid Id,
	Guid AnnouncementId,
	AnnouncementStatus Status,
	DateTimeOffset LogdAt) : INotification;
