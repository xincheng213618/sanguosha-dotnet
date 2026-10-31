#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDiscardSuitHistoryEntries
{
    internal void CaptureMovementBatchFactsCaptureTurnDiscardSuitFact(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureTurnDiscardSuitFact(batch, movements);
    internal void CaptureTurnDiscardSuitFact(int turn, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureTurnDiscardSuitFact(turn, movements);
    internal int? ProgramTriggerUsageLimit(global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ProgramTriggerUsageLimit(trigger);
    internal int CurrentTurnDiscardSuitMask { get => _resolve().CurrentTurnDiscardSuitMask; }
    internal bool TracksTurnDiscardSuits { get => _resolve().TracksTurnDiscardSuits; }
}
