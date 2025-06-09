using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.CrewMembers;

namespace GambitsCrew.Domain.Selectors;

public record PetSelector(
    List<ICondition> Pet
) : ISelector
{
    public bool Eval(CrewContext ctx)
    {
        if (ctx.Pet == null)
        {
            return false;
        }
        ctx.ContextualEntity = ctx.Pet;
        ctx.ContextualTarget = "<pet>";

        return Pet.All(condition => condition.Eval(ctx));
    }
}
