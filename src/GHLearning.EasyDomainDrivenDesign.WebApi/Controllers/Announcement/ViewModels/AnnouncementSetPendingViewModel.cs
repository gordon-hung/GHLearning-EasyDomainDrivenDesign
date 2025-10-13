namespace GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;

public record AnnouncementSetPendingViewModel(
	string Title,
	string Content,
	DateTimeOffset PublishAt,
	DateTimeOffset? ExpireAt);