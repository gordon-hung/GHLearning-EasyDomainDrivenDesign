using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Draft;
using GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;
using MediatR;
using NSubstitute;
using Microsoft.Extensions.DependencyInjection;

namespace GHLearning.EasyDomainDrivenDesign.InfrastructureTests.Announcement;

public class CreateAnnouncementTests
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
	public async Task CreateAnnouncement_ShouldReturnNoContent_WhenUpdateIsSuccessful()
	{
		var source = new AnnouncementCreateViewModel(
			Title: "Title",
			Content: "Content",
			PublishAt: DateTimeOffset.UtcNow,
			ExpireAt: DateTimeOffset.UtcNow.AddHours(1),
			IsDraft: false);

		var id = Guid.NewGuid();
		var web = new WebApiApplicationTests(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<DraftAnnouncementCommand>(predicate: compare =>
					compare.Id == Guid.Empty &&
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

		var jsonContent = JsonContent.Create(source);
		var response = await httpClient.PostAsync(
			"/api/Announcement",
			jsonContent);

		var actual = JsonSerializer.Deserialize<Guid>(json: await response.Content.ReadAsStringAsync(), options: _SerializerOptions);
		Assert.Equal(id, actual);
	}
}
