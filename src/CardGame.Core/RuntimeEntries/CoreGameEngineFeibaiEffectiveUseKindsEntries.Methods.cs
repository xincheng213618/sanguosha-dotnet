#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFeibaiEffectiveUseKindsEntries
{
    internal global::CardGame.Core.CardKind FeibaiEffectiveUseKind(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int end, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent current) => _resolve().FeibaiEffectiveUseKind(fact, history, end, current);
}
