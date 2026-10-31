#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiscardedProvenanceEntries
{
    internal void AssertProvenanceClaim(ProgramSkillFrame f) => _resolve().AssertProvenanceClaim(f);
    internal SkillProgramStepOutcome BeginDiscardedProvenanceClaim(ProgramSkillFrame input, string tag) => _resolve().BeginDiscardedProvenanceClaim(input, tag);
    internal void CaptureDiscardedEntityOrigin(Card card, CardMovementRecord movement) => _resolve().CaptureDiscardedEntityOrigin(card, movement);
    internal void CaptureProvenanceJudgmentOrigin(JudgmentFrame judgment, Card card, Suit effectiveSuit) => _resolve().CaptureProvenanceJudgmentOrigin(judgment, card, effectiveSuit);
    internal ProgramDiscardedEntityOriginEvent? DiscardedProvenanceOrigin(CardMovementRecord movement) => _resolve().DiscardedProvenanceOrigin(movement);
    internal int GetOutsideProvenanceCount(int seat, string skill, string instance, string tag) => _resolve().GetOutsideProvenanceCount(seat, skill, instance, tag);
    internal bool IsProvenanceJudgmentResultHandoff(int cardId, ProgramJudgmentEntityOriginEvent judged, CardMovementRecord previous, CardLocation currentFrom) => _resolve().IsProvenanceJudgmentResultHandoff(cardId, judged, previous, currentFrom);
    internal int[] MatchingProvenanceDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().MatchingProvenanceDiscardIndexes(batch, candidate, trigger);
    internal SkillProgramStepOutcome OfferProvenanceFaceUp(ProgramSkillFrame input, string tag) => _resolve().OfferProvenanceFaceUp(input, tag);
    internal bool ProvenanceThresholdQualified(ProgramSkillFrame f, ProgramProvenanceClaimReceipt receipt) => _resolve().ProvenanceThresholdQualified(f, receipt);
    internal void RecordProvenanceFaceUpReset(int owner, bool wasFaceDown) => _resolve().RecordProvenanceFaceUpReset(owner, wasFaceDown);
    internal void ResolveProvenanceFaceUp(PromptChoice choice) => _resolve().ResolveProvenanceFaceUp(choice);
    internal bool ResumeProvenanceClaim(long id) => _resolve().ResumeProvenanceClaim(id);
    internal int[]? SelectWindowIndexes_MatchingProvenanceDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingProvenanceDiscardIndexes(batch, candidate, trigger);
    internal bool TracksDiscardedEntityProvenance { get => _resolve().TracksDiscardedEntityProvenance; }
}
