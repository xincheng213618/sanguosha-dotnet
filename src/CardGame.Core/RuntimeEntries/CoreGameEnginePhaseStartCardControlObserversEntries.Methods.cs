#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseStartCardControlObserversEntries
{
    internal bool IsPhaseStartCardControlDying() => _resolve().IsPhaseStartCardControlDying();
    internal bool PhaseStartCardControlFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().PhaseStartCardControlFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? PhaseStartCardControlObserverRoot() => _resolve().PhaseStartCardControlObserverRoot();
    internal bool TryAdvancePhaseStartCardControlSubtree() => _resolve().TryAdvancePhaseStartCardControlSubtree();
}
