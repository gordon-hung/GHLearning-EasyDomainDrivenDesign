using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Log;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace GHLearning.EasyDomainDrivenDesign.ApplicationTests.Announcement;

public class LogAnnouncementHandlerTests
{
	[Fact]
	public async Task Handle_ShouldNotAddLog_WhenEntityExists()
	{
		// Arrange
		var logId = Guid.NewGuid();
		var fakeEntity = Substitute.For<AnnouncementLogEntity>(
			logId,
			Guid.NewGuid(),
			AnnouncementStatus.Published,
			DateTimeOffset.UtcNow);

		var fakeRepository = Substitute.For<IAnnouncementLogRepository>();
		var notification = new LogAnnouncementNotification(
			logId,
			fakeEntity.AnnouncementId,
			fakeEntity.Status,
			DateTimeOffset.UtcNow);

		fakeRepository.GetAsync(logId, Arg.Any<CancellationToken>())
			.Returns(Task.FromResult<AnnouncementLogEntity?>(fakeEntity));

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		var sut = new LogAnnouncementHandler(fakeRepository);

		// Act
		await sut.Handle(notification, CancellationToken.None);

		// Assert
		await fakeRepository.DidNotReceive().AddAsync(Arg.Any<AnnouncementLogEntity>(), Arg.Any<CancellationToken>());
		await fakeRepository.UnitOfWork.DidNotReceive().SaveEntitiesAsync(Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Handle_ShouldAddLog_WhenEntityDoesNotExist()
	{
		// Arrange
		var logId = Guid.NewGuid();
		var announcementId = Guid.NewGuid();
		var status = AnnouncementStatus.Published;
		var notification = new LogAnnouncementNotification(
			logId,
			announcementId,
			status,
			DateTimeOffset.UtcNow);

		var fakeRepository = Substitute.For<IAnnouncementLogRepository>();
		fakeRepository.GetAsync(logId, Arg.Any<CancellationToken>())
			.ReturnsNull();

		fakeRepository.AddAsync(Arg.Any<AnnouncementLogEntity>(), Arg.Any<CancellationToken>())
			.Returns(Task.CompletedTask);

		fakeRepository.UnitOfWork.Returns(Substitute.For<IUnitOfWork>());

		var sut = new LogAnnouncementHandler(fakeRepository);

		// Act
		await sut.Handle(notification, CancellationToken.None);

		// Assert
		await fakeRepository.Received(1).AddAsync(Arg.Is<AnnouncementLogEntity>(e =>
			e.Id == logId &&
			e.AnnouncementId == announcementId &&
			e.Status == status), Arg.Any<CancellationToken>());
		await fakeRepository.UnitOfWork.Received(1).SaveEntitiesAsync(Arg.Any<CancellationToken>());
	}
}