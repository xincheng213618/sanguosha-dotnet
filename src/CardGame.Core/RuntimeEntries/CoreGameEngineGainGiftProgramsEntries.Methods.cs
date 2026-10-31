#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGainGiftProgramsEntries
{
    internal bool CanRunGainGiftTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunGainGiftTrigger(candidate, trigger, context);
    internal RedOwnedLossMaterial? CaptureRedOwnedLoss(Card card, CardLocation from, CardLocation to, long sequence) => _resolve().CaptureRedOwnedLoss(card, from, to, sequence);
    internal bool ExactGainGiftMovementParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, scoped out CardsMovedTriggerWindowFrame parent) => _resolve().ExactGainGiftMovementParent(candidate, context, out parent);
    internal SkillProgramStepOutcome ExecuteGainGift(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ExecuteGainGift(supplied, effect);
    internal void FinishGainGift(ProgramSkillFrame f) => _resolve().FinishGainGift(f);
    internal IReadOnlyList<PromptChoice> GainGiftChoices(ProgramSkillFrame f) => _resolve().GainGiftChoices(f);
    internal bool GainGiftHasFinalGameEnd() => _resolve().GainGiftHasFinalGameEnd();
    internal CardLocation GainGiftOriginalSource(CardMovementRecord first) => _resolve().GainGiftOriginalSource(first);
    internal SkillProgramEffect? GainGiftPausedEffect(ProgramSkillFrame f) => _resolve().GainGiftPausedEffect(f);
    internal bool GainGiftPhaseUsed(int owner, string skill, ActualDiscardRecoveryPhaseKey phase) => _resolve().GainGiftPhaseUsed(owner, skill, phase);
    internal IEnumerable<CardSnapshot> GainGiftPublicLostCards() => _resolve().GainGiftPublicLostCards();
    internal bool IsGainGiftMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsGainGiftMovement(f, effect, pending);
    internal int[] MatchingActualGainGiftIndexes(CardMovementBatchContext batch, int owner, CardLocation location) => _resolve().MatchingActualGainGiftIndexes(batch, owner, location);
    internal int[] MatchingRedOwnerLossIndexes(CardMovementBatchContext batch, int owner, CardLocation location) => _resolve().MatchingRedOwnerLossIndexes(batch, owner, location);
    internal SkillProgramStepOutcome PayDelegatedJudgment(ProgramSkillFrame f, SkillProgramEffect effect) => _resolve().PayDelegatedJudgment(f, effect);
    internal void PublishGainGiftChoice(ProgramSkillFrame f) => _resolve().PublishGainGiftChoice(f);
    internal void RecordDelegatedJudgmentPayment(JudgmentFrame j, long batchId) => _resolve().RecordDelegatedJudgmentPayment(j, batchId);
    internal void ResolveGainGiftChoice(PromptChoice choice) => _resolve().ResolveGainGiftChoice(choice);
    internal bool ResumeGainGift(long id) => _resolve().ResumeGainGift(id);
    internal bool ReturnGainGiftMovement(ProgramSkillFrame f) => _resolve().ReturnGainGiftMovement(f);
    internal PromptChoice SelectAiGainGift(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiGainGift(decision, f);
    internal int[]? SelectWindowIndexes_MatchingActualGainGiftIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location) => _resolve().SelectWindowIndexes_MatchingActualGainGiftIndexes(batch, candidate, trigger, location);
    internal int[]? SelectWindowIndexes_MatchingRedOwnerLossIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location) => _resolve().SelectWindowIndexes_MatchingRedOwnerLossIndexes(batch, candidate, trigger, location);
    internal void StartNextRedLossDraw(ProgramSkillFrame f) => _resolve().StartNextRedLossDraw(f);
    internal Card[] TransferableGainGiftCards(int owner) => _resolve().TransferableGainGiftCards(owner);
    internal bool TracksRedOwnedLoss { get => _resolve().TracksRedOwnedLoss; }
}
