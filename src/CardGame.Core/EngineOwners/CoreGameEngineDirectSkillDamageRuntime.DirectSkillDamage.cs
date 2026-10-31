#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDirectSkillDamageRuntime
{
    internal bool IsDirectSkillDamage(GameEngine.IDamageAttempt attack) => attack is NativeOwners.ProgramAttackHandle && !attack.IsSourceLess && !attack.IsChainPropagation && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == attack.ResolutionId && frame.OwnerSeat == attack.SourceSeat && frame.AttackAttempt is not null);
}
