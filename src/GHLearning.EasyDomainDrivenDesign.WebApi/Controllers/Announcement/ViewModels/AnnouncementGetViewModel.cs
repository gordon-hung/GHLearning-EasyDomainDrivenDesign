using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;

namespace GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;

public record AnnouncementGetViewModel(
	Guid Id,
	string Title,
	string Content,
	int Status,
	string StatusName,
	DateTimeOffset PublishAt,
	DateTimeOffset? ExpireAt,
	DateTimeOffset CreatedAt,
	DateTimeOffset UpdatedAt);