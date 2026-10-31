#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeSlashTargetDuelEntries
{
    internal void AssertNativeSlashTargetDuelPlans() => _resolve().AssertNativeSlashTargetDuelPlans();
    internal void FreezeNativeSlashTargetDuelPlan(global::CardGame.Core.CardUseFrame use) => _resolve().FreezeNativeSlashTargetDuelPlan(use);
    internal bool IsNativeSlashTargetDuelAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().IsNativeSlashTargetDuelAttack(attack);
    internal bool IsNativeSlashTargetDuelZeroUse(long useId) => _resolve().IsNativeSlashTargetDuelZeroUse(useId);
    internal global::CardGame.Core.CardActionContext NativeSlashTargetDuelEffectAction(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NativeSlashTargetDuelSlice slice) => _resolve().NativeSlashTargetDuelEffectAction(use, slice);
    internal global::CardGame.Core.Card NativeSlashTargetDuelRepresentation(global::CardGame.Core.CardUseFrame use) => _resolve().NativeSlashTargetDuelRepresentation(use);
    internal bool TryBeginNativeSlashTargetDuel(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginNativeSlashTargetDuel(attack);
    internal bool TryCompleteCanceledNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().TryCompleteCanceledNativeSlashTargetDuel(pending);
    internal bool TryGetNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.NativeSlashTargetDuelPlan plan, scoped out global::CardGame.Core.NativeSlashTargetDuelSlice slice) => _resolve().TryGetNativeSlashTargetDuel(pending, out use, out plan, out slice);
    internal bool TryResolveNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().TryResolveNativeSlashTargetDuel(pending);
    internal bool ValidNativeSlashTargetDuelPlan(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NativeSlashTargetDuelPlan plan) => _resolve().ValidNativeSlashTargetDuelPlan(use, plan);
}
