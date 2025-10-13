using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

namespace GHLearning.EasyDomainDrivenDesign.Domain.Announcement;

public class AnnouncementStatus : DescriptionEnumeration
{
	public static readonly AnnouncementStatus Draft = new(1, "Draft", "草稿");
	public static readonly AnnouncementStatus Pending = new(2, "Pending", "待發佈");
	public static readonly AnnouncementStatus Published = new(3, "Published", "發佈");
	public static readonly AnnouncementStatus Archived = new(4, "Archived", "下架");
	public static readonly AnnouncementStatus Deleted = new(5, "Deleted", "刪除");

	private AnnouncementStatus(int id, string name, string description) : base(id, name, description) { }

	public static AnnouncementStatus FromName(string name)
		=> GetAll<AnnouncementStatus>().FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentException($"無效的公告狀態名稱: {name}");


	public static AnnouncementStatus FromId(int id)
		=> GetAll<AnnouncementStatus>().FirstOrDefault(x => x.Id == id) ?? throw new ArgumentException($"無效的公告狀態 Id: {id}");
}