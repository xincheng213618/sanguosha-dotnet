#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeJudgmentEntries
{
    internal void AdvanceJudgmentCandidate(JudgmentFrame pending) => _resolve().AdvanceJudgmentCandidate(pending);
    internal bool? BeginJudgment(NativeOwners.CardAttackHandle? attack, int targetSeat, string reason, long parentFrameId, CardKind? sourceCard, JudgmentContinuationKind continuation, Card? delayedCard = default, int? sourceSeat = default, string? programResultBind = default, SkillProgramCardSetVisibility? programResultVisibility = default, OriginalUseLightningAttachment? originalUseLightning = default, int? originalUseLightningExcludedCardId = default) => _resolve().BeginJudgment(attack, targetSeat, reason, parentFrameId, sourceCard, continuation, delayedCard, sourceSeat, programResultBind, programResultVisibility, originalUseLightning, originalUseLightningExcludedCardId);
    internal void BeginJudgmentReplacementChoice(JudgmentFrame pending) => _resolve().BeginJudgmentReplacementChoice(pending);
    internal IReadOnlyList<JudgmentTriggerCandidate> CollectJudgmentReplacementCandidates(Card judgmentCard, int targetSeat, string reason) => _resolve().CollectJudgmentReplacementCandidates(judgmentCard, targetSeat, reason);
    internal void CompleteBaguaResponse(NativeOwners.CardAttackHandle attack, bool succeeded) => _resolve().CompleteBaguaResponse(attack, succeeded);
    internal void CompleteDelayedJudgment(JudgmentFrame pending, bool succeeded) => _resolve().CompleteDelayedJudgment(pending, succeeded);
    internal bool? CompleteFinalizedJudgment(JudgmentFrame pending, Card judgmentCard, bool succeeded) => _resolve().CompleteFinalizedJudgment(pending, judgmentCard, succeeded);
    internal void CompleteLightningJudgment(JudgmentFrame pending, bool hit) => _resolve().CompleteLightningJudgment(pending, hit);
    internal void CompleteLightningWithoutHit(long resolutionId, Card lightning, CharacterState target, int? judgmentCardId) => _resolve().CompleteLightningWithoutHit(resolutionId, lightning, target, judgmentCardId);
    internal JudgmentTriggerCandidate? CurrentJudgmentCandidate(JudgmentFrame pending) => _resolve().CurrentJudgmentCandidate(pending);
    internal bool? FinalizeJudgment(JudgmentFrame pending) => _resolve().FinalizeJudgment(pending);
    internal void FinishProgramSkillJudgment(JudgmentFrame pending, Card judgmentCard) => _resolve().FinishProgramSkillJudgment(pending, judgmentCard);
    internal void FinishResolvedJudgment(JudgmentFrame pending) => _resolve().FinishResolvedJudgment(pending);
    internal JudgmentFrame GetJudgmentFrame(long frameId) => _resolve().GetJudgmentFrame(frameId);
    internal bool IsProtectedJudgmentSourceEquipment(CharacterState owner, Card card, int judgmentSubjectSeat, string judgmentReason) => _resolve().IsProtectedJudgmentSourceEquipment(owner, card, judgmentSubjectSeat, judgmentReason);
    internal void ReplaceJudgmentFrame(JudgmentFrame frame) => _resolve().ReplaceJudgmentFrame(frame);
    internal bool? ResolveBaguaJudgment(NativeOwners.CardAttackHandle attack, CharacterState defender) => _resolve().ResolveBaguaJudgment(attack, defender);
    internal void ResumeAfterLightningDamage(int delayedSeat, int? excludedCardId = default) => _resolve().ResumeAfterLightningDamage(delayedSeat, excludedCardId);
    internal void ResumeCompletedJudgment(GameEngine.JudgmentCompletionReceipt receipt) => _resolve().ResumeCompletedJudgment(receipt);
    internal void SetJudgmentFrameStep(long frameId, ResolutionFrameStep step) => _resolve().SetJudgmentFrameStep(frameId, step);
}
