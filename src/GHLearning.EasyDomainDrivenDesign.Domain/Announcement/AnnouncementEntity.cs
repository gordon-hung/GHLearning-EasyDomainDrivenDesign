using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

namespace GHLearning.EasyDomainDrivenDesign.Domain.Announcement;

public class AnnouncementEntity : Entity, IAggregateRoot
{
	public string Title { get; private set; }
	public string Content { get; private set; }
	public AnnouncementStatus Status { get; private set; }
	public DateTimeOffset PublishAt { get; private set; }
	public DateTimeOffset? ExpireAt { get; private set; }
	public DateTimeOffset CreatedAt { get; private set; }
	public DateTimeOffset UpdatedAt { get; private set; }

	private AnnouncementEntity()
	{
		Id = SequentialGuid.SequentialGuidGenerator.Instance.NewGuid();
		Title = string.Empty;
		Content = string.Empty;
		Status = AnnouncementStatus.Draft;
		CreatedAt = DateTimeOffset.UtcNow;
		UpdatedAt = DateTimeOffset.UtcNow;
	}

	public AnnouncementEntity(
	Guid id,
	string title,
	string content,
	AnnouncementStatus status,
	DateTimeOffset publishAt,
	DateTimeOffset? expireAt,
	DateTimeOffset createdAt,
	DateTimeOffset updatedAt)
	{
		Id = id;
		Title = title;
		Content = content;
		Status = status;
		PublishAt = publishAt;
		ExpireAt = expireAt;
		CreatedAt = createdAt;
		UpdatedAt = updatedAt;
	}

	public AnnouncementEntity(
	string title,
	string content,
	DateTimeOffset publishAt,
	DateTimeOffset? expireAt)
	{
		Id = SequentialGuid.SequentialGuidGenerator.Instance.NewGuid();
		Title = title;
		Content = content;
		Status = AnnouncementStatus.Draft;
		PublishAt = publishAt;
		ExpireAt = expireAt;
		CreatedAt = DateTimeOffset.UtcNow;
		UpdatedAt = DateTimeOffset.UtcNow;

		// 觸發 Domain Event
		AddDomainEvent(
			eventItem: new AnnouncementLogDomainEvent(
				id: SequentialGuid.SequentialGuidGenerator.Instance.NewGuid(),
				announcementId: Id,
				status: Status.Name,
				logAt: UpdatedAt));
	}

	/// <summary>
	/// Sets the draft.
	/// </summary>
	/// <param name="title">The title.</param>
	/// <param name="content">The content.</param>
	/// <param name="publishAt">The publish at.</param>
	/// <param name="expireAt">The expire at.</param>
	/// <exception cref="System.InvalidOperationException">非草稿狀態不可修改</exception>
	public void SetDraft(string title, string content, DateTimeOffset publishAt, DateTimeOffset? expireAt)
	{
		if (Status != AnnouncementStatus.Draft)
			throw new InvalidOperationException("非草稿狀態不可修改");

		Title = title;
		Content = content;
		PublishAt = publishAt;
		ExpireAt = expireAt;
		UpdatedAt = DateTimeOffset.UtcNow;

		// 觸發 Domain Event
		AddDomainEvent(
			eventItem: new AnnouncementLogDomainEvent(
				id: SequentialGuid.SequentialGuidGenerator.Instance.NewGuid(),
				announcementId: Id,
				status: Status.Name,
				logAt: UpdatedAt));
	}

	/// <summary>
	/// Sets the pending.
	/// </summary>
	/// <param name="title">The title.</param>
	/// <param name="content">The content.</param>
	/// <param name="publishAt">The publish at.</param>
	/// <param name="expireAt">The expire at.</param>
	/// <exception cref="System.InvalidOperationException">只有草稿狀態可轉待發佈</exception>
	public void SetPending(string title, string content, DateTimeOffset publishAt, DateTimeOffset? expireAt)
	{
		if (Status != AnnouncementStatus.Draft)
			throw new InvalidOperationException("只有草稿狀態可轉待發佈");

		Title = title;
		Content = content;
		PublishAt = publishAt;
		ExpireAt = expireAt;
		Status = AnnouncementStatus.Pending;
		UpdatedAt = DateTimeOffset.UtcNow;

		// 觸發 Domain Event
		AddDomainEvent(
			eventItem: new AnnouncementLogDomainEvent(
				id: SequentialGuid.SequentialGuidGenerator.Instance.NewGuid(),
				announcementId: Id,
				status: Status.Name,
				logAt: UpdatedAt));
	}

