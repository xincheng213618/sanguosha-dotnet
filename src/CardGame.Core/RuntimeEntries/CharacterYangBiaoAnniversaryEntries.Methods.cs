#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangBiaoAnniversaryEntries
{
    internal bool AdvanceYangBiaoSubtree() => _resolve().AdvanceYangBiaoSubtree();
    internal void AnswerYangBiao(global::CardGame.Core.PromptChoice c) => _resolve().AnswerYangBiao(c);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYangBiao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginYangBiao(supplied, op);
    internal bool CanOfferYangBiao(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanOfferYangBiao(g);
    internal void CompleteYangBiao(global::CardGame.Core.ProgramSkillFrame f, bool rescuePaid = false) => _resolve().CompleteYangBiao(f, rescuePaid);
    internal void CompleteYangBiaoNative(global::CardGame.Core.ProgramSkillFrame f, bool? hadAttack = default) => _resolve().CompleteYangBiaoNative(f, hadAttack);
    internal bool ContinueYangBiao(long id) => _resolve().ContinueYangBiao(id);
    internal bool DrainYangBiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainYangBiao(f);
    internal void FinishYangBiaoUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishYangBiaoUse(f);
    internal bool IsAnniversaryYangBiaoFiveGrainsUse(long id) => _resolve().IsAnniversaryYangBiaoFiveGrainsUse(id);
    internal bool IsAnniversaryYangBiaoVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsAnniversaryYangBiaoVirtualUse(use);
    internal bool IsOriginalDyingSuspendedByAnniversaryYangBiao(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByAnniversaryYangBiao(dying);
    internal void IssueYangBiaoSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueYangBiaoSlash(f);
    internal void IssueYangBiaoUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueYangBiaoUse(f);
    internal bool KeepPaidYangBiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidYangBiao(f);
    internal void NextYangBiaoUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NextYangBiaoUse(f);
    internal void PayYangBiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayYangBiao(f);
    internal void PublishYangBiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishYangBiao(f);
    internal bool ReturnYangBiaoAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnYangBiaoAttack(completion);
    internal void ReturnYangBiaoDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnYangBiaoDraw(supplied, draw);
    internal bool ReturnYangBiaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnYangBiaoMovement(f);
    internal bool TryReturnAnniversaryYangBiaoVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().TryReturnAnniversaryYangBiaoVirtualUse(use);
    internal bool ValidYangBiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidYangBiao(f);
    internal bool ValidYangBiaoUseCursor(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidYangBiaoUseCursor(f);
    internal global::CardGame.Core.PromptChoice YangBiaoAi(global::CardGame.Core.PendingDecision p) => _resolve().YangBiaoAi(p);
    internal global::CardGame.Core.AnniversaryYangBiaoDrawArmedEvent? YangBiaoArm(int owner, string instance, global::CardGame.Core.ProgramSkillWindowContext? context = default) => _resolve().YangBiaoArm(owner, instance, context);
    internal bool YangBiaoBeforeDamageObserver(long id) => _resolve().YangBiaoBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> YangBiaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YangBiaoChoices(f);
    internal bool YangBiaoDamageObserver(long id) => _resolve().YangBiaoDamageObserver(id);
    internal int YangBiaoDiscardCount() => _resolve().YangBiaoDiscardCount();
    internal long YangBiaoDrawPhaseId(global::CardGame.Core.ProgramSkillWindowContext? context = default) => _resolve().YangBiaoDrawPhaseId(context);
    internal bool YangBiaoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().YangBiaoFirstChild(f, child);
    internal bool YangBiaoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().YangBiaoMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? YangBiaoObserverRoot() => _resolve().YangBiaoObserverRoot();
    internal bool YangBiaoOriginal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YangBiaoOriginal(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> YangBiaoPaymentChoices(global::CardGame.Core.ProgramSkillFrame f, bool gift) => _resolve().YangBiaoPaymentChoices(f, gift);
    internal bool YangBiaoPaymentProof(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YangBiaoPaymentProof(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.AnniversaryYangBiaoPlan> YangBiaoPlans(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YangBiaoPlans(f);
    internal bool YangBiaoRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().YangBiaoRecoveryParent(f, child);
    internal global::CardGame.Core.CardConversionSource YangBiaoSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YangBiaoSource(f);
}
