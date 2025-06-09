using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;
using Microsoft.Extensions.DependencyInjection;

namespace GambitsCrew.Domain.Extensions;

public static class ServiceCollectionExtensions 
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<CommandJsonConverter>()
            .AddSingleton<ConditionJsonConverter>()
            .AddSingleton<CrewMemberJsonConverter>()
            .AddSingleton<GambitJsonConverter>()
            .AddSingleton<NumberOperatorJsonConverter>()
            .AddSingleton<StringOperatorJsonConverter>()
            .AddSingleton<SelectorJsonConverter>()
            .AddSingleton<JsonSerializerOptionsBuilder>();
    }
}
