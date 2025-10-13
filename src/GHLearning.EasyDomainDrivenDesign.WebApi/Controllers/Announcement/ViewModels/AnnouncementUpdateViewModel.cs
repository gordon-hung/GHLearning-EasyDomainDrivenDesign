namespace GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;

public record AnnouncementUpdateViewModel(
	string Title,
	string Content,
	DateTimeOffset PublishAt,
	DateTimeOffset? ExpireAt,
	bool IsDraft);