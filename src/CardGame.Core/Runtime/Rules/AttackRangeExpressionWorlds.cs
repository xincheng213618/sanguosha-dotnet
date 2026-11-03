using CardGame.Core.EngineOwners;

namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class AttackRangeExpressionWorlds(IKernelCreateProgramExpressionWorldOperations worlds)
    : IAttackRangeExpressionWorlds
{
    public IProgramExpressionWorld Create(int sourceOwner, int actor, int target) =>
        worlds.CreateAttackRangeExpressionWorld(sourceOwner, actor, target);
}
