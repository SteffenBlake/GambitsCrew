using System.Text.Json;
using EliteMMO.API;
using GambitsCrew.Domain.Operators;

namespace GambitsCrew.IntegrationTests.Parsing;

public class OperatorParsingTests : FileProviderFixture
{ 
    [Fact]
    public void Parsing_BuffsContainsOperator_Succeeds()
    {
        // Arrange
        var data = """{ "contains": 5 }""";

        // Act
        var condition = JsonSerializer.Deserialize<IBuffsOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as BuffsContainsOperator;
        Assert.NotNull(conditionConcrete);
        Assert.Equal(5, conditionConcrete.Contains);
    }
    [Fact]
    public void Parsing_NumberEqualsOperator_Succeeds()
    {
        // Arrange
        var data = """{ "eq": 5 }""";

        // Act
        var condition = JsonSerializer.Deserialize<INumberOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as NumberEqualsOperator;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_NumberGreaterThenOperator_Succeeds()
    {
        // Arrange
        var data = """{ "gt": 5 }""";

        // Act
        var condition = JsonSerializer.Deserialize<INumberOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as NumberGreaterThanOperator;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_NumberGreaterThanOrEqualsOperator_Succeeds()
    {
        // Arrange
        var data = """{ "gte": 5 }""";

        // Act
        var condition = JsonSerializer.Deserialize<INumberOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as NumberGreaterThanOrEqualsOperator;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_NumberLessThan_Succeeds()
    {
        // Arrange
        var data = """{ "lt": 5 }""";

        // Act
        var condition = JsonSerializer.Deserialize<INumberOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as NumberLessThanOperator;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_NumberLessThanOrEqualsOperator_Succeeds()
    {
        // Arrange
        var data = """{ "lte": 5 }""";

        // Act
        var condition = JsonSerializer.Deserialize<INumberOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as NumberLessThanOrEqualsOperator;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_NumberNotEqualsOperator_Succeeds()
    {
        // Arrange
        var data = """{ "ne": 5 }""";

        // Act
        var condition = JsonSerializer.Deserialize<INumberOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as NumberNotEqualsOperator;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_StatusHasFlagOperator_Succeeds()
    {
        // Arrange
        var data = """{ "hasFlag": "Chocobo" }""";

        // Act
        var condition = JsonSerializer.Deserialize<IStatusOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as StatusHasFlagOperator;
        Assert.NotNull(conditionConcrete);
        Assert.Equal(EntityStatus.Chocobo, conditionConcrete.HasFlag);
    }

    [Fact]
    public void Parsing_StringContainsOperator_Succeeds()
    {
        // Arrange
        var data = """{ "contains": "a" }""";

        // Act
        var condition = JsonSerializer.Deserialize<IStringOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as StringContainsOperator;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_StringEqualsOperator_Succeeds()
    {
        // Arrange
        var data = """{ "eq": "a" }""";

        // Act
        var condition = JsonSerializer.Deserialize<IStringOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as StringEqualsOperator;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_StringNotEqualsOperator_Succeeds()
    {
        // Arrange
        var data = """{ "ne": "a" }""";

        // Act
        var condition = JsonSerializer.Deserialize<IStringOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as StringNotEqualsOperator;
        Assert.NotNull(conditionConcrete);
    }

    [Fact]
    public void Parsing_StringToLowerOperator_Succeeds()
    {
        // Arrange
        var data = """{ "lower": {"eq": "a"} }""";

        // Act
        var condition = JsonSerializer.Deserialize<IStringOperator>(data, JsonSerializerOptions);

        // Assert
        var conditionConcrete = condition as StringToLowerOperator;
        Assert.NotNull(conditionConcrete);
    }
}
