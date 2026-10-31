#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardMovementProgramsEntries
{
    internal void AdvanceCardsMovedProgramCandidate(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _resolve().AdvanceCardsMovedProgramCandidate(frame, candidate, activated, completed);
    internal void AdvanceCardsMovedProgramCursor(CardsMovedTriggerWindowFrame frame) => _resolve().AdvanceCardsMovedProgramCursor(frame);
    internal CardMovementBatchBuilder BeginCardMovementBatch(IEnumerable<CardLocation> sources, IEnumerable<CardLocation> destinations) => _resolve().BeginCardMovementBatch(sources, destinations);
    internal SkillProgramTriggerFacts CaptureCardsMovedTriggerFacts(CharacterState owner, int movedCardCount, CardMovementSourceCount sourceCount, SkillProgramTriggerWindow window, CardMovementTiming? timing = default, CardMovementBatchContext? nativeDrawBatch = default) => _resolve().CaptureCardsMovedTriggerFacts(owner, movedCardCount, sourceCount, window, timing, nativeDrawBatch);
    internal void CaptureMovementBatchStartMovementTiming(CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartMovementTiming(batch);
    internal CardMovementTiming? CaptureMovementTiming() => _resolve().CaptureMovementTiming();
    internal void ClaimProgramMovedCards(ProgramSkillFrame frame) => _resolve().ClaimProgramMovedCards(frame);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectCardsMovedProgramCandidates(CardMovementBatchContext batch) => _resolve().CollectCardsMovedProgramCandidates(batch);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectDiscardPileReceivedCandidates(CardMovementBatchContext batch) => _resolve().CollectDiscardPileReceivedCandidates(batch);
    internal void CompleteCardMovementBatch(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements, bool committed) => _resolve().CompleteCardMovementBatch(batch, movements, committed);
    internal void CompleteCardsMovedProgramWindow(CardsMovedTriggerWindowFrame frame) => _resolve().CompleteCardsMovedProgramWindow(frame);
    internal void ContinueCardsMovedProgramWindowCore() => _resolve().ContinueCardsMovedProgramWindowCore();
    internal ProgramSkillWindowContext CreateCardsMovedProgramContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateCardsMovedProgramContext(frame, candidate);
    internal int[] MatchingDiscardPileIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().MatchingDiscardPileIndexes(batch, candidate, trigger);
    internal int[] MatchingMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location) => _resolve().MatchingMovementIndexes(batch, candidate, trigger, location);
    internal int[] MatchingOwnerBatchMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().MatchingOwnerBatchMovementIndexes(batch, candidate, trigger);
    internal bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => _resolve().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}
