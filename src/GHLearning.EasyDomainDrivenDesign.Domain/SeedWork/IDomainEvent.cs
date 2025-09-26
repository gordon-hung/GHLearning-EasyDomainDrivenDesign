namespace GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

public interface IDomainEvent
{
    DateTimeOffset PublishAt { get; }
}
