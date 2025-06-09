using System.Text.Json;
using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Deployments;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.IntegrationTests;

public class ParsingTests : GambitsTestFixture
{
    [Fact]
    public async Task SerializerFullStackTests()
    {
        // Arrange
        var deploymentId = "TestDeployment";
        
        var member1Id = "Member1";
        var member2Id = "Member2";


        var gambit1Id = "Gambit1";
        var gambit2Id = "Gambit2";
        var gambit3Id = "Gambit3";


        var btSelectorId = "BtSelector";
        var meSelectorId = "MeSelector";
        var ptSelectorId = "PtSelector";
        

        var hppConditionId = "HppCondition";
        var nameConditionId = "NameCondition";
        var distanceConditionId = "DistanceCondition";


        var ltOperatorId = "LtOperator";
        var ltOperatorVal = 5;

        var containsOperatorId = "ContainsOperator";
        var containsOperatorVal = "Contains";

        var eqOperatorId = "EqOperator";
        var eqOperatorVal = 10;


        var abilityCommandId = "AbilityCommand";
        var abilityCommandName = "Provoke";
        var abilityCommandTarget = "<me>";
        var abilityCommandWait = 5;

        var assistCommandId = "AssistCommand";
        var assistCommandTarget = "Steve";
        var assistCommandWait = 10;

        var castCommandId = "CastCommand";
        var castCommandName = "Cure IV";
        var castCommandTarget = "<t>";
        var castCommandWait = 15;

        FileProvider.Deployments[deploymentId] = 
$$"""
{
    "crew": [ "{{member1Id}}", "{{member2Id}}" ]
}
""";


        FileProvider.CrewMembers[member1Id] = 
$$"""
{
    "name": "{{member1Id}}",
    "gambits": [ "{{gambit1Id}}", "{{gambit2Id}}" ]
}
""";
        FileProvider.CrewMembers[member2Id] = 
$$"""
{
    "name": "{{member2Id}}",
    "gambits": [ "{{gambit3Id}}" ]
}
""";


        FileProvider.Gambits[gambit1Id] = 
$$"""
{
    "when": "{{btSelectorId}}",
    "do": "{{abilityCommandId}}" 
}
""";
        FileProvider.Gambits[gambit2Id] = 
$$"""
{
    "when": "{{meSelectorId}}",
    "do": "{{assistCommandId}}" 
}
""";
        FileProvider.Gambits[gambit3Id] = 
$$"""
{
    "when": "{{ptSelectorId}}",
    "do": "{{castCommandId}}" 
}
""";


        FileProvider.Selectors[btSelectorId] = 
$$"""
{
    "bt": [ "{{hppConditionId}}" ]
}
""";
        FileProvider.Selectors[meSelectorId] = 
$$"""
{
    "me": [ "{{nameConditionId}}" ]
}
""";
        FileProvider.Selectors[ptSelectorId] = 
$$"""
{
    "p": [ "{{distanceConditionId}}" ]
}
""";


        FileProvider.Conditions[hppConditionId] =
$$"""
{
    "hpp": [ "{{ltOperatorId}}" ]
}
""";
        FileProvider.Conditions[nameConditionId] =
$$"""
{
    "name": [ "{{containsOperatorId}}" ]
}
""";
        FileProvider.Conditions[distanceConditionId] =
$$"""
{
    "distance": [ "{{eqOperatorId}}" ]
}
""";


        FileProvider.Operators[ltOperatorId] =
$$"""
{
    "lt": {{ltOperatorVal}}
}
""";
        FileProvider.Operators[containsOperatorId] =
$$"""
{
    "contains": {{containsOperatorVal}}
}
""";
        FileProvider.Operators[eqOperatorId] =
$$"""
{
    "eq": {{eqOperatorVal}}
}
""";


        FileProvider.Commands[abilityCommandId] = 
$$"""
{
    "ja": {
        "name": "{{abilityCommandName}}",
        "target": "{{abilityCommandTarget}}",
        "wait": {{abilityCommandWait}}
    }
}
""";

        FileProvider.Commands[assistCommandId] = 
$$"""
{
    "assist": {
        "target": "{{assistCommandTarget}}",
        "wait": {{assistCommandWait}}
    }
}
""";

        FileProvider.Commands[castCommandId] = 
$$"""
{
    "assist": {
        "name": "{{castCommandName}}",
        "target": "{{castCommandTarget}}",
        "wait": {{castCommandWait}}
    }
}
""";

        // Act
        var deployment = await JsonSerializer.DeserializeAsync<Deployment>(
            FileProvider.GetDeployment(deploymentId), JsonSerializerOptions
        );

        // Assert
        Assert.NotNull(deployment);
        var member1 = Assert.Single(deployment.Crew, c => c.Name == member1Id);
        var member2 = Assert.Single(deployment.Crew, c => c.Name == member2Id);

        Assert.Equal(2, member1.Gambits.Count);
        var gambit1 = member1.Gambits[0];
        var gambit2 = member1.Gambits[1];

        var gambit3 = Assert.Single(member2.Gambits);

        var btSelector = gambit1.When as BattleTargetSelector;
        var meSelector = gambit2.When as MeSelector;
        var ptSelector = gambit3.When as PartySelector;

        Assert.NotNull(btSelector);
        Assert.NotNull(meSelector);
        Assert.NotNull(ptSelector);

        var hppCondition = Assert.Single(btSelector.BT) as HppCondition;
        var nameCondition = Assert.Single(meSelector.Me) as NameCondition;
        var distanceCondition = Assert.Single(ptSelector.P) as DistanceCondition;

        Assert.NotNull(hppCondition);
        Assert.NotNull(nameCondition);
        Assert.NotNull(distanceCondition);

        var ltOperator = Assert.Single(hppCondition.HPP) as NumberLessThanOperator;
        var containsOperator = Assert.Single(nameCondition.Name) as StringContainsOperator;
        var eqOperator = Assert.Single(distanceCondition.Distance) as NumberEqualsOperator;

        Assert.NotNull(ltOperator);
        Assert.NotNull(containsOperator);
        Assert.NotNull(eqOperator);

        Assert.Equal(ltOperatorVal, ltOperator.LT);
        Assert.Equal(containsOperatorVal, containsOperator.Contains);
        Assert.Equal(eqOperatorVal, eqOperator.EQ);

        var abilityCommand = gambit1.Do as AbilityCommand;
        var assistCommand = gambit2.Do as AssistCommand;
        var castCommand = gambit3.Do as CastCommand;

        Assert.NotNull(abilityCommand);
        Assert.NotNull(assistCommand);
        Assert.NotNull(castCommand);

        Assert.Equal(abilityCommandName, abilityCommand.JA.Name);
        Assert.Equal(abilityCommandTarget, abilityCommand.JA.Target);
        Assert.Equal(abilityCommandWait, abilityCommand.JA.Wait);

        Assert.Equal(assistCommandTarget, assistCommand.Assist.Target);
        Assert.Equal(assistCommandWait, assistCommand.Assist.Wait);

        Assert.Equal(castCommandName, castCommand.Cast.Name);
        Assert.Equal(castCommandTarget, castCommand.Cast.Target);
        Assert.Equal(castCommandWait, castCommand.Cast.Wait);
    }
}
