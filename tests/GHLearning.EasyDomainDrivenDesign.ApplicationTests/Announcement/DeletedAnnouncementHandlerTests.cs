using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Deleted;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace GHLearning.EasyDomainDrivenDesign.ApplicationTests.Announcement;

public class DeletedAnnouncementHandlerTests
{
	[Fact]
	public async Task Handle_ShouldDeleteAnnouncement_AndDispatchEvents()
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
		var fakeDispatcher = Substitute.For<IAnnouncementDomainEventDispatcher>();
		var command = new DeletedAnnouncementCommand(announcementId);

		fakeRepository.GetAsync(announcementId, Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<AnnouncementEntity?>(fakeAnnouncement));

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		// Act
		var sut = new DeletedAnnouncementHandler(fakeRepository, fakeDispatcher);
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
		var command = new DeletedAnnouncementCommand(announcementId);

		fakeRepository.GetAsync(announcementId, Arg.Any<CancellationToken>())
			.ReturnsNull();

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		var sut = new DeletedAnnouncementHandler(fakeRepository, fakeDispatcher);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() =>
			sut.Handle(command, CancellationToken.None));
	}
}