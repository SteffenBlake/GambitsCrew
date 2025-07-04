using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Deployments;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.LocalTesting.Deployments;

public class ReapplySpectralJig(
    LocalTestingConfig config
) : IDeploymentBuilder
{
    public Deployment Build()
    {
        var player1 = new CrewMember(config.Player1, [
            new Gambit(
                new NotSelector(new MeSelector([
                    new BuffsCondition([
                        new BuffsContainsOperator(71)
                    ])
                ])),
                new AbilityCommand(new("Spectral Jig"))
            )
        ]);

        return new([player1]);
    }
}
