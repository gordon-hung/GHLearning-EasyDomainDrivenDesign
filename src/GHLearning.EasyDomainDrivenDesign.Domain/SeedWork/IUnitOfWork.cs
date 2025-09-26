namespace GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

public interface IUnitOfWork
{
    Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}