#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuYongOlEntries
{
    internal bool AdvanceOrdinaryBingyiChildren() => _resolve().AdvanceOrdinaryBingyiChildren();
    internal void AnswerOrdinaryBingyi(PromptChoice selected) => _resolve().AnswerOrdinaryBingyi(selected);
    internal SkillProgramStepOutcome BeginOrdinaryBingyi(ProgramSkillFrame supplied) => _resolve().BeginOrdinaryBingyi(supplied);
    internal bool CanContinueIssuedOrdinaryGuYong(ProgramSkillFrame f) => _resolve().CanContinueIssuedOrdinaryGuYong(f);
    internal bool CanOfferOrdinaryBingyi(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferOrdinaryBingyi(c, trigger, context);
    internal PromptChoice ChooseOrdinaryBingyiAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseOrdinaryBingyiAi(decision, f);
    internal void ContinueOrdinaryBingyi(long id) => _resolve().ContinueOrdinaryBingyi(id);
    internal bool DrainOrdinaryBingyi(ProgramSkillFrame f) => _resolve().DrainOrdinaryBingyi(f);
    internal void FinishOrdinaryBingyi(ProgramSkillFrame f) => _resolve().FinishOrdinaryBingyi(f);
    internal bool IsOrdinaryBingyiDiscard(CardMovementRecord m, int owner) => _resolve().IsOrdinaryBingyiDiscard(m, owner);
    internal bool IsOrdinaryBingyiDrawMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsOrdinaryBingyiDrawMovement(f, m);
    internal bool IsOrdinaryGuYongBingyiTrigger(SkillProgramTrigger t) => _resolve().IsOrdinaryGuYongBingyiTrigger(t);
    internal bool IsOrdinaryGuYongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsOrdinaryGuYongRecoveryParent(f, recovery);
    internal IReadOnlyList<PromptChoice> OrdinaryBingyiChoices(ProgramSkillFrame f) => _resolve().OrdinaryBingyiChoices(f);
    internal bool OrdinaryBingyiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().OrdinaryBingyiFirstChild(f, child);
    internal bool OrdinaryBingyiMovementRoot(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().OrdinaryBingyiMovementRoot(c, context);
    internal ProgramSkillFrame? OrdinaryBingyiObserverRoot() => _resolve().OrdinaryBingyiObserverRoot();
    internal bool OrdinaryBingyiObserverSuffix(int i) => _resolve().OrdinaryBingyiObserverSuffix(i);
    internal bool OrdinaryBingyiPhaseSpent(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().OrdinaryBingyiPhaseSpent(owner, phase);
    internal bool PermitPaidOrdinaryBingyi(ProgramSkillFrame f) => _resolve().PermitPaidOrdinaryBingyi(f);
    internal void PublishOrdinaryBingyi(ProgramSkillFrame f) => _resolve().PublishOrdinaryBingyi(f);
    internal bool ResumeOrdinaryBingyi(long id) => _resolve().ResumeOrdinaryBingyi(id);
    internal void ReturnOrdinaryBingyiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnOrdinaryBingyiDraw(f, outcome);
    internal bool ReturnOrdinaryBingyiMovement(ProgramSkillFrame f) => _resolve().ReturnOrdinaryBingyiMovement(f);
    internal bool HasOrdinaryGuYongCapability { get => _resolve().HasOrdinaryGuYongCapability; }
}
