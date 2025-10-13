using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;
using GHLearning.EasyDomainDrivenDesign.Infrastructure.Announcement.Tables;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace GHLearning.EasyDomainDrivenDesign.Infrastructure.Announcement;

internal class AnnouncementRepository(
	EasyDomainDrivenDesignDbContext dbContext) : IAnnouncementRepository
{
	public IUnitOfWork UnitOfWork => dbContext;

	public Task AddAsync(AnnouncementEntity entity, CancellationToken cancellationToken = default)
	{
		var table = MapToTable(entity);
		return dbContext.AddAsync(table, cancellationToken).AsTask();
	}

	public async Task<AnnouncementEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var table = await dbContext.Announcements
			.AsNoTracking() // 避免 EF 跟 Domain 實體同時追蹤
			.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
			.ConfigureAwait(false);

		return table == null ? null : MapToEntity(table);
	}

	public IAsyncEnumerable<AnnouncementEntity> GetPendingToPublishAsync(DateTimeOffset now, CancellationToken cancellationToken = default)
		=> dbContext.Announcements
		.Where(a => a.Status == AnnouncementStatus.Pending.Name && a.PublishAt <= now.UtcDateTime)
		.Select(a => MapToEntity(a))
		.AsAsyncEnumerable();

	public async Task UpdateAsync(AnnouncementEntity entity, CancellationToken cancellationToken = default)
	{
		var tracked = await dbContext.Announcements.FindAsync([entity.Id], cancellationToken).ConfigureAwait(false);
		if (tracked != null)
		{
			dbContext.Entry(tracked).CurrentValues.SetValues(MapToTable(entity));
		}
		else
		{
			dbContext.Update(MapToTable(entity));
		}
	}

	private static AnnouncementTable MapToTable(AnnouncementEntity entity)
	{
		return new AnnouncementTable
		{
			Id = entity.Id,
			Title = entity.Title,
			Content = entity.Content,
			Status = entity.Status.Name,
			PublishAt = entity.PublishAt.UtcDateTime,
			ExpireAt = entity.ExpireAt?.UtcDateTime,
			CreatedAt = entity.CreatedAt.UtcDateTime,
			UpdatedAt = entity.UpdatedAt.UtcDateTime
		};
	}

	private static AnnouncementEntity MapToEntity(AnnouncementTable table)
	{
		ArgumentNullException.ThrowIfNull(table);

		return new AnnouncementEntity(
			id: table.Id,
			title: table.Title,
			content: table.Content,
			status: AnnouncementStatus.FromName(table.Status),
			publishAt: new DateTimeOffset(table.PublishAt, TimeSpan.Zero),
			expireAt: table.ExpireAt.HasValue ? new DateTimeOffset(table.ExpireAt.Value, TimeSpan.Zero) : null,
			createdAt: new DateTimeOffset(table.CreatedAt, TimeSpan.Zero),
			updatedAt: new DateTimeOffset(table.UpdatedAt, TimeSpan.Zero)
		);
	}
}