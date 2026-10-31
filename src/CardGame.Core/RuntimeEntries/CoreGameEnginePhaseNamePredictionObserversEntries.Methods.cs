#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseNamePredictionObserversEntries
{
    internal bool HasPhaseNamePredictionDamageObserver(long window) => _resolve().HasPhaseNamePredictionDamageObserver(window);
    internal bool PhaseNamePredictionFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().PhaseNamePredictionFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? PhaseNamePredictionObserverRoot() => _resolve().PhaseNamePredictionObserverRoot();
    internal bool TryAdvancePhaseNamePredictionSubtree() => _resolve().TryAdvancePhaseNamePredictionSubtree();
}
