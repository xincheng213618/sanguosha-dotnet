#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardMovementProgramsEntries
{
    internal void AdvanceCardsMovedProgramCandidate(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, bool activated, bool completed) => _resolve().AdvanceCardsMovedProgramCandidate(frame, candidate, activated, completed);
    internal void AdvanceCardsMovedProgramCursor(global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => _resolve().AdvanceCardsMovedProgramCursor(frame);
    internal global::CardGame.Core.CardMovementBatchBuilder BeginCardMovementBatch(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> sources, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> destinations) => _resolve().BeginCardMovementBatch(sources, destinations);
    internal global::CardGame.Core.SkillProgramTriggerFacts CaptureCardsMovedTriggerFacts(global::CardGame.Core.CharacterState owner, int movedCardCount, global::CardGame.Core.CardMovementSourceCount sourceCount, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.CardMovementTiming? timing = default, global::CardGame.Core.CardMovementBatchContext? nativeDrawBatch = default) => _resolve().CaptureCardsMovedTriggerFacts(owner, movedCardCount, sourceCount, window, timing, nativeDrawBatch);
    internal void CaptureMovementBatchStartMovementTiming(global::CardGame.Core.CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartMovementTiming(batch);
    internal global::CardGame.Core.CardMovementTiming? CaptureMovementTiming() => _resolve().CaptureMovementTiming();
    internal void ClaimProgramMovedCards(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ClaimProgramMovedCards(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectCardsMovedProgramCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectCardsMovedProgramCandidates(batch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectDiscardPileReceivedCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectDiscardPileReceivedCandidates(batch);
    internal void CompleteCardMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements, bool committed) => _resolve().CompleteCardMovementBatch(batch, movements, committed);
    internal void CompleteCardsMovedProgramWindow(global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => _resolve().CompleteCardsMovedProgramWindow(frame);
    internal void ContinueCardsMovedProgramWindowCore() => _resolve().ContinueCardsMovedProgramWindowCore();
    internal global::CardGame.Core.ProgramSkillWindowContext CreateCardsMovedProgramContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateCardsMovedProgramContext(frame, candidate);
    internal int[] MatchingDiscardPileIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().MatchingDiscardPileIndexes(batch, candidate, trigger);
    internal int[] MatchingMovementIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardLocation location) => _resolve().MatchingMovementIndexes(batch, candidate, trigger, location);
    internal int[] MatchingOwnerBatchMovementIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().MatchingOwnerBatchMovementIndexes(batch, candidate, trigger);
    internal bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => _resolve().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}
