using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GHLearning.EasyDomainDrivenDesign.Infrastructure.Announcement.Tables;

public class AnnouncementTable
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("title")]
    public required string Title { get; set; }

    [BsonElement("content")]
    public required string Content { get; set; }

    [BsonElement("status")]
    public required string Status { get; set; }

    [BsonElement("publish_at")]
    public DateTime PublishAt { get; set; }

    [BsonElement("expire_at")]
    public DateTime? ExpireAt { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
