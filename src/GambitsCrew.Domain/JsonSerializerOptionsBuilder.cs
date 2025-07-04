using System.Text.Json;
using System.Text.Json.Serialization;
using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.Domain;

public class JsonSerializerOptionsBuilder(
    GenericListJsonConverter<ICommand> listCommandJsonConverter,
    CommandJsonConverter commandJsonConverter,
    GenericListJsonConverter<ICondition> listConditionJsonConverter,
    ConditionJsonConverter conditionJsonConverter,
    GenericListJsonConverter<ICrewMember> listCrewMemberJsonConverter,
    CrewMemberJsonConverter crewMemberJsonConverter,
    GenericListJsonConverter<IGambit> listGambitJsonConverter,
    GambitJsonConverter gambitJsonConverter,
    GenericListJsonConverter<IBuffsOperator> listBuffsOperatorJsonConverter,
    BuffsOperatorJsonConverter buffsOperatorJsonConverter,
    GenericListJsonConverter<INumberOperator> listNumberOperatorJsonConverter,
    NumberOperatorJsonConverter numberOperatorJsonConverter,
    GenericListJsonConverter<IStatusOperator> listStatusOperatorJsonConverter,
    StatusOperatorJsonConverter statusOperatorJsonConverter,
    GenericListJsonConverter<IStringOperator> listStringOperatorJsonConverter,
    StringOperatorJsonConverter stringOperatorJsonConverter,
    GenericListJsonConverter<ISelector> listSelectorJsonConverter,
    SelectorJsonConverter selectorJsonConverter
)
{
    public JsonSerializerOptions Compile()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(listCommandJsonConverter);
        options.Converters.Add(commandJsonConverter);
        options.Converters.Add(listConditionJsonConverter);
        options.Converters.Add(conditionJsonConverter);
        options.Converters.Add(listCrewMemberJsonConverter);
        options.Converters.Add(crewMemberJsonConverter);
        options.Converters.Add(listGambitJsonConverter);
        options.Converters.Add(gambitJsonConverter);
        options.Converters.Add(listBuffsOperatorJsonConverter);
        options.Converters.Add(buffsOperatorJsonConverter);
        options.Converters.Add(listNumberOperatorJsonConverter);
        options.Converters.Add(numberOperatorJsonConverter);
        options.Converters.Add(listStatusOperatorJsonConverter);
        options.Converters.Add(statusOperatorJsonConverter);
        options.Converters.Add(listStringOperatorJsonConverter);
        options.Converters.Add(stringOperatorJsonConverter);
        options.Converters.Add(listSelectorJsonConverter);
        options.Converters.Add(selectorJsonConverter);

        options.Converters.Add(new JsonStringEnumConverter());

        return options;
    }
}
