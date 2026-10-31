#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseNamePredictionObserversEntries
{
    internal bool PhaseNamePredictionFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().PhaseNamePredictionFirstChild(root, child);
    internal ProgramSkillFrame? PhaseNamePredictionObserverRoot() => _resolve().PhaseNamePredictionObserverRoot();
    internal bool TryAdvancePhaseNamePredictionSubtree() => _resolve().TryAdvancePhaseNamePredictionSubtree();
}
