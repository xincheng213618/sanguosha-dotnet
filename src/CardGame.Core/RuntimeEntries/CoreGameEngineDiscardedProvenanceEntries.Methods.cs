#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiscardedProvenanceEntries
{
    internal void AssertProvenanceClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertProvenanceClaim(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDiscardedProvenanceClaim(global::CardGame.Core.ProgramSkillFrame input, string tag) => _resolve().BeginDiscardedProvenanceClaim(input, tag);
    internal void CaptureDiscardedEntityOrigin(global::CardGame.Core.Card card, global::CardGame.Core.CardMovementRecord movement) => _resolve().CaptureDiscardedEntityOrigin(card, movement);
    internal void CaptureProvenanceJudgmentOrigin(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, global::CardGame.Core.Suit effectiveSuit) => _resolve().CaptureProvenanceJudgmentOrigin(judgment, card, effectiveSuit);
    internal global::CardGame.Core.ProgramDiscardedEntityOriginEvent? DiscardedProvenanceOrigin(global::CardGame.Core.CardMovementRecord movement) => _resolve().DiscardedProvenanceOrigin(movement);
    internal int GetOutsideProvenanceCount(int seat, string skill, string instance, string tag) => _resolve().GetOutsideProvenanceCount(seat, skill, instance, tag);
    internal bool IsProvenanceJudgmentResultHandoff(int cardId, global::CardGame.Core.ProgramJudgmentEntityOriginEvent judged, global::CardGame.Core.CardMovementRecord previous, global::CardGame.Core.CardLocation currentFrom) => _resolve().IsProvenanceJudgmentResultHandoff(cardId, judged, previous, currentFrom);
    internal int[] MatchingProvenanceDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().MatchingProvenanceDiscardIndexes(batch, candidate, trigger);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferProvenanceFaceUp(global::CardGame.Core.ProgramSkillFrame input, string tag) => _resolve().OfferProvenanceFaceUp(input, tag);
    internal bool ProvenanceThresholdQualified(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramProvenanceClaimReceipt receipt) => _resolve().ProvenanceThresholdQualified(f, receipt);
    internal void RecordProvenanceFaceUpReset(int owner, bool wasFaceDown) => _resolve().RecordProvenanceFaceUpReset(owner, wasFaceDown);
    internal void ResolveProvenanceFaceUp(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveProvenanceFaceUp(choice);
    internal bool ResumeProvenanceClaim(long id) => _resolve().ResumeProvenanceClaim(id);
    internal int[]? SelectWindowIndexes_MatchingProvenanceDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingProvenanceDiscardIndexes(batch, candidate, trigger);
    internal bool TracksDiscardedEntityProvenance { get => _resolve().TracksDiscardedEntityProvenance; }
}
