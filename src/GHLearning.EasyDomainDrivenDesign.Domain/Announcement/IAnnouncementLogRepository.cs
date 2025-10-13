using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

namespace GHLearning.EasyDomainDrivenDesign.Domain.Announcement;

public interface IAnnouncementLogRepository : IRepository
{
	Task AddAsync(AnnouncementLogEntity entity, CancellationToken cancellationToken = default);

	Task<AnnouncementLogEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);

	IAsyncEnumerable<AnnouncementLogEntity> QueryAsync(Guid announcementId, CancellationToken cancellationToken = default);
}