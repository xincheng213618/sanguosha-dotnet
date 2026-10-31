#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeJudgmentEntries
{
    internal void AdvanceJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending) => _resolve().AdvanceJudgmentCandidate(pending);
    internal bool? BeginJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, int targetSeat, string reason, long parentFrameId, global::CardGame.Core.CardKind? sourceCard, global::CardGame.Core.JudgmentContinuationKind continuation, global::CardGame.Core.Card? delayedCard = default, int? sourceSeat = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility? programResultVisibility = default, global::CardGame.Core.OriginalUseLightningAttachment? originalUseLightning = default, int? originalUseLightningExcludedCardId = default) => _resolve().BeginJudgment(attack, targetSeat, reason, parentFrameId, sourceCard, continuation, delayedCard, sourceSeat, programResultBind, programResultVisibility, originalUseLightning, originalUseLightningExcludedCardId);
    internal void BeginJudgmentReplacementChoice(global::CardGame.Core.JudgmentFrame pending) => _resolve().BeginJudgmentReplacementChoice(pending);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.JudgmentTriggerCandidate> CollectJudgmentReplacementCandidates(global::CardGame.Core.Card judgmentCard, int targetSeat, string reason) => _resolve().CollectJudgmentReplacementCandidates(judgmentCard, targetSeat, reason);
    internal void CompleteBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool succeeded) => _resolve().CompleteBaguaResponse(attack, succeeded);
    internal void CompleteDelayedJudgment(global::CardGame.Core.JudgmentFrame pending, bool succeeded) => _resolve().CompleteDelayedJudgment(pending, succeeded);
    internal bool? CompleteFinalizedJudgment(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card judgmentCard, bool succeeded) => _resolve().CompleteFinalizedJudgment(pending, judgmentCard, succeeded);
    internal void CompleteLightningJudgment(global::CardGame.Core.JudgmentFrame pending, bool hit) => _resolve().CompleteLightningJudgment(pending, hit);
    internal void CompleteLightningWithoutHit(long resolutionId, global::CardGame.Core.Card lightning, global::CardGame.Core.CharacterState target, int? judgmentCardId) => _resolve().CompleteLightningWithoutHit(resolutionId, lightning, target, judgmentCardId);
    internal global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending) => _resolve().CurrentJudgmentCandidate(pending);
    internal bool? FinalizeJudgment(global::CardGame.Core.JudgmentFrame pending) => _resolve().FinalizeJudgment(pending);
    internal void FinishProgramSkillJudgment(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card judgmentCard) => _resolve().FinishProgramSkillJudgment(pending, judgmentCard);
    internal void FinishResolvedJudgment(global::CardGame.Core.JudgmentFrame pending) => _resolve().FinishResolvedJudgment(pending);
    internal global::CardGame.Core.JudgmentFrame GetJudgmentFrame(long frameId) => _resolve().GetJudgmentFrame(frameId);
    internal bool IsProtectedJudgmentSourceEquipment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, int judgmentSubjectSeat, string judgmentReason) => _resolve().IsProtectedJudgmentSourceEquipment(owner, card, judgmentSubjectSeat, judgmentReason);
    internal void ReplaceJudgmentFrame(global::CardGame.Core.JudgmentFrame frame) => _resolve().ReplaceJudgmentFrame(frame);
    internal bool? ResolveBaguaJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender) => _resolve().ResolveBaguaJudgment(attack, defender);
    internal void ResumeAfterLightningDamage(int delayedSeat, int? excludedCardId = default) => _resolve().ResumeAfterLightningDamage(delayedSeat, excludedCardId);
    internal void ResumeCompletedJudgment(global::CardGame.Core.GameEngine.JudgmentCompletionReceipt receipt) => _resolve().ResumeCompletedJudgment(receipt);
    internal void SetJudgmentFrameStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => _resolve().SetJudgmentFrameStep(frameId, step);
}
