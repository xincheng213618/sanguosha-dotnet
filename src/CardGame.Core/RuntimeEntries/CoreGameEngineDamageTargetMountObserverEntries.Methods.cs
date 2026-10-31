#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetMountObserverEntries
{
    internal bool HasPaidDamageTargetMountObserver(long damageWindowId) => _resolve().HasPaidDamageTargetMountObserver(damageWindowId);
    internal bool IsPaidDamageTargetMountDying() => _resolve().IsPaidDamageTargetMountDying();
    internal global::CardGame.Core.ProgramSkillFrame? PaidDamageTargetMountObserverRoot(long damageWindowId) => _resolve().PaidDamageTargetMountObserverRoot(damageWindowId);
}