	/// <summary>
	/// Sets the pending.
	/// </summary>
	/// <exception cref="System.InvalidOperationException">只有草稿狀態可轉待發佈</exception>
	public void SetPending()
	{
		if (Status != AnnouncementStatus.Draft)
			throw new InvalidOperationException("只有草稿狀態可轉待發佈");

		Status = AnnouncementStatus.Pending;
		UpdatedAt = DateTimeOffset.UtcNow;

		// 觸發 Domain Event
		AddDomainEvent(
			eventItem: new AnnouncementLogDomainEvent(
				id: SequentialGuid.SequentialGuidGenerator.Instance.NewGuid(),
				announcementId: Id,
				status: Status.Name,
				logAt: UpdatedAt));
	}

	/// <summary>
	/// Sets the publish.
	/// </summary>
	/// <exception cref="System.InvalidOperationException">只有待發佈狀態可發佈</exception>
	public void SetPublish()
	{
		if (Status != AnnouncementStatus.Pending)
			throw new InvalidOperationException("只有待發佈狀態可發佈");

		Status = AnnouncementStatus.Published;
		UpdatedAt = DateTimeOffset.UtcNow;

		// 觸發 Domain Event
		AddDomainEvent(new AnnouncementPublishedDomainEvent(Id, Title));
		AddDomainEvent(
			eventItem: new AnnouncementLogDomainEvent(
				id: SequentialGuid.SequentialGuidGenerator.Instance.NewGuid(),
				announcementId: Id,
				status: Status.Name,
				logAt: UpdatedAt));
	}

	/// <summary>
	/// Sets the archive.
	/// </summary>
	/// <exception cref="System.InvalidOperationException">只有已發佈公告可下架</exception>
	public void SetArchive()
	{
		if (Status != AnnouncementStatus.Published)
			throw new InvalidOperationException("只有已發佈公告可下架");

		Status = AnnouncementStatus.Archived;
		UpdatedAt = DateTimeOffset.UtcNow;
		// 觸發 Domain Event
		AddDomainEvent(
			eventItem: new AnnouncementLogDomainEvent(
				id: SequentialGuid.SequentialGuidGenerator.Instance.NewGuid(),
				announcementId: Id,
				status: Status.Name,
				logAt: UpdatedAt));
	}

	/// <summary>
	/// Sets the delete.
	/// </summary>
	public void SetDelete()
	{
		Status = AnnouncementStatus.Deleted;
		UpdatedAt = DateTimeOffset.UtcNow;
		// 觸發 Domain Event
		AddDomainEvent(
			eventItem: new AnnouncementLogDomainEvent(
				id: SequentialGuid.SequentialGuidGenerator.Instance.NewGuid(),
				announcementId: Id,
				status: Status.Name,
				logAt: UpdatedAt));
	}

	// 自動狀態更新 (可以放在 Scheduler 或 Application Service)
	public void UpdateStatusByDate(DateTimeOffset now)
	{
		if (Status == AnnouncementStatus.Published && ExpireAt.HasValue && ExpireAt.Value <= now)
		{
			Status = AnnouncementStatus.Archived;
			UpdatedAt = DateTimeOffset.UtcNow;
			// 觸發 Domain Event
			AddDomainEvent(
				eventItem: new AnnouncementLogDomainEvent(
					id: SequentialGuid.SequentialGuidGenerator.Instance.NewGuid(),
					announcementId: Id,
					status: Status.Name,
					logAt: UpdatedAt));
		}
	}
}