using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Deployments;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.LocalTesting.Deployments;

public class SoloPld(
    LocalTestingConfig config
) : IDeploymentBuilder
{
    public Deployment Build()
    {
        return new([
            new CrewMember(config.Player1, [
                new Gambit(
                    new ScanSelector([
                        new NameCondition([
                            new StringToLowerOperator(new StringContainsOperator("worm"))
                        ]),
                    ]),
                    new TargetCommand(new())
                )
            ])
        ]);
    }
}
