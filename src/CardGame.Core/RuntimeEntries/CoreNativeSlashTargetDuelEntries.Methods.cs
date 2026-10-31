#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeSlashTargetDuelEntries
{
    internal void AssertNativeSlashTargetDuelPlans() => _resolve().AssertNativeSlashTargetDuelPlans();
    internal void FreezeNativeSlashTargetDuelPlan(CardUseFrame use) => _resolve().FreezeNativeSlashTargetDuelPlan(use);
    internal bool IsNativeSlashTargetDuelAttack(NativeOwners.CardAttackHandle attack) => _resolve().IsNativeSlashTargetDuelAttack(attack);
    internal bool IsNativeSlashTargetDuelZeroUse(long useId) => _resolve().IsNativeSlashTargetDuelZeroUse(useId);
    internal CardActionContext NativeSlashTargetDuelEffectAction(CardUseFrame use, NativeSlashTargetDuelSlice slice) => _resolve().NativeSlashTargetDuelEffectAction(use, slice);
    internal Card NativeSlashTargetDuelRepresentation(CardUseFrame use) => _resolve().NativeSlashTargetDuelRepresentation(use);
    internal bool TryBeginNativeSlashTargetDuel(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginNativeSlashTargetDuel(attack);
    internal bool TryCompleteCanceledNativeSlashTargetDuel(NullificationWindowFrame pending) => _resolve().TryCompleteCanceledNativeSlashTargetDuel(pending);
    internal bool TryGetNativeSlashTargetDuel(NullificationWindowFrame pending, scoped out CardUseFrame use, scoped out NativeSlashTargetDuelPlan plan, scoped out NativeSlashTargetDuelSlice slice) => _resolve().TryGetNativeSlashTargetDuel(pending, out use, out plan, out slice);
    internal bool TryResolveNativeSlashTargetDuel(NullificationWindowFrame pending) => _resolve().TryResolveNativeSlashTargetDuel(pending);
    internal bool ValidNativeSlashTargetDuelPlan(CardUseFrame use, NativeSlashTargetDuelPlan plan) => _resolve().ValidNativeSlashTargetDuelPlan(use, plan);
}
