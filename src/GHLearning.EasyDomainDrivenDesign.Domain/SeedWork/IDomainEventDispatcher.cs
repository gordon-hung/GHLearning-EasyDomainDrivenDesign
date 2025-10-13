namespace GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

public interface IDomainEventDispatcher
{
	Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}