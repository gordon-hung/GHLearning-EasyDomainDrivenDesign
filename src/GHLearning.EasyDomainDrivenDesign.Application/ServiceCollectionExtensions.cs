using GHLearning.EasyDomainDrivenDesign.Application.Abstractions.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GHLearning.EasyDomainDrivenDesign.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(HandleTracingPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        return services;
    }
}
