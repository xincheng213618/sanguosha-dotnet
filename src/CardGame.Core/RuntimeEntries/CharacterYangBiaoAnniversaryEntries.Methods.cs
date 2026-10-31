#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangBiaoAnniversaryEntries
{
    internal bool AdvanceYangBiaoSubtree() => _resolve().AdvanceYangBiaoSubtree();
    internal void AnswerYangBiao(PromptChoice c) => _resolve().AnswerYangBiao(c);
    internal SkillProgramStepOutcome BeginYangBiao(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginYangBiao(supplied, op);
    internal bool CanOfferYangBiao(ProgramTriggerLegalityContext g) => _resolve().CanOfferYangBiao(g);
    internal void CompleteYangBiao(ProgramSkillFrame f, bool rescuePaid = false) => _resolve().CompleteYangBiao(f, rescuePaid);
    internal void CompleteYangBiaoNative(ProgramSkillFrame f, bool? hadAttack = default) => _resolve().CompleteYangBiaoNative(f, hadAttack);
    internal bool ContinueYangBiao(long id) => _resolve().ContinueYangBiao(id);
    internal bool DrainYangBiao(ProgramSkillFrame f) => _resolve().DrainYangBiao(f);
    internal void FinishYangBiaoUse(ProgramSkillFrame f) => _resolve().FinishYangBiaoUse(f);
    internal bool IsAnniversaryYangBiaoFiveGrainsUse(long id) => _resolve().IsAnniversaryYangBiaoFiveGrainsUse(id);
    internal bool IsAnniversaryYangBiaoVirtualUse(CardUseFrame use) => _resolve().IsAnniversaryYangBiaoVirtualUse(use);
    internal bool IsOriginalDyingSuspendedByAnniversaryYangBiao(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByAnniversaryYangBiao(dying);
    internal void IssueYangBiaoSlash(ProgramSkillFrame f) => _resolve().IssueYangBiaoSlash(f);
    internal void IssueYangBiaoUse(ProgramSkillFrame f) => _resolve().IssueYangBiaoUse(f);
    internal bool KeepPaidYangBiao(ProgramSkillFrame f) => _resolve().KeepPaidYangBiao(f);
    internal void NextYangBiaoUse(ProgramSkillFrame f) => _resolve().NextYangBiaoUse(f);
    internal void PayYangBiao(ProgramSkillFrame f) => _resolve().PayYangBiao(f);
    internal void PublishYangBiao(ProgramSkillFrame f) => _resolve().PublishYangBiao(f);
    internal bool ReturnYangBiaoAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnYangBiaoAttack(completion);
    internal void ReturnYangBiaoDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnYangBiaoDraw(supplied, draw);
    internal bool ReturnYangBiaoMovement(ProgramSkillFrame f) => _resolve().ReturnYangBiaoMovement(f);
    internal bool TryReturnAnniversaryYangBiaoVirtualUse(CardUseFrame use) => _resolve().TryReturnAnniversaryYangBiaoVirtualUse(use);
    internal bool ValidYangBiao(ProgramSkillFrame f) => _resolve().ValidYangBiao(f);
    internal bool ValidYangBiaoUseCursor(ProgramSkillFrame f) => _resolve().ValidYangBiaoUseCursor(f);
    internal PromptChoice YangBiaoAi(PendingDecision p) => _resolve().YangBiaoAi(p);
    internal AnniversaryYangBiaoDrawArmedEvent? YangBiaoArm(int owner, string instance, ProgramSkillWindowContext? context = default) => _resolve().YangBiaoArm(owner, instance, context);
    internal IReadOnlyList<PromptChoice> YangBiaoChoices(ProgramSkillFrame f) => _resolve().YangBiaoChoices(f);
    internal int YangBiaoDiscardCount() => _resolve().YangBiaoDiscardCount();
    internal long YangBiaoDrawPhaseId(ProgramSkillWindowContext? context = default) => _resolve().YangBiaoDrawPhaseId(context);
    internal bool YangBiaoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().YangBiaoFirstChild(f, child);
    internal ProgramSkillFrame? YangBiaoObserverRoot() => _resolve().YangBiaoObserverRoot();
    internal bool YangBiaoOriginal(ProgramSkillFrame f) => _resolve().YangBiaoOriginal(f);
    internal IReadOnlyList<PromptChoice> YangBiaoPaymentChoices(ProgramSkillFrame f, bool gift) => _resolve().YangBiaoPaymentChoices(f, gift);
    internal bool YangBiaoPaymentProof(ProgramSkillFrame f) => _resolve().YangBiaoPaymentProof(f);
    internal IReadOnlyList<AnniversaryYangBiaoPlan> YangBiaoPlans(ProgramSkillFrame f) => _resolve().YangBiaoPlans(f);
    internal bool YangBiaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child) => _resolve().YangBiaoRecoveryParent(f, child);
    internal CardConversionSource YangBiaoSource(ProgramSkillFrame f) => _resolve().YangBiaoSource(f);
}
