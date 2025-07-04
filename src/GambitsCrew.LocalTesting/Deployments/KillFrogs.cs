using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Deployments;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.LocalTesting.Deployments;

public class KillFrogs(
    LocalTestingConfig config
) : IDeploymentBuilder
{
    public Deployment Build()
    {
        var player1 = new CrewMember(config.Player1, [
            // Find target if we dont have one
            new Gambit(
                new AndSelector([
                    new NotSelector(new TargetSelector([
                        new NameCondition([new StringEqualsOperator("Apex Toad")])
                    ])),
                    new NotSelector(new MeSelector([
                        new StatusCondition([new StatusHasFlagOperator(EliteMMO.API.EntityStatus.Engaged)])
                    ])),
                    new ScanSelector([
                        new NameCondition([new StringEqualsOperator("Apex Toad")]),
                        new ClaimedCondition(false),
                        new DistanceCondition([new NumberLessThanOperator(30)])
                    ])
                ]),
                new TargetCommand(new())
            ),
            // Attack if we have a valid target
            new Gambit(
                new AndSelector([
                    new NotSelector(new MeSelector([
                        new StatusCondition([new StatusHasFlagOperator(EliteMMO.API.EntityStatus.Engaged)])
                    ])),
                    new TargetSelector([
                        new NameCondition([new StringEqualsOperator("Apex Toad")]),
                        new ClaimedCondition(false),
                        new DistanceCondition([new NumberLessThanOperator(25)])
                    ])
                ]),
                new AttackCommand(new())
            ),
            // Engage if we are attacking but too far
            new Gambit(
                new AndSelector([
                    new MeSelector([
                        new StatusCondition([new StatusHasFlagOperator(EliteMMO.API.EntityStatus.Engaged)])
                    ]),
                    new TargetSelector([
                        new DistanceCondition([new NumberGreaterThanOperator(2)])
                    ]),
                ]),
                new FollowCommand(new(Min:1, Max: 2))
            ),
            // Face towards if we arent facing it
            new Gambit(
                new AndSelector([
                    new MeSelector([
                        new StatusCondition([new StatusHasFlagOperator(EliteMMO.API.EntityStatus.Engaged)])
                    ]),
                    new TargetSelector([
                        new FacingTowardsCondition(false)
                    ])
                ]),
                new FaceTowardsCommand(true)
            ),

            // Reapply Corsair's Roll
            new Gambit(
                new NotSelector(new MeSelector([
                    new BuffsCondition([
                        new BuffsContainsOperator(326)
                    ])
                ])),
                new AbilityCommand(new("Corsair's Roll"))
            ),

            // Reapply Samurai Roll
            new Gambit(
                new NotSelector(new MeSelector([
                    new BuffsCondition([
                        new BuffsContainsOperator(321)
                    ])
                ])),
                new AbilityCommand(new("Samurai Roll"))
            ),

            // Reapply Food
            new Gambit(
                new NotSelector(new MeSelector([
                    new BuffsCondition([
                        new BuffsContainsOperator(251)
                    ])
                ])),
                new ItemCommand(new("Cutlet Sandwich"))
            ),
        ]);

        return new([player1]);
    }
}
