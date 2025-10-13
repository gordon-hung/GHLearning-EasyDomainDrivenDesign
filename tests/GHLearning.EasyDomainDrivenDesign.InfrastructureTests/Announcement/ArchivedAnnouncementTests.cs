using System.Net;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Archived;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace GHLearning.EasyDomainDrivenDesign.InfrastructureTests.Announcement;

public class ArchivedAnnouncementTests
{
    [Fact]
    public async Task ArchivedAnnouncement_ShouldReturnNoContent_WhenArchiveIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var web = new WebApiApplicationTests(builder =>
        {
            var fakeIMediator = Substitute.For<IMediator>();

            _ = fakeIMediator.Send(
                request: Arg.Is<ArchivedAnnouncementCommand>(cmd => cmd.Id == id),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

            _ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
        });

        var httpClient = web.CreateDefaultClient();

        // Act
        var httpResponseMessage = await httpClient.PostAsync($"/api/Announcement/{id}/Archived", null);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, httpResponseMessage.StatusCode);
    }
}
