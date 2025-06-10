using GambitsCrew.Domain.Conditions;
using GambitsCrew.Domain.Gambits;

namespace GambitsCrew.Domain.Selectors;

public record PetSelector(
    List<ICondition> Pet
) : ISelector
{
    public bool Eval(GambitContext ctx, IEliteAPI api)
    {
        var pet = api.PetEntity;
        if (pet == null)
        {
            return false;
        }
        ctx.ContextualEntity = pet;
        ctx.ContextualTarget = "<pet>";

        return Pet.All(condition => condition.Eval(ctx));
    }
}
