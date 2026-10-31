#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessReturnsEntries
{
    internal bool FinishRoundOnceCardlessAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishRoundOnceCardlessAttack(attack, out paused);
    internal void ObserveRoundOnceCardlessRequestedOwnerDamage(DamageAppliedEvent damage) => _resolve().ObserveRoundOnceCardlessRequestedOwnerDamage(damage);
    internal bool RoundOnceCardlessRequestedOwnerWasDamaged(ProgramSkillFrame parent, GameEngine.AttackCompletionReceipt completion) => _resolve().RoundOnceCardlessRequestedOwnerWasDamaged(parent, completion);
    internal bool TryResolveRoundOnceCardlessAiDodge() => _resolve().TryResolveRoundOnceCardlessAiDodge();
}
