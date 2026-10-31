#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInspectedHandSlashObserversEntries
{
    internal bool HasInspectedHandDamageObserver(long window) => _resolve().HasInspectedHandDamageObserver(window);
    internal global::CardGame.Core.ProgramSkillFrame? InspectedHandPaidObserverRoot(long? damageWindow = default) => _resolve().InspectedHandPaidObserverRoot(damageWindow);
    internal bool IsInspectedHandMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsInspectedHandMovement(f, effect, pending);
    internal bool IsInspectedHandProgramDying() => _resolve().IsInspectedHandProgramDying();
}
