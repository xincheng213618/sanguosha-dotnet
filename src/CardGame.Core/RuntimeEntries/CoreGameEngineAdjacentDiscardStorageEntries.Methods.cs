#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdjacentDiscardStorageEntries
{
    internal ProgramAdjacentDiscardOriginEvent? AdjacentDiscardOrigin(CardMovementRecord move) => _resolve().AdjacentDiscardOrigin(move);
    internal void AssertAdjacentDiscardStorage(ProgramSkillFrame f) => _resolve().AssertAdjacentDiscardStorage(f);
    internal SkillProgramStepOutcome BeginAdjacentDiscardStorage(ProgramSkillFrame input, CardZoneKind zone) => _resolve().BeginAdjacentDiscardStorage(input, zone);
    internal void CaptureAdjacentDiscardOrigin(Card card, CardMovementRecord move, CardKind? frozenIdentity) => _resolve().CaptureAdjacentDiscardOrigin(card, move, frozenIdentity);
    internal CardKind? FreezeAdjacentDiscardPaymentIdentity(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _resolve().FreezeAdjacentDiscardPaymentIdentity(card, from, to, reason);
    internal int[] MatchingAdjacentDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingAdjacentDiscardIndexes(batch, candidate);
    internal bool ResumeAdjacentDiscardStorage(long id) => _resolve().ResumeAdjacentDiscardStorage(id);
    internal int[]? SelectWindowIndexes_MatchingAdjacentDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingAdjacentDiscardIndexes(batch, candidate, trigger);
    internal bool TracksAdjacentDiscardStorage { get => _resolve().TracksAdjacentDiscardStorage; }
}
