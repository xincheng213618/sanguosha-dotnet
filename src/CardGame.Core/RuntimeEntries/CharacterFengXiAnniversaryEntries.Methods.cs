#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFengXiAnniversaryEntries
{
    internal bool AdvanceFengXiChildren() => _resolve().AdvanceFengXiChildren();
    internal void AnswerFengXi(PromptChoice choice) => _resolve().AnswerFengXi(choice);
    internal SkillProgramStepOutcome BeginFengXi(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginFengXi(supplied, operation);
    internal void BindFengXiLossPool(ProgramSkillFrame f, int target, int amount) => _resolve().BindFengXiLossPool(f, target, amount);
    internal bool CanOfferFengXi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferFengXi(c, t, w);
    internal void CaptureFengXiLoss(ProgramSkillHpLostEvent fact) => _resolve().CaptureFengXiLoss(fact);
    internal void CapturePreventedFengXiLoss(IdentityHpDeductionPreventedEvent fact) => _resolve().CapturePreventedFengXiLoss(fact);
    internal PromptChoice ChooseFengXi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseFengXi(p, f);
    internal bool ContinueFengXi(long id) => _resolve().ContinueFengXi(id);
    internal bool DrainFengXi(ProgramSkillFrame f) => _resolve().DrainFengXi(f);
    internal IReadOnlyList<PromptChoice> FengXiChoices(ProgramSkillFrame f) => _resolve().FengXiChoices(f);
    internal bool FengXiDyingObserver() => _resolve().FengXiDyingObserver();
    internal bool FengXiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().FengXiFirstChild(f, child);
    internal bool FengXiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().FengXiMovement(f, effect, p);
    internal ProgramSkillFrame? FengXiObserverRoot() => _resolve().FengXiObserverRoot();
    internal bool FengXiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().FengXiRecoveryParent(f, recovery);
    internal void FinishFengXiOperation(ProgramSkillFrame f) => _resolve().FinishFengXiOperation(f);
    internal void PublishFengXi(ProgramSkillFrame f) => _resolve().PublishFengXi(f);
    internal bool RetainIssuedFengXi(ProgramSkillFrame f) => _resolve().RetainIssuedFengXi(f);
    internal void ReturnFengXiDraw(ProgramSkillFrame f, NativeDrawOutcome d) => _resolve().ReturnFengXiDraw(f, d);
    internal bool ReturnFengXiMovement(ProgramSkillFrame f) => _resolve().ReturnFengXiMovement(f);
}
