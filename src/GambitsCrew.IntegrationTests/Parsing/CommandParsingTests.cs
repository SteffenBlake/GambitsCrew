using System.Text.Json;
using GambitsCrew.Domain.Commands;

namespace GambitsCrew.IntegrationTests.Parsing;

public class CommandParsingTests : FileProviderFixture 
{
    [Fact]
    public void Parsing_AbilityCommand_Succeeds()
    {
        // Arrange
        var commandName = "TestName";
        var commandTarget = "TestTarget";
        var commandWait = 10;
        var data = 
$$"""
{
    "ja": {
        "name": "{{commandName}}",
        "target": "{{commandTarget}}",
        "wait": {{commandWait}}
    }
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var ability = command as AbilityCommand;
        Assert.NotNull(ability);
        Assert.Equal(commandName, ability.JA.Name);
        Assert.Equal(commandTarget, ability.JA.Target);
        Assert.Equal(commandWait, ability.JA.Wait);
    }

    [Fact]
    public void Parsing_AllCommand_Succeeds()
    {
        // Arrange
        var data = 
$$"""
{
    "all": []
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var all = command as AllCommand;
        Assert.NotNull(all);
        Assert.NotNull(all.All);
        Assert.Empty(all.All);
    }

    [Fact]
    public void Parsing_AnyCommand_Succeeds()
    {
        // Arrange
        var data = 
$$"""
{
    "any": []
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var any = command as AnyCommand;
        Assert.NotNull(any);
        Assert.NotNull(any.Any);
        Assert.Empty(any.Any);
    }

    [Fact]
    public void Parsing_AssistCommand_Succeeds()
    {
        // Arrange
        var commandTarget = "TestTarget";
        var commandWait = 10;
        var data = 
$$"""
{
    "assist": {
        "target": "{{commandTarget}}",
        "wait": {{commandWait}}
    }
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var assist = command as AssistCommand;
        Assert.NotNull(assist);
        Assert.Equal(commandTarget, assist.Assist.Target);
        Assert.Equal(commandWait, assist.Assist.Wait);
    }

    [Fact]
    public void Parsing_AttackCommand_Succeeds()
    {
        // Arrange
        var commandWait = 10;
        var data = 
$$"""
{
    "attack": {
        "wait": {{commandWait}}
    }
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var attack = command as AttackCommand;
        Assert.NotNull(attack);
        Assert.Equal(commandWait, attack.Attack.Wait);
    }

    [Fact]
    public void Parsing_CastCommand_Succeeds()
    {
        // Arrange
        var commandName = "TestName";
        var commandTarget = "TestTarget";
        var commandWait = 10;
        var data = 
$$"""
{
    "cast": {
        "name": "{{commandName}}",
        "target": "{{commandTarget}}",
        "wait": {{commandWait}}
    }
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var cast = command as CastCommand;
        Assert.NotNull(cast);
        Assert.Equal(commandName, cast.Cast.Name);
        Assert.Equal(commandTarget, cast.Cast.Target);
        Assert.Equal(commandWait, cast.Cast.Wait);
    }

    [Fact]
    public void Parsing_ExecuteCommand_Succeeds()
    {
        // Arrange
        var commandExecute = "TestExecute";
        var data = 
$$"""
{
    "execute": "{{commandExecute}}"
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var execute = command as ExecuteCommand;
        Assert.NotNull(execute);
        Assert.Equal(commandExecute, execute.Execute);
    }

    [Fact]
    public void Parsing_ItemCommand_Succeeds()
    {
        // Arrange
        var commandName = "TestName";
        var commandTarget = "TestTarget";
        var commandWait = 10;
        var data = 
$$"""
{
    "item": {
        "name": "{{commandName}}",
        "target": "{{commandTarget}}",
        "wait": {{commandWait}}
    }
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var cast = command as ItemCommand;
        Assert.NotNull(cast);
        Assert.Equal(commandName, cast.Item.Name);
        Assert.Equal(commandTarget, cast.Item.Target);
        Assert.Equal(commandWait, cast.Item.Wait);
    }

    [Fact]
    public void Parsing_PetCommand_Succeeds()
    {
        // Arrange
        var commandName = "TestName";
        var commandTarget = "TestTarget";
        var commandWait = 10;
        var data = 
$$"""
{
    "pet": {
        "name": "{{commandName}}",
        "target": "{{commandTarget}}",
        "wait": {{commandWait}}
    }
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var pet = command as PetCommand;
        Assert.NotNull(pet);
        Assert.Equal(commandName, pet.Pet.Name);
        Assert.Equal(commandTarget, pet.Pet.Target);
        Assert.Equal(commandWait, pet.Pet.Wait);
    }

    [Fact]
    public void Parsing_WeaponskillCommand_Succeeds()
    {
        // Arrange
        var commandName = "TestName";
        var commandTp = 1500;
        var commandWait = 10;
        var data = 
$$"""
{
    "ws": {
        "name": "{{commandName}}",
        "tp": {{commandTp}},
        "wait": {{commandWait}}
    }
}
""";

        // Act
        var command = JsonSerializer.Deserialize<ICommand>(data, JsonSerializerOptions);

        // Assert
        var ws = command as WeaponskillCommand;
        Assert.NotNull(ws);
        Assert.Equal(commandName, ws.WS.Name);
        Assert.Equal(commandTp, ws.WS.TP);
        Assert.Equal(commandWait, ws.WS.Wait);
    }
}
