using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

namespace GHLearning.EasyDomainDrivenDesign.Domain.Announcement;

public class AnnouncementPublishedDomainEvent(Guid announcementId, string title) : IDomainEvent
{
	public Guid AnnouncementId { get; } = announcementId;
	public string Title { get; } = title;

	public DateTimeOffset PublishAt => DateTimeOffset.UtcNow;
}