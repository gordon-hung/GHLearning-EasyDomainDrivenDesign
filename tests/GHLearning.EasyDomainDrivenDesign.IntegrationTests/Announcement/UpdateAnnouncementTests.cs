using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Draft;
using GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace GHLearning.EasyDomainDrivenDesign.IntegrationTests.Announcement;

public class UpdateAnnouncementTests
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters =
		{
			new JsonStringEnumConverter()
		}
	};

	[Fact]
	public async Task UpdateAnnouncement_ShouldReturnNoContent_WhenUpdateIsSuccessful()
	{
		var id = Guid.NewGuid();
		var source = new AnnouncementUpdateViewModel(
			Title: "Updated Title",
			Content: "Updated Content",
			PublishAt: DateTimeOffset.UtcNow,
			ExpireAt: DateTimeOffset.UtcNow.AddHours(2),
			IsDraft: true);

		var web = new WebApiApplicationTests(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<DraftAnnouncementCommand>(compare =>
					compare.Id == id &&
					compare.Title == source.Title &&
					compare.Content == source.Content &&
					compare.PublishAt == source.PublishAt &&
					compare.ExpireAt == source.ExpireAt &&
					compare.IsDraft == source.IsDraft),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Task.FromResult(id));

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		var jsonContent = JsonContent.Create(source, options: _SerializerOptions);
		var httpResponseMessage = await httpClient.PutAsync(
			$"/api/Announcement/{id}",
			jsonContent);

		Assert.Equal(System.Net.HttpStatusCode.NoContent, httpResponseMessage.StatusCode);
	}
}