namespace GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

public interface IRepository : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}