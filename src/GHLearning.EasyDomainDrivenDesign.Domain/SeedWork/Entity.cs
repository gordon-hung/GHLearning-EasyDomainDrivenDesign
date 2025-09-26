namespace GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    private List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents ??= [];
        _domainEvents.Add(eventItem);
    }
}