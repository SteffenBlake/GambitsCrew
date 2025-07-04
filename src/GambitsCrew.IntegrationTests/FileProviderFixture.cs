using System.Text.Json;
using GambitsCrew.Domain;
using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;
using GambitsCrew.IntegrationTests.Mocks;

namespace GambitsCrew.IntegrationTests;

public class FileProviderFixture 
{
    public MockFileProviderService FileProvider { get; } = new();
    
    public JsonSerializerOptions JsonSerializerOptions { get; }

    public FileProviderFixture()
    {
        var builder = new JsonSerializerOptionsBuilder(
            new GenericListJsonConverter<ICommand>(),
            new CommandJsonConverter(FileProvider),
            new GenericListJsonConverter<ICondition>(),
            new ConditionJsonConverter(FileProvider),
            new GenericListJsonConverter<ICrewMember>(),
            new CrewMemberJsonConverter(FileProvider),
            new GenericListJsonConverter<IGambit>(),
            new GambitJsonConverter(FileProvider),
            new GenericListJsonConverter<IBuffsOperator>(),
            new BuffsOperatorJsonConverter(FileProvider),
            new GenericListJsonConverter<INumberOperator>(),
            new NumberOperatorJsonConverter(FileProvider),
            new GenericListJsonConverter<IStatusOperator>(),
            new StatusOperatorJsonConverter(FileProvider),
            new GenericListJsonConverter<IStringOperator>(),
            new StringOperatorJsonConverter(FileProvider),
            new GenericListJsonConverter<ISelector>(),
            new SelectorJsonConverter(FileProvider)
        );

        JsonSerializerOptions = builder.Compile(); 
    }
}
