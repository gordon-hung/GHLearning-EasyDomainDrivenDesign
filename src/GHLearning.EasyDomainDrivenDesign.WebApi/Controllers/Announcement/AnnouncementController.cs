using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Archived;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Deleted;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Draft;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Get;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Pending;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Published;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementController(IMediator mediator) : ControllerBase
{

	// 取得單一公告
	[HttpGet("{id:guid}")]
	public async Task<ActionResult<AnnouncementGetViewModel>> GetAsync(Guid id, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(new GetAnnouncementQuery(id), cancellationToken).ConfigureAwait(false);
		return result == null
			? (ActionResult<AnnouncementGetViewModel>)NotFound()
			: (ActionResult<AnnouncementGetViewModel>)Ok(new AnnouncementGetViewModel(
				Id: result.Id,
				Title: result.Title,
				Content: result.Content,
				Status: result.Status.Id,
				StatusName: result.Status.Name,
				PublishAt: result.PublishAt,
				ExpireAt: result.ExpireAt,
				CreatedAt: result.CreatedAt,
				UpdatedAt: result.UpdatedAt));
	}

	// 建立公告
	[HttpPost]
	public async Task<ActionResult<Guid>> CreateAsync([FromBody] AnnouncementCreateViewModel model, CancellationToken cancellationToken)
	{
		var id = await mediator.Send(new DraftAnnouncementCommand(Guid.Empty, model.Title, model.Content, model.PublishAt, model.ExpireAt, model.IsDraft), cancellationToken).ConfigureAwait(false);
		return (ActionResult<Guid>)Ok(id);
	}

	// 更新公告內容
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> UpdateAsync(Guid id, [FromBody] AnnouncementUpdateViewModel model, CancellationToken cancellationToken)
	{
		await mediator.Send(new DraftAnnouncementCommand(id, model.Title, model.Content, model.PublishAt, model.ExpireAt, model.IsDraft), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}

	// 刪除公告
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> DeletedAsync(Guid id, CancellationToken cancellationToken)
	{
		await mediator.Send(new DeletedAnnouncementCommand(id), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}

	// 公告進入待發佈狀態
	[HttpPost("{id:guid}/pending")]
	public async Task<ActionResult> SetPendingAsync(Guid id, [FromBody] AnnouncementSetPendingViewModel model, CancellationToken cancellationToken)
	{
		await mediator.Send(new PendingAnnouncementCommand(id, model.Title, model.Content, model.PublishAt, model.ExpireAt), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}

	// 公告發佈
	[HttpPost("{id:guid}/Published")]
	public async Task<ActionResult> PublishedAsync(Guid id, CancellationToken cancellationToken)
	{
		await mediator.Send(new PublishedAnnouncementCommand(id), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}

	// 公告下架
	[HttpPost("{id:guid}/Archived")]
	public async Task<ActionResult> ArchivedAsync(Guid id, CancellationToken cancellationToken)
	{
		await mediator.Send(new ArchivedAnnouncementCommand(id), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}
}
