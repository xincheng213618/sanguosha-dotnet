#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiFuRenOlEntries
{
    internal bool AdvanceOrdinaryQietingChildren() => _resolve().AdvanceOrdinaryQietingChildren();
    internal void AnswerOrdinaryQieting(PromptChoice selected) => _resolve().AnswerOrdinaryQieting(selected);
    internal SkillProgramStepOutcome BeginOrdinaryQieting(ProgramSkillFrame supplied) => _resolve().BeginOrdinaryQieting(supplied);
    internal bool CanActivateOrdinaryCaiFuRenXianzhou(ProgramActivationLegalityContext c) => _resolve().CanActivateOrdinaryCaiFuRenXianzhou(c);
    internal bool CanContinueIssuedOrdinaryCaiFuRen(ProgramSkillFrame f) => _resolve().CanContinueIssuedOrdinaryCaiFuRen(f);
    internal bool CanMoveOrdinaryQietingEquipment(ProgramSkillFrame f, Card card) => _resolve().CanMoveOrdinaryQietingEquipment(f, card);
    internal bool CaptureOrdinaryQietingFinalUse(CardUseFrame use) => _resolve().CaptureOrdinaryQietingFinalUse(use);
    internal PromptChoice ChooseOrdinaryQietingAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseOrdinaryQietingAi(decision, f);
    internal PromptChoice ChooseOrdinaryXianzhouAi(PendingDecision d, ProgramSkillFrame f) => _resolve().ChooseOrdinaryXianzhouAi(d, f);
    internal void ContinueOrdinaryQieting(long id) => _resolve().ContinueOrdinaryQieting(id);
    internal ProgramSkillWindowContext CreateOrdinaryCaiFuRenQietingContext(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CreateOrdinaryCaiFuRenQietingContext(parent, candidate, context);
    internal bool DrainOrdinaryQieting(ProgramSkillFrame f) => _resolve().DrainOrdinaryQieting(f);
    internal void FinishOrdinaryQieting(ProgramSkillFrame f) => _resolve().FinishOrdinaryQieting(f);
    internal bool IsEligibleOrdinaryCaiFuRenQietingCandidate(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsEligibleOrdinaryCaiFuRenQietingCandidate(parent, candidate, context);
    internal bool IsIssuedOrdinaryCaiFuRenXianzhou(ProgramSkillFrame f) => _resolve().IsIssuedOrdinaryCaiFuRenXianzhou(f);
    internal bool IsOrdinaryCaiFuRenQietingTrigger(SkillProgramTrigger trigger) => _resolve().IsOrdinaryCaiFuRenQietingTrigger(trigger);
    internal bool IsOrdinaryCaiFuRenRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsOrdinaryCaiFuRenRecoveryParent(f, recovery);
    internal bool IsOrdinaryQietingPaidMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsOrdinaryQietingPaidMovement(f, m);
    internal void ObserveOrdinaryQietingUses(IGameEvent e) => _resolve().ObserveOrdinaryQietingUses(e);
    internal IReadOnlyList<PromptChoice> OrdinaryQietingChoices(ProgramSkillFrame f) => _resolve().OrdinaryQietingChoices(f);
    internal bool OrdinaryQietingFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().OrdinaryQietingFirstChild(f, child);
    internal ProgramSkillFrame? OrdinaryQietingObserverRoot() => _resolve().OrdinaryQietingObserverRoot();
    internal bool OrdinaryQietingObserverSuffix(int index) => _resolve().OrdinaryQietingObserverSuffix(index);
    internal bool OrdinaryQietingUsedOnAnother(OrdinaryQietingTurnContext c) => _resolve().OrdinaryQietingUsedOnAnother(c);
    internal bool PermitPaidOrdinaryCaiFuRen(ProgramSkillFrame f) => _resolve().PermitPaidOrdinaryCaiFuRen(f);
    internal void PublishOrdinaryQieting(ProgramSkillFrame f) => _resolve().PublishOrdinaryQieting(f);
    internal void RecordOrdinaryQietingUse(CardUseFrame use) => _resolve().RecordOrdinaryQietingUse(use);
    internal bool ResumeOrdinaryQieting(long id) => _resolve().ResumeOrdinaryQieting(id);
    internal void ReturnOrdinaryQietingDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnOrdinaryQietingDraw(f, outcome);
    internal bool ReturnOrdinaryQietingMovement(ProgramSkillFrame f) => _resolve().ReturnOrdinaryQietingMovement(f);
    internal bool ReturnPaidOrdinaryXianzhouMovement(ProgramSkillFrame f) => _resolve().ReturnPaidOrdinaryXianzhouMovement(f);
    internal bool HasOrdinaryCaiFuRenCapability { get => _resolve().HasOrdinaryCaiFuRenCapability; }
}
