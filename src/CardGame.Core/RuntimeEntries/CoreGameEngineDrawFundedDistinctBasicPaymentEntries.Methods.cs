#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicPaymentEntries
{
    internal void BeginDrawFundedDistinctBasic(CharacterState actor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets, PendingDecision original, GameEngine.DrawFundedBasicNeed need) => _resolve().BeginDrawFundedDistinctBasic(actor, source, kind, targets, original, need);
    internal void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => _resolve().BeginDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    internal void CancelDrawFundedDistinctBasicAttempt(DrawFundedDistinctBasicFrame frame) => _resolve().CancelDrawFundedDistinctBasicAttempt(frame);
    internal void ContinueDrawFundedDistinctBasic(long frameId) => _resolve().ContinueDrawFundedDistinctBasic(frameId);
    internal bool DrawFundedDistinctBasicOriginalNeedCurrent(DrawFundedDistinctBasicFrame frame) => _resolve().DrawFundedDistinctBasicOriginalNeedCurrent(frame);
    internal bool IsDrawFundedDistinctBasicAnswer(ChoiceId choice) => _resolve().IsDrawFundedDistinctBasicAnswer(choice);
    internal bool IsDrawFundedDistinctBasicHpParent(ResolutionFrame? parent, HpChangedTriggerWindowFrame child) => _resolve().IsDrawFundedDistinctBasicHpParent(parent, child);
    internal bool IsDrawFundedDistinctBasicRecoveryParent(ResolutionFrame? parent, RecoveryReplacementFrame child) => _resolve().IsDrawFundedDistinctBasicRecoveryParent(parent, child);
    internal void ResumeDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => _resolve().ResumeDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    internal void ResumeDrawFundedDistinctBasicHpChange(HpChangedTriggerWindowFrame completed) => _resolve().ResumeDrawFundedDistinctBasicHpChange(completed);
    internal void ResumeDrawFundedDistinctBasicMovement(CardsMovedTriggerWindowFrame completed) => _resolve().ResumeDrawFundedDistinctBasicMovement(completed);
    internal void ResumeDrawFundedDistinctBasicRecovery(RecoveryReplacementFrame completed) => _resolve().ResumeDrawFundedDistinctBasicRecovery(completed);
    internal CommandResult SubmitDrawFundedDistinctBasicAnswer(int actorSeat, PendingDecision prompt, PromptChoice choice) => _resolve().SubmitDrawFundedDistinctBasicAnswer(actorSeat, prompt, choice);
    internal CommandResult SubmitDrawFundedDistinctBasicAnswer2(int actorSeat, PromptId promptId, ChoiceId choiceId) => _resolve().SubmitDrawFundedDistinctBasicAnswer(actorSeat, promptId, choiceId);
    internal bool ValidDrawFundedDistinctBasicPayment(DrawFundedDistinctBasicPayment p) => _resolve().ValidDrawFundedDistinctBasicPayment(p);
}
