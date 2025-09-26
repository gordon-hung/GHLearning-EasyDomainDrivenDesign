using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;
using GHLearning.EasyDomainDrivenDesign.Infrastructure.Announcement.Tables;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace GHLearning.EasyDomainDrivenDesign.Infrastructure;

public class EasyDomainDrivenDesignDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
	public DbSet<AnnouncementTable> Announcements { get; init; }

	public DbSet<AnnouncementLogTable> AnnouncementLogs { get; init; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<AnnouncementTable>().ToCollection("announcement");
		modelBuilder.Entity<AnnouncementLogTable>().ToCollection("announcement_log");
	}

	public Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
		=> base.SaveChangesAsync(cancellationToken);
}
