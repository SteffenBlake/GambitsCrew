using System.Text.Json;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.IntegrationTests.Parsing;

public class SelectorParsingTests : FileProviderFixture 
{
    [Fact]
    public void Parsing_AllianceOthersSelector_Succeeds()
    {
        // Arrange
        var data = """{ "ax": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as AllianceOtherSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_AllianceSelector_Succeeds()
    {
        // Arrange
        var data = """{ "a": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as AllianceSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_AndSelector_Succeeds()
    {
        // Arrange
        var data = """{ "and": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as AndSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_BattleTargetSelector_Succeeds()
    {
        // Arrange
        var data = """{ "bt": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as BattleTargetSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_LeaderSelector_Succeeds()
    {
        // Arrange
        var data = """{ "l": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as LeaderSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_MeSelector_Succeeds()
    {
        // Arrange
        var data = """{ "me": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as MeSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_NotSelector_Succeeds()
    {
        // Arrange
        var data = """{ "not": {"or": []} }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as NotSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_OrSelector_Succeeds()
    {
        // Arrange
        var data = """{ "or": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as OrSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_PartyOtherSelector_Succeeds()
    {
        // Arrange
        var data = """{ "px": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as PartyOtherSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_PartySelector_Succeeds()
    {
        // Arrange
        var data = """{ "p": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as PartySelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_PetSelector_Succeeds()
    {
        // Arrange
        var data = """{ "pet": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as PetSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_ScanSelector_Succeeds()
    {
        // Arrange
        var data = """{ "scan": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as ScanSelector;
        Assert.NotNull(selectorConcrete);
    }

    [Fact]
    public void Parsing_TargetSelector_Succeeds()
    {
        // Arrange
        var data = """{ "t": [] }""";

        // Act
        var selector = JsonSerializer.Deserialize<ISelector>(data, JsonSerializerOptions);

        // Assert
        var selectorConcrete = selector as TargetSelector;
        Assert.NotNull(selectorConcrete);
    }
}
