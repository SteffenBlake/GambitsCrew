using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Deployments;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.LocalTesting.Deployments;

public class FollowLeader(
    LocalTestingConfig config    
) : IDeploymentBuilder
{
    public Deployment Build()
    {
        return new Deployment([
            new CrewMember(config.Player2, [
                new Gambit(
                    new AndSelector([
                        new PartyOtherSelector([
                            new NameCondition([
                                new StringEqualsOperator(config.Player1)
                            ]),
                            new DistanceCondition([
                                new NumberGreaterThanOperator(10)
                            ])
                        ])
                    ]),
                    new FollowCommand(new())
                )
            ])
        ]);
    }
}
