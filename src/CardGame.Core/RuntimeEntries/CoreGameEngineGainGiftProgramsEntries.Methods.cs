#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGainGiftProgramsEntries
{
    internal bool CanRunGainGiftTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunGainGiftTrigger(candidate, trigger, context);
    internal global::CardGame.Core.RedOwnedLossMaterial? CaptureRedOwnedLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, long sequence) => _resolve().CaptureRedOwnedLoss(card, from, to, sequence);
    internal bool ExactGainGiftMovementParent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardsMovedTriggerWindowFrame parent) => _resolve().ExactGainGiftMovementParent(candidate, context, out parent);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteGainGift(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteGainGift(supplied, effect);
    internal void FinishGainGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishGainGift(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GainGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GainGiftChoices(f);
    internal bool GainGiftHasFinalGameEnd() => _resolve().GainGiftHasFinalGameEnd();
    internal global::CardGame.Core.CardLocation GainGiftOriginalSource(global::CardGame.Core.CardMovementRecord first) => _resolve().GainGiftOriginalSource(first);
    internal global::CardGame.Core.SkillProgramEffect? GainGiftPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GainGiftPausedEffect(f);
    internal bool GainGiftPhaseUsed(int owner, string skill, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().GainGiftPhaseUsed(owner, skill, phase);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> GainGiftPublicLostCards() => _resolve().GainGiftPublicLostCards();
    internal bool IsGainGiftMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsGainGiftMovement(f, effect, pending);
    internal int[] MatchingActualGainGiftIndexes(global::CardGame.Core.CardMovementBatchContext batch, int owner, global::CardGame.Core.CardLocation location) => _resolve().MatchingActualGainGiftIndexes(batch, owner, location);
    internal int[] MatchingRedOwnerLossIndexes(global::CardGame.Core.CardMovementBatchContext batch, int owner, global::CardGame.Core.CardLocation location) => _resolve().MatchingRedOwnerLossIndexes(batch, owner, location);
    internal global::CardGame.Core.SkillProgramStepOutcome PayDelegatedJudgment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect) => _resolve().PayDelegatedJudgment(f, effect);
    internal void PublishGainGiftChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishGainGiftChoice(f);
    internal void RecordDelegatedJudgmentPayment(global::CardGame.Core.JudgmentFrame j, long batchId) => _resolve().RecordDelegatedJudgmentPayment(j, batchId);
    internal void ResolveGainGiftChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveGainGiftChoice(choice);
    internal bool ResumeGainGift(long id) => _resolve().ResumeGainGift(id);
    internal bool ReturnGainGiftMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnGainGiftMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiGainGift(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiGainGift(decision, f);
    internal int[]? SelectWindowIndexes_MatchingActualGainGiftIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardLocation location) => _resolve().SelectWindowIndexes_MatchingActualGainGiftIndexes(batch, candidate, trigger, location);
    internal int[]? SelectWindowIndexes_MatchingRedOwnerLossIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardLocation location) => _resolve().SelectWindowIndexes_MatchingRedOwnerLossIndexes(batch, candidate, trigger, location);
    internal void StartNextRedLossDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StartNextRedLossDraw(f);
    internal global::CardGame.Core.Card[] TransferableGainGiftCards(int owner) => _resolve().TransferableGainGiftCards(owner);
    internal bool TracksRedOwnedLoss { get => _resolve().TracksRedOwnedLoss; }
}
