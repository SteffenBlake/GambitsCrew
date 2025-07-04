using GambitsCrew.Domain.Deployments;

namespace GambitsCrew.LocalTesting.Deployments;

public interface IDeploymentBuilder
{
    Deployment Build();
}

