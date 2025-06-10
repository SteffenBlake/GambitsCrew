using System.Text.Json;
using GambitsCrew.Domain.Conditions;

namespace GambitsCrew.IntegrationTests.Parsing;

public class ConditionParsingTests : FileProviderFixture 
{
    [Fact]
    public void Parsing_DistanceCondition_Succeeds()
    {
        // Arrange
        var data = """{ "distance": [] }""";

        // Act
        var condition = JsonSerializer.Deserialize<ICondition>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as DistanceCondition;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_HppCondition_Succeeds()
    {
        // Arrange
        var data = """{ "hpp": [] }""";

        // Act
        var condition = JsonSerializer.Deserialize<ICondition>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as HppCondition;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_MppCondition_Succeeds()
    {
        // Arrange
        var data = """{ "mpp": [] }""";

        // Act
        var condition = JsonSerializer.Deserialize<ICondition>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as MppCondition;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_NameCondition_Succeeds()
    {
        // Arrange
        var data = """{ "name": [] }""";

        // Act
        var condition = JsonSerializer.Deserialize<ICondition>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as NameCondition;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_StatusCondition_Succeeds()
    {
        // Arrange
        var data = """{ "status": [] }""";

        // Act
        var condition = JsonSerializer.Deserialize<ICondition>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as StatusCondition;
        Assert.NotNull(conditionConcrete);
    }
}
