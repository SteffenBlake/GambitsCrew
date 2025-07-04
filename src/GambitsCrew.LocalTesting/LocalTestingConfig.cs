using GambitsCrew.LocalTesting.Deployments;

namespace GambitsCrew.LocalTesting;

public record LocalTestingConfig(
    DeploymentChoice DeploymentChoice,
    string Player1,
    string Player2
);
