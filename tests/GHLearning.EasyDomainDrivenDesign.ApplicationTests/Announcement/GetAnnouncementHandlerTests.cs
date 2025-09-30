using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Get;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace GHLearning.EasyDomainDrivenDesign.ApplicationTests.Announcement;

public class GetAnnouncementHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnAnnouncementEntity_WhenFound()
	{
		// Arrange
		var announcementId = Guid.NewGuid();
		var fakeAnnouncement = Substitute.For<AnnouncementEntity>(
			announcementId,
			"標題",
			"內容",
			AnnouncementStatus.Published,
			DateTimeOffset.UtcNow,
			null,
			DateTimeOffset.UtcNow,
			DateTimeOffset.UtcNow);

		var fakeRepository = Substitute.For<IAnnouncementRepository>();
		var query = new GetAnnouncementQuery(announcementId);

		fakeRepository.GetAsync(announcementId, Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<AnnouncementEntity?>(fakeAnnouncement));

		var sut = new GetAnnouncementHandler(fakeRepository);

		// Act
		var result = await sut.Handle(query, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(announcementId, result.Id);
		Assert.Equal("標題", result.Title);
		Assert.Equal("內容", result.Content);
		Assert.Equal(AnnouncementStatus.Published, result.Status);
	}

	[Fact]
	public async Task Handle_ShouldReturnNull_WhenAnnouncementNotFound()
	{
		// Arrange
		var announcementId = Guid.NewGuid();
		var fakeRepository = Substitute.For<IAnnouncementRepository>();
		var query = new GetAnnouncementQuery(announcementId);

		fakeRepository.GetAsync(announcementId, Arg.Any<CancellationToken>())
			.ReturnsNull();

		var sut = new GetAnnouncementHandler(fakeRepository);

		// Act
		var result = await sut.Handle(query, CancellationToken.None);

		// Assert
		Assert.Null(result);
	}
}
