#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicPaymentEntries
{
    internal void BeginDrawFundedDistinctBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need) => _resolve().BeginDrawFundedDistinctBasic(actor, source, kind, targets, original, need);
    internal void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => _resolve().BeginDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    internal void CancelDrawFundedDistinctBasicAttempt(global::CardGame.Core.DrawFundedDistinctBasicFrame frame) => _resolve().CancelDrawFundedDistinctBasicAttempt(frame);
    internal void ContinueDrawFundedDistinctBasic(long frameId) => _resolve().ContinueDrawFundedDistinctBasic(frameId);
    internal bool DrawFundedDistinctBasicOriginalNeedCurrent(global::CardGame.Core.DrawFundedDistinctBasicFrame frame) => _resolve().DrawFundedDistinctBasicOriginalNeedCurrent(frame);
    internal bool IsDrawFundedDistinctBasicAnswer(global::CardGame.Core.ChoiceId choice) => _resolve().IsDrawFundedDistinctBasicAnswer(choice);
    internal bool IsDrawFundedDistinctBasicHpParent(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame child) => _resolve().IsDrawFundedDistinctBasicHpParent(parent, child);
    internal bool IsDrawFundedDistinctBasicRecoveryParent(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().IsDrawFundedDistinctBasicRecoveryParent(parent, child);
    internal void ResumeDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => _resolve().ResumeDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    internal void ResumeDrawFundedDistinctBasicHpChange(global::CardGame.Core.HpChangedTriggerWindowFrame completed) => _resolve().ResumeDrawFundedDistinctBasicHpChange(completed);
    internal void ResumeDrawFundedDistinctBasicMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame completed) => _resolve().ResumeDrawFundedDistinctBasicMovement(completed);
    internal void ResumeDrawFundedDistinctBasicRecovery(global::CardGame.Core.RecoveryReplacementFrame completed) => _resolve().ResumeDrawFundedDistinctBasicRecovery(completed);
    internal global::CardGame.Core.CommandResult SubmitDrawFundedDistinctBasicAnswer(int actorSeat, global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.PromptChoice choice) => _resolve().SubmitDrawFundedDistinctBasicAnswer(actorSeat, prompt, choice);
    internal global::CardGame.Core.CommandResult SubmitDrawFundedDistinctBasicAnswer2(int actorSeat, global::CardGame.Core.PromptId promptId, global::CardGame.Core.ChoiceId choiceId) => _resolve().SubmitDrawFundedDistinctBasicAnswer(actorSeat, promptId, choiceId);
    internal bool ValidDrawFundedDistinctBasicPayment(global::CardGame.Core.DrawFundedDistinctBasicPayment p) => _resolve().ValidDrawFundedDistinctBasicPayment(p);
}
