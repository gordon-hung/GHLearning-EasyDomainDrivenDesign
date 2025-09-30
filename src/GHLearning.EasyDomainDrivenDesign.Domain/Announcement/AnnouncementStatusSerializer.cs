using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GHLearning.EasyDomainDrivenDesign.Domain.Announcement;

public class AnnouncementStatusSerializer : SerializerBase<AnnouncementStatus>
{
	public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, AnnouncementStatus value)
	{
		context.Writer.WriteString(value.Name); // 存成字串
	}

	public override AnnouncementStatus Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
	{
		var name = context.Reader.ReadString();
		return name switch
		{
			"Draft" => AnnouncementStatus.Draft,
			"Pending" => AnnouncementStatus.Pending,
			"Published" => AnnouncementStatus.Published,
			"Archived" => AnnouncementStatus.Archived,
			"Deleted" => AnnouncementStatus.Deleted,
			_ => throw new ArgumentOutOfRangeException($"未知的 AnnouncementStatus: {name}")
		};
	}
}
