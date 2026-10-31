#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseHandSeizureObserversEntries
{
    internal bool HasPhaseHandSeizureDamageObserver(long windowId) => _resolve().HasPhaseHandSeizureDamageObserver(windowId);
    internal bool IsPhaseHandSeizureProgramDying() => _resolve().IsPhaseHandSeizureProgramDying();
    internal bool PhaseHandSeizureFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().PhaseHandSeizureFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? PhaseHandSeizureObserverRoot() => _resolve().PhaseHandSeizureObserverRoot();
}
