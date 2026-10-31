#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetObtainObserverEntries
{
    internal bool HasPaidDamageTargetObtainObserver(long id) => _resolve().HasPaidDamageTargetObtainObserver(id);
    internal global::CardGame.Core.ProgramSkillFrame? PaidDamageTargetObtainObserverRoot(long windowId) => _resolve().PaidDamageTargetObtainObserverRoot(windowId);
}
