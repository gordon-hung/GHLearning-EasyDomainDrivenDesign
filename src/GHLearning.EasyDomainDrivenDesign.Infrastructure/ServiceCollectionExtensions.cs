using CorrelationId;
using CorrelationId.DependencyInjection;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.Infrastructure.Announcement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GHLearning.EasyDomainDrivenDesign.Infrastructure;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptions)
		=> services.AddDbContext<EasyDomainDrivenDesignDbContext>(dbContextOptions)
		.AddCorrelationId<CustomCorrelationIdProvider>(options =>
		{
			//Learn more about configuring CorrelationId at https://github.com/stevejgordon/CorrelationId/wiki
			options.AddToLoggingScope = true;
			options.LoggingScopeKey = CorrelationIdOptions.DefaultHeader;
		})
		.Services
		.AddTransient<IAnnouncementRepository, AnnouncementRepository>()
		.AddTransient<IAnnouncementLogRepository, AnnouncementLogRepository>()
		.AddTransient<IAnnouncementDomainEventDispatcher, AnnouncementDomainEventDispatcher>();
}