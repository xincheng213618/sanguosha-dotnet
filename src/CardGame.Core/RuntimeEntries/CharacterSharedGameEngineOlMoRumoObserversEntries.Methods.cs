#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSharedGameEngineOlMoRumoObserversEntries
{
    internal bool AdvanceOlMoRumoChildren() => _resolve().AdvanceOlMoRumoChildren();
    internal bool HasOlMoRumoBeforeDamageObserver(long frameId) => _resolve().HasOlMoRumoBeforeDamageObserver(frameId);
    internal bool HasOlMoRumoDamageObserver(long frameId) => _resolve().HasOlMoRumoDamageObserver(frameId);
    internal bool IsOlMoRumoDying() => _resolve().IsOlMoRumoDying();
    internal bool IsOlMoRumoRecoveryParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsOlMoRumoRecoveryParent(frame, recovery);
    internal bool OlMoRumoFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().OlMoRumoFirstChild(frame, child);
    internal bool OlMoRumoLifecycleEdge(int index) => _resolve().OlMoRumoLifecycleEdge(index);
    internal global::CardGame.Core.ProgramSkillFrame? OlMoRumoObserverRoot() => _resolve().OlMoRumoObserverRoot();
    internal bool OlMoRumoObserverSuffix(int index) => _resolve().OlMoRumoObserverSuffix(index);
}
