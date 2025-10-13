using System.Net.Http.Json;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Pending;
using GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace GHLearning.EasyDomainDrivenDesign.IntegrationTests.Announcement;

public class SetPendingAnnouncementTests
{
	[Fact]
	public async Task SetPending_ShouldReturnNoContent_WhenUpdateIsSuccessful()
	{
		var source = new AnnouncementSetPendingViewModel(
			Title: "待發佈標題",
			Content: "待發佈內容",
			PublishAt: DateTimeOffset.UtcNow.AddDays(1),
			ExpireAt: DateTimeOffset.UtcNow.AddDays(2)
		);

		var id = Guid.NewGuid();
		var web = new WebApiApplicationTests(builder =>
		{
			var fakeIMediator = Substitute.For<IMediator>();

			_ = fakeIMediator.Send(
				request: Arg.Is<PendingAnnouncementCommand>(compare =>
					compare.Id == id &&
					compare.Title == source.Title &&
					compare.Content == source.Content &&
					compare.PublishAt == source.PublishAt &&
					compare.ExpireAt == source.ExpireAt),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Task.CompletedTask);

			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeIMediator));
		});

		var httpClient = web.CreateDefaultClient();

		var jsonContent = JsonContent.Create(source);
		var httpResponseMessage = await httpClient.PostAsync(
			$"/api/Announcement/{id}/pending",
			jsonContent);

		Assert.Equal(System.Net.HttpStatusCode.NoContent, httpResponseMessage.StatusCode);
	}
}