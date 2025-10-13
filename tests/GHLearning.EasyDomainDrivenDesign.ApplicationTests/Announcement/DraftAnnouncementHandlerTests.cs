using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Draft;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace GHLearning.EasyDomainDrivenDesign.ApplicationTests.Announcement;

public class DraftAnnouncementHandlerTests
{
	[Fact]
	public async Task Handle_ShouldAddDraftAnnouncement_AndDispatchEvents()
	{
		// Arrange
		var fakeRepository = Substitute.For<IAnnouncementRepository>();
		var fakeDispatcher = Substitute.For<IAnnouncementDomainEventDispatcher>();
		var command = new DraftAnnouncementCommand(
			Guid.Empty,
			"標題",
			"內容",
			DateTimeOffset.UtcNow,
			null,
			true);

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		// Act
		var sut = new DraftAnnouncementHandler(fakeRepository, fakeDispatcher);
		var result = await sut.Handle(command, CancellationToken.None);

		// Assert
		await fakeRepository.Received(1).AddAsync(Arg.Any<AnnouncementEntity>(), Arg.Any<CancellationToken>());
		await fakeRepository.UnitOfWork.Received(1).SaveEntitiesAsync(Arg.Any<CancellationToken>());
		await fakeDispatcher.Received(1).DispatchAsync(Arg.Any<IReadOnlyCollection<IDomainEvent>>(), Arg.Any<CancellationToken>());
		Assert.NotEqual(Guid.Empty, result);
	}

	[Fact]
	public async Task Handle_ShouldUpdateDraftAnnouncement_AndDispatchEvents()
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
		var command = new DraftAnnouncementCommand(
			announcementId,
			"新標題",
			"新內容",
			DateTimeOffset.UtcNow.AddDays(1),
			null,
			true);

		fakeRepository.GetAsync(announcementId, Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<AnnouncementEntity?>(fakeAnnouncement));

		fakeRepository.UpdateAsync(fakeAnnouncement, Arg.Any<CancellationToken>())
			.Returns(Task.CompletedTask);

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		// Act
		var sut = new DraftAnnouncementHandler(fakeRepository, fakeDispatcher);
		var result = await sut.Handle(command, CancellationToken.None);

		// Assert
		await fakeRepository.Received(1).UpdateAsync(fakeAnnouncement, Arg.Any<CancellationToken>());
		await fakeRepository.UnitOfWork.Received(1).SaveEntitiesAsync(Arg.Any<CancellationToken>());
		await fakeDispatcher.Received(1).DispatchAsync(Arg.Any<IReadOnlyCollection<IDomainEvent>>(), Arg.Any<CancellationToken>());
		Assert.Equal(announcementId, result);
	}

	[Fact]
	public async Task Handle_ShouldSetPending_WhenIsDraftIsFalse()
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
		var command = new DraftAnnouncementCommand(
			announcementId,
			"新標題",
			"新內容",
			DateTimeOffset.UtcNow.AddDays(1),
			null,
			false);

		fakeRepository.GetAsync(announcementId, Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<AnnouncementEntity?>(fakeAnnouncement));

		fakeRepository.UpdateAsync(fakeAnnouncement, Arg.Any<CancellationToken>())
			.Returns(Task.CompletedTask);

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		// Act
		var sut = new DraftAnnouncementHandler(fakeRepository, fakeDispatcher);
		var result = await sut.Handle(command, CancellationToken.None);

		// Assert
		await fakeRepository.Received(2).UpdateAsync(fakeAnnouncement, Arg.Any<CancellationToken>());
		await fakeRepository.UnitOfWork.Received(1).SaveEntitiesAsync(Arg.Any<CancellationToken>());
		await fakeDispatcher.Received(1).DispatchAsync(Arg.Any<IReadOnlyCollection<IDomainEvent>>(), Arg.Any<CancellationToken>());
		Assert.Equal(announcementId, result);
	}

	[Fact]
	public async Task Handle_ShouldThrowArgumentNullException_WhenAnnouncementNotFound()
	{
		// Arrange
		var announcementId = Guid.NewGuid();
		var fakeRepository = Substitute.For<IAnnouncementRepository>();
		var fakeDispatcher = Substitute.For<IAnnouncementDomainEventDispatcher>();
		var command = new DraftAnnouncementCommand(
			announcementId,
			"標題",
			"內容",
			DateTimeOffset.UtcNow,
			null,
			true);

		fakeRepository.GetAsync(announcementId, Arg.Any<CancellationToken>())
			.ReturnsNull();

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		var sut = new DraftAnnouncementHandler(fakeRepository, fakeDispatcher);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() =>
			sut.Handle(command, CancellationToken.None));
	}
}