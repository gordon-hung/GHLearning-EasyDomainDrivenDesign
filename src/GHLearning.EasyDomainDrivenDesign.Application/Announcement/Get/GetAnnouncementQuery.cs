using GHLearning.EasyDomainDrivenDesign.Domain.Announcement;
using MediatR;

namespace GHLearning.EasyDomainDrivenDesign.Application.Announcement.Get;

public record GetAnnouncementQuery(
	Guid Id) : IRequest<AnnouncementEntity?>;