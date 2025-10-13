namespace GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;

public record AnnouncementCreateViewModel(
	string Title,
	string Content,
	DateTimeOffset PublishAt,
	DateTimeOffset? ExpireAt,
	bool IsDraft);