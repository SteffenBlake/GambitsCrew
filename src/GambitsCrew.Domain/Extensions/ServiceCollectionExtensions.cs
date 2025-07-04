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
            .AddSingleton<GenericListJsonConverter<ICommand>>()
            .AddSingleton<CommandJsonConverter>()
            .AddSingleton<GenericListJsonConverter<ICondition>>()
            .AddSingleton<ConditionJsonConverter>()
            .AddSingleton<GenericListJsonConverter<ICrewMember>>()
            .AddSingleton<CrewMemberJsonConverter>()
            .AddSingleton<GenericListJsonConverter<IGambit>>()
            .AddSingleton<GambitJsonConverter>()
            .AddSingleton<GenericListJsonConverter<IBuffsOperator>>()
            .AddSingleton<BuffsOperatorJsonConverter>()
            .AddSingleton<GenericListJsonConverter<INumberOperator>>()
            .AddSingleton<NumberOperatorJsonConverter>()
            .AddSingleton<GenericListJsonConverter<IStatusOperator>>()
            .AddSingleton<StatusOperatorJsonConverter>()
            .AddSingleton<GenericListJsonConverter<IStringOperator>>()
            .AddSingleton<StringOperatorJsonConverter>()
            .AddSingleton<GenericListJsonConverter<ISelector>>()
            .AddSingleton<SelectorJsonConverter>()
            .AddSingleton<JsonSerializerOptionsBuilder>();
    }
}
