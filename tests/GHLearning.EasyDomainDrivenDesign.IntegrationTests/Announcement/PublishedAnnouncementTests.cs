using System.Net;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Published;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace GHLearning.EasyDomainDrivenDesign.IntegrationTests.Announcement;

public class PublishedAnnouncementTests
{
	[Fact]
	public async Task PublishedAnnouncement_ShouldReturnNoContent_WhenPublishIsSuccessful()
	{
		// Arrange
		var id = Guid.NewGuid();
		var web = new WebApiApplicationTests(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<PublishedAnnouncementCommand>(cmd => cmd.Id == id),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Task.CompletedTask);

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		// Act
		var httpResponseMessage = await httpClient.PostAsync($"/api/Announcement/{id}/Published", null);

		// Assert
		Assert.Equal(HttpStatusCode.NoContent, httpResponseMessage.StatusCode);
	}
}