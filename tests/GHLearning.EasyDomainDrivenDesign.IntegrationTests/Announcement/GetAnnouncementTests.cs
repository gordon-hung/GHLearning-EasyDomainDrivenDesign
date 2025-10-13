using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Get;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace GHLearning.EasyDomainDrivenDesign.IntegrationTests.Announcement;

public class GetAnnouncementTests
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
	public async Task GetAnnouncement_ShouldReturnAnnouncement_WhenFound()
	{
		// Arrange
		var id = Guid.NewGuid();
		var expected = new AnnouncementEntity(
			id: id,
			title: "Test Title",
			content: "Test Content",
			status: AnnouncementStatus.Published,
			publishAt: DateTimeOffset.UtcNow,
			expireAt: DateTimeOffset.UtcNow.AddDays(1),
			createdAt: DateTimeOffset.UtcNow.AddDays(-1),
			updatedAt: DateTimeOffset.UtcNow);

		var web = new WebApiApplicationTests(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();
			fakeIMediator.Send(
				Arg.Is<GetAnnouncementQuery>(q => q.Id == id),
				Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<AnnouncementEntity?>(expected));

			builder.ConfigureServices(services => services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		// Act
		var response = await httpClient.GetAsync($"/api/Announcement/{id}");
		var json = await response.Content.ReadAsStringAsync();
		var actual = JsonSerializer.Deserialize<AnnouncementGetViewModel>(json: await response.Content.ReadAsStringAsync(), options: _SerializerOptions);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(actual);
		Assert.Equal(expected.Id, actual!.Id);
		Assert.Equal(expected.Title, actual.Title);
		Assert.Equal(expected.Content, actual.Content);
		Assert.Equal(expected.Status.Id, actual.Status);
		Assert.Equal(expected.Status.Name, actual.StatusName);
	}

	[Fact]
	public async Task GetAnnouncement_ShouldReturnNotFound_WhenNotExists()
	{
		// Arrange
		var id = Guid.NewGuid();

		var web = new WebApiApplicationTests(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();
			fakeIMediator.Send(
				Arg.Is<GetAnnouncementQuery>(q => q.Id == id),
				Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<AnnouncementEntity?>(null));

			builder.ConfigureServices(services => services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		// Act
		var response = await httpClient.GetAsync($"/api/Announcement/{id}");

		// Assert
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}