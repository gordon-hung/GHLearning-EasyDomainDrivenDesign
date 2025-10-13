using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

namespace GHLearning.EasyDomainDrivenDesign.Domain.Announcement;

public class AnnouncementLogEntity : Entity, IAggregateRoot
{
	public Guid AnnouncementId { get; private set; }

	public AnnouncementStatus Status { get; private set; }

	public DateTimeOffset LogdAt { get; private set; }

	public AnnouncementLogEntity(Guid id, Guid announcementId, AnnouncementStatus status, DateTimeOffset logdAt)
	{
		Id = id;
		AnnouncementId = announcementId;
		Status = status;
		LogdAt = logdAt;
	}
}