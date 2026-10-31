#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessReturnsEntries
{
    internal bool FinishRoundOnceCardlessAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishRoundOnceCardlessAttack(attack, out paused);
    internal bool IsRoundOnceCardlessProgramRequestAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsRoundOnceCardlessProgramRequestAttackConsistent(attack, processing);
    internal void ObserveRoundOnceCardlessRequestedOwnerDamage(global::CardGame.Core.DamageAppliedEvent damage) => _resolve().ObserveRoundOnceCardlessRequestedOwnerDamage(damage);
    internal bool RoundOnceCardlessRequestedOwnerWasDamaged(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().RoundOnceCardlessRequestedOwnerWasDamaged(parent, completion);
    internal bool TryResolveRoundOnceCardlessAiDodge() => _resolve().TryResolveRoundOnceCardlessAiDodge();
}
