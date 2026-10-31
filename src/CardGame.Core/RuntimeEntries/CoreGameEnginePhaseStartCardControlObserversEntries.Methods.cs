#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseStartCardControlObserversEntries
{
    internal bool IsPhaseStartCardControlDying() => _resolve().IsPhaseStartCardControlDying();
    internal bool PhaseStartCardControlFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().PhaseStartCardControlFirstChild(root, child);
    internal ProgramSkillFrame? PhaseStartCardControlObserverRoot() => _resolve().PhaseStartCardControlObserverRoot();
    internal bool TryAdvancePhaseStartCardControlSubtree() => _resolve().TryAdvancePhaseStartCardControlSubtree();
}
