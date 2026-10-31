#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSharedGameEngineOlMoRumoObserversEntries
{
    internal bool AdvanceOlMoRumoChildren() => _resolve().AdvanceOlMoRumoChildren();
    internal bool IsOlMoRumoRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().IsOlMoRumoRecoveryParent(frame, recovery);
    internal bool OlMoRumoFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().OlMoRumoFirstChild(frame, child);
    internal bool OlMoRumoLifecycleEdge(int index) => _resolve().OlMoRumoLifecycleEdge(index);
    internal ProgramSkillFrame? OlMoRumoObserverRoot() => _resolve().OlMoRumoObserverRoot();
    internal bool OlMoRumoObserverSuffix(int index) => _resolve().OlMoRumoObserverSuffix(index);
}
