using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;
using GHLearning.EasyDomainDrivenDesign.Infrastructure.Announcement.Tables;
using Microsoft.EntityFrameworkCore;

namespace GHLearning.EasyDomainDrivenDesign.Infrastructure.Announcement;

internal class AnnouncementLogRepository(
	EasyDomainDrivenDesignDbContext dbContext) : IAnnouncementLogRepository
{
	public IUnitOfWork UnitOfWork => dbContext;

	public Task AddAsync(AnnouncementLogEntity entity, CancellationToken cancellationToken = default)
	{
		var table = MapToTable(entity);
		return dbContext.AddAsync(table, cancellationToken).AsTask();
	}

	public Task<AnnouncementLogEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
		=> dbContext.AnnouncementLogs
			.AsNoTracking() // 避免 EF 跟 Domain 實體同時追蹤
			.FirstOrDefaultAsync(log => log.Id == id, cancellationToken)
			.ContinueWith(t => t.Result == null ? null : MapToEntity(t.Result), cancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);

	public IAsyncEnumerable<AnnouncementLogEntity> QueryAsync(Guid announcementId, CancellationToken cancellationToken = default)
		=> dbContext.AnnouncementLogs
		.Where(log => log.AnnouncementId == announcementId)
		.OrderBy(log => log.LogdAt)
		.Select(MapToEntity)
		.ToAsyncEnumerable();

	private static AnnouncementLogTable MapToTable(AnnouncementLogEntity entity)
	{
		return new AnnouncementLogTable
		{
			Id = entity.Id,
			AnnouncementId = entity.AnnouncementId,
			Status = entity.Status.Name,
			LogdAt = entity.LogdAt.UtcDateTime,
		};
	}

	private static AnnouncementLogEntity MapToEntity(AnnouncementLogTable table)
	{
		ArgumentNullException.ThrowIfNull(table);

		return new AnnouncementLogEntity(
			id: table.Id,
			announcementId: table.AnnouncementId,
			status: AnnouncementStatus.FromName(table.Status),
			logdAt: new DateTimeOffset(table.LogdAt, TimeSpan.Zero)
		);
	}
}