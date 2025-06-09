using System.Text.Json;
using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.Domain;

public class JsonSerializerOptionsBuilder(
    CommandJsonConverter commandJsonConverter,
    ConditionJsonConverter conditionJsonConverter,
    CrewMemberJsonConverter crewMemberJsonConverter,
    GambitJsonConverter gambitJsonConverter,
    NumberOperatorJsonConverter numberOperatorJsonConverter,
    StringOperatorJsonConverter stringOperatorJsonConverter,
    SelectorJsonConverter selectorJsonConverter
)
{
    public JsonSerializerOptions Compile()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(commandJsonConverter);
        options.Converters.Add(conditionJsonConverter);
        options.Converters.Add(crewMemberJsonConverter);
        options.Converters.Add(gambitJsonConverter);
        options.Converters.Add(numberOperatorJsonConverter);
        options.Converters.Add(stringOperatorJsonConverter);
        options.Converters.Add(selectorJsonConverter);
        return options;
    }
}
