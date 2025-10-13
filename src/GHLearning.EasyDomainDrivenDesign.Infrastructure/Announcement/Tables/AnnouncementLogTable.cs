using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GHLearning.EasyDomainDrivenDesign.Infrastructure.Announcement.Tables;

public class AnnouncementLogTable
{
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	public Guid Id { get; set; }

	[BsonElement("announcement_id")]
	[BsonRepresentation(BsonType.String)]
	public Guid AnnouncementId { get; set; }

	[BsonElement("status")]
	public string Status { get; set; }

	[BsonElement("log_at")]
	public DateTime LogdAt { get; set; }
}