#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFeibaiEffectiveUseKindsEntries
{
    internal CardKind FeibaiEffectiveUseKind(CurrentTurnCardUseKindsRecordedEvent fact, IReadOnlyList<IGameEvent> history, int end, CurrentTurnCardUseKindsRecordedEvent current) => _resolve().FeibaiEffectiveUseKind(fact, history, end, current);
}
