#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDiscardSuitHistoryEntries
{
    internal void CaptureMovementBatchFactsCaptureTurnDiscardSuitFact(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureTurnDiscardSuitFact(batch, movements);
    internal void CaptureTurnDiscardSuitFact(int turn, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureTurnDiscardSuitFact(turn, movements);
    internal int? ProgramTriggerUsageLimit(SkillProgramTrigger trigger) => _resolve().ProgramTriggerUsageLimit(trigger);
    internal int CurrentTurnDiscardSuitMask { get => _resolve().CurrentTurnDiscardSuitMask; }
    internal bool TracksTurnDiscardSuits { get => _resolve().TracksTurnDiscardSuits; }
}
