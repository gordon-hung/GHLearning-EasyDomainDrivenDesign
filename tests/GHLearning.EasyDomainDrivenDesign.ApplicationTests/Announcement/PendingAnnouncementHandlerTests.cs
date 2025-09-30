using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Pending;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace GHLearning.EasyDomainDrivenDesign.ApplicationTests.Announcement;

public class PendingAnnouncementHandlerTests
{
	[Fact]
	public async Task Handle_ShouldSetPendingAndUpdate_WhenAnnouncementFound()
	{
		// Arrange
		var announcementId = Guid.NewGuid();
		var fakeAnnouncement = Substitute.For<AnnouncementEntity>(
			announcementId,
			"原標題",
			"原內容",
			AnnouncementStatus.Draft,
			DateTimeOffset.UtcNow,
			null,
			DateTimeOffset.UtcNow,
			DateTimeOffset.UtcNow);

		var fakeRepository = Substitute.For<IAnnouncementRepository>();
		var fakeDispatcher = Substitute.For<IAnnouncementDomainEventDispatcher>();
		var command = new PendingAnnouncementCommand(
			announcementId,
			"新標題",
			"新內容",
			DateTimeOffset.UtcNow.AddDays(1),
			null);

		fakeRepository.GetAsync(announcementId, Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<AnnouncementEntity?>(fakeAnnouncement));

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		// Act
		var sut = new PendingAnnouncementHandler(fakeRepository, fakeDispatcher);
		await sut.Handle(command, CancellationToken.None);

		// Assert
		await fakeRepository.Received(1).UpdateAsync(fakeAnnouncement, Arg.Any<CancellationToken>());
		await fakeRepository.UnitOfWork.Received(1).SaveEntitiesAsync(Arg.Any<CancellationToken>());
		await fakeDispatcher.Received(1).DispatchAsync(Arg.Any<IReadOnlyCollection<IDomainEvent>>(), Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Handle_ShouldThrowArgumentNullException_WhenAnnouncementNotFound()
	{
		// Arrange
		var announcementId = Guid.NewGuid();
		var fakeRepository = Substitute.For<IAnnouncementRepository>();
		var fakeDispatcher = Substitute.For<IAnnouncementDomainEventDispatcher>();
		var command = new PendingAnnouncementCommand(
			announcementId,
			"標題",
			"內容",
			DateTimeOffset.UtcNow,
			null);

		fakeRepository.GetAsync(announcementId, Arg.Any<CancellationToken>())
			.ReturnsNull();

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		var sut = new PendingAnnouncementHandler(fakeRepository, fakeDispatcher);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() =>
			sut.Handle(command, CancellationToken.None));
	}
}
