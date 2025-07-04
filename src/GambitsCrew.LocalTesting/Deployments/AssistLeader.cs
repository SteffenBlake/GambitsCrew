using EliteMMO.API;
using GambitsCrew.Domain.Commands;
using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;
using GambitsCrew.Domain.Deployments;
using GambitsCrew.Domain.Gambits;
using GambitsCrew.Domain.Operators;
using GambitsCrew.Domain.Selectors;

namespace GambitsCrew.LocalTesting.Deployments;

public class AssistLeader(
    LocalTestingConfig config
) : IDeploymentBuilder
{
    public Deployment Build()
    {
        var player1 = new CrewMember(config.Player1, [
            new Gambit(
                new AndSelector([
                    new MeSelector([
                        new StatusCondition([
                            new StatusHasFlagOperator(EntityStatus.Engaged)
                        ])
                    ]),
                    new TargetSelector([
                        new DistanceCondition([
                            new NumberGreaterThanOperator(2),
                            new NumberLessThanOperator(5),
                        ]),
                        new ClaimedCondition(true)
                    ])
                ]),
                new FollowCommand(new(Min: 1, Max:2, Wait: 1))
            ),

            new Gambit(
                new AndSelector([
                    new MeSelector([
                        new StatusCondition([
                            new StatusHasFlagOperator(EntityStatus.Engaged)
                        ])
                    ]),
                    new TargetSelector([
                        new DistanceCondition([
                            new NumberGreaterThanOperator(5),
                            new NumberLessThanOperator(21)
                        ]),
                        new ClaimedCondition(false)
                    ])
                ]),
                new CastCommand(new("Flash"))
            ),

            new Gambit(
                new AndSelector([
                    new MeSelector([
                        new TpCondition([
                            new NumberGreaterThanOrEqualsOperator(2000)
                        ]),
                        new StatusCondition([
                            new StatusHasFlagOperator(EntityStatus.Engaged)
                        ])
                    ]),
                    new TargetSelector([
                        new DistanceCondition([
                            new NumberLessThanOperator(5)
                        ]),
                        new HppCondition([
                            new NumberGreaterThanOperator(75)
                        ])
                    ])
                ]),
                new WeaponskillCommand(new("Knights of Round"))
            ),

            // Scan for Apex
            new Gambit(
                new AndSelector([
                    new NotSelector(
                        new MeSelector([
                            new StatusCondition([
                                new StatusHasFlagOperator(EntityStatus.Engaged)
                            ])
                        ])
                    ),
                    new ScanSelector([
                        new NameCondition([
                            new StringToLowerOperator(new StringContainsOperator("apex"))
                        ]),
                        new DistanceCondition([
                            new NumberLessThanOperator(21)
                        ]),
                        new ClaimedCondition(false)
                    ])
                ]),
                new AllCommand([
                    new TargetCommand(new(Wait: 1)),
                    new AttackCommand(new(Wait: 1)),
                    new CastCommand(new("Flash", Wait: 3))
                ])
            )
        ]);

        var player2 = new CrewMember(config.Player2, [
            new Gambit(
                new AndSelector([
                    new MeSelector([
                        new StatusCondition([
                            new StatusHasFlagOperator(EntityStatus.Engaged)
                        ])
                    ]),
                    new TargetSelector([
                        new DistanceCondition([
                            new NumberGreaterThanOperator(2),
                            new NumberLessThanOperator(5),
                        ]),
                        new ClaimedCondition(true)
                    ])
                ]),
                new FollowCommand(new(Min: 1, Max:2, Wait: 1))
            ),

            new Gambit(
                new AndSelector([
                    new PartySelector([
                        new NameCondition([
                            new StringEqualsOperator(config.Player1)
                        ]),
                        new TpCondition([
                            new NumberLessThanOperator(250)
                        ]),
                        new StatusCondition([
                            new StatusHasFlagOperator(EntityStatus.Engaged)
                        ])
                    ]),
                    new MeSelector([
                        new TpCondition([
                            new NumberGreaterThanOperator(1500)
                        ])
                    ]),
                    new TargetSelector([
                        new HppCondition([
                            new NumberGreaterThanOperator(10)
                        ])
                    ])
                ]),
                new WeaponskillCommand(new("Savage Blade", Wait: 1))
            ),
            new Gambit(
                new AndSelector([
                    new NotSelector(new MeSelector([
                        new StatusCondition([
                            new StatusHasFlagOperator(EntityStatus.Engaged)
                        ])
                    ])),
                    new PartySelector([
                        new NameCondition([
                            new StringEqualsOperator(config.Player1)
                        ]),
                        new StatusCondition([
                            new StatusHasFlagOperator(EntityStatus.Engaged)
                        ])
                    ])
                ]),
                new AllCommand([
                    new AssistCommand(new(Wait: 1)),
                    new AttackCommand(new(Wait: 1))
                ])
            )
        ]);

        return new([ player1, player2 ]);
    }
}
