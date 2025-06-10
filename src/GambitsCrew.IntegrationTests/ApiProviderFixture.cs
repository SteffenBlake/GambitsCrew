using GambitsCrew.IntegrationTests.Mocks;

namespace GambitsCrew.IntegrationTests;

public class ApiProviderFixture 
{
    public MockEliteApi Api { get; } = new();
}
