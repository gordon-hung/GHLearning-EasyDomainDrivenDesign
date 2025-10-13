using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

namespace GHLearning.EasyDomainDrivenDesign.Domain.Announcement;

public interface IAnnouncementRepository : IRepository
{
	Task AddAsync(AnnouncementEntity entity, CancellationToken cancellationToken = default);

	Task UpdateAsync(AnnouncementEntity entity, CancellationToken cancellationToken = default);

	Task<AnnouncementEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);

	IAsyncEnumerable<AnnouncementEntity> GetPendingToPublishAsync(DateTimeOffset now, CancellationToken cancellationToken = default);
}