using System.Net;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Deleted;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace GHLearning.EasyDomainDrivenDesign.IntegrationTests.Announcement;

public class DeleteAnnouncementTests
{
	[Fact]
	public async Task DeleteAnnouncement_ShouldReturnNoContent_WhenDeleteIsSuccessful()
	{
		// Arrange
		var id = Guid.NewGuid();
		var web = new WebApiApplicationTests(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<DeletedAnnouncementCommand>(cmd => cmd.Id == id),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Task.CompletedTask);

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		// Act
		var httpResponseMessage = await httpClient.DeleteAsync($"/api/Announcement/{id}");

		// Assert
		Assert.Equal(HttpStatusCode.NoContent, httpResponseMessage.StatusCode);
	}
}