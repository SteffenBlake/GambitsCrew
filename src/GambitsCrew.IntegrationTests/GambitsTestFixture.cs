using System.Text.Json;
using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;
using GambitsCrew.IntegrationTests.Mocks;

namespace GambitsCrew.IntegrationTests;

public class GambitsTestFixture 
{
    public MockFileProviderService FileProvider { get; } = new();
    
    public JsonSerializerOptions JsonSerializerOptions { get; }

    public GambitsTestFixture()
    {
        JsonSerializerOptions = new JsonSerializerOptions();
        JsonSerializerOptions.Converters.Add(
            new CommandJsonConverter(FileProvider)
        );
        JsonSerializerOptions.Converters.Add(
            new ConditionJsonConverter(FileProvider)
        );
        JsonSerializerOptions.Converters.Add(
            new CrewMemberJsonConverter(FileProvider)
        );
        JsonSerializerOptions.Converters.Add(
            new GambitJsonConverter(FileProvider)
        );
        JsonSerializerOptions.Converters.Add(
            new NumberOperatorJsonConverter(FileProvider)
        );
        JsonSerializerOptions.Converters.Add(
            new StringOperatorJsonConverter(FileProvider)
        );
        JsonSerializerOptions.Converters.Add(
            new SelectorJsonConverter(FileProvider)
        );
    }
}
