using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

namespace GHLearning.EasyDomainDrivenDesign.Domain.Announcement;

public class AnnouncementLogDomainEvent(Guid id, Guid announcementId, string status, DateTimeOffset logAt) : IDomainEvent
{
	public Guid Id { get; } = id;
	public Guid AnnouncementId { get; } = announcementId;
	public string Status { get; } = status;

	public DateTimeOffset LogAt { get; } = logAt;

	public DateTimeOffset PublishAt => DateTimeOffset.UtcNow;
}