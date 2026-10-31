#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdjacentDiscardStorageEntries
{
    internal global::CardGame.Core.ProgramAdjacentDiscardOriginEvent? AdjacentDiscardOrigin(global::CardGame.Core.CardMovementRecord move) => _resolve().AdjacentDiscardOrigin(move);
    internal void AssertAdjacentDiscardStorage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertAdjacentDiscardStorage(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAdjacentDiscardStorage(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.CardZoneKind zone) => _resolve().BeginAdjacentDiscardStorage(input, zone);
    internal void CaptureAdjacentDiscardOrigin(global::CardGame.Core.Card card, global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardKind? frozenIdentity) => _resolve().CaptureAdjacentDiscardOrigin(card, move, frozenIdentity);
    internal global::CardGame.Core.CardKind? FreezeAdjacentDiscardPaymentIdentity(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => _resolve().FreezeAdjacentDiscardPaymentIdentity(card, from, to, reason);
    internal int[] MatchingAdjacentDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingAdjacentDiscardIndexes(batch, candidate);
    internal bool ResumeAdjacentDiscardStorage(long id) => _resolve().ResumeAdjacentDiscardStorage(id);
    internal int[]? SelectWindowIndexes_MatchingAdjacentDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingAdjacentDiscardIndexes(batch, candidate, trigger);
    internal bool TracksAdjacentDiscardStorage { get => _resolve().TracksAdjacentDiscardStorage; }
}
