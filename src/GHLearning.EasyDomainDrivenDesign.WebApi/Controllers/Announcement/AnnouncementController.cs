using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Archived;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Deleted;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Draft;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Get;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Pending;
using GHLearning.EasyDomainDrivenDesign.Application.Announcement.Published;
using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyDomainDrivenDesign.WebApi.Controllers.Announcement;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementController(IMediator mediator) : ControllerBase
{

	// 取得單一公告
	[HttpGet("{id:guid}")]
	public async Task<ActionResult<AnnouncementEntity>> Get(Guid id, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(new GetAnnouncementQuery(id), cancellationToken).ConfigureAwait(false);
		return result == null ? (ActionResult<AnnouncementEntity>)NotFound() : (ActionResult<AnnouncementEntity>)Ok(result);
	}

	// 建立公告
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] AnnouncementCreateViewModel model, CancellationToken cancellationToken)
	{
		var id = await mediator.Send(new DraftAnnouncementCommand(Guid.Empty, model.Title, model.Content, model.PublishAt, model.ExpireAt, model.IsDraft), cancellationToken).ConfigureAwait(false);
		return CreatedAtAction(nameof(Get), new { id }, null);
	}

	// 更新公告內容
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] AnnouncementCreateViewModel model, CancellationToken cancellationToken)
	{
		await mediator.Send(new DraftAnnouncementCommand(id, model.Title, model.Content, model.PublishAt, model.ExpireAt, model.IsDraft), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}

	// 刪除公告
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Deleted(Guid id, CancellationToken cancellationToken)
	{
		await mediator.Send(new DeletedAnnouncementCommand(id), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}

	// 公告進入待發佈狀態
	[HttpPost("{id:guid}/pending")]
	public async Task<ActionResult> SetPending(Guid id, [FromBody] AnnouncementUpdateViewModel model, CancellationToken cancellationToken)
	{
		await mediator.Send(new PendingAnnouncementCommand(id, model.Title, model.Content, model.PublishAt, model.ExpireAt), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}

	// 公告發佈
	[HttpPost("{id:guid}/Published")]
	public async Task<ActionResult> Published(Guid id, CancellationToken cancellationToken)
	{
		await mediator.Send(new PublishedAnnouncementCommand(id), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}

	// 公告下架
	[HttpPost("{id:guid}/Archived")]
	public async Task<ActionResult> Archived(Guid id, CancellationToken cancellationToken)
	{
		await mediator.Send(new ArchivedAnnouncementCommand(id), cancellationToken).ConfigureAwait(false);
		return NoContent();
	}
}
