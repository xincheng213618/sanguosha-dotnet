#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFengXiAnniversaryEntries
{
    internal bool AdvanceFengXiChildren() => _resolve().AdvanceFengXiChildren();
    internal void AnswerFengXi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerFengXi(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFengXi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginFengXi(supplied, operation);
    internal void BindFengXiLossPool(global::CardGame.Core.ProgramSkillFrame f, int target, int amount) => _resolve().BindFengXiLossPool(f, target, amount);
    internal bool CanOfferFengXi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferFengXi(c, t, w);
    internal void CaptureFengXiLoss(global::CardGame.Core.ProgramSkillHpLostEvent fact) => _resolve().CaptureFengXiLoss(fact);
    internal void CapturePreventedFengXiLoss(global::CardGame.Core.IdentityHpDeductionPreventedEvent fact) => _resolve().CapturePreventedFengXiLoss(fact);
    internal global::CardGame.Core.PromptChoice ChooseFengXi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseFengXi(p, f);
    internal bool ContinueFengXi(long id) => _resolve().ContinueFengXi(id);
    internal bool DrainFengXi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainFengXi(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FengXiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FengXiChoices(f);
    internal bool FengXiDyingObserver() => _resolve().FengXiDyingObserver();
    internal bool FengXiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().FengXiFirstChild(f, child);
    internal bool FengXiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().FengXiMovement(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? FengXiObserverRoot() => _resolve().FengXiObserverRoot();
    internal bool FengXiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().FengXiRecoveryParent(f, recovery);
    internal void FinishFengXiOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishFengXiOperation(f);
    internal void PublishFengXi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishFengXi(f);
    internal bool RetainIssuedFengXi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainIssuedFengXi(f);
    internal void ReturnFengXiDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome d) => _resolve().ReturnFengXiDraw(f, d);
    internal bool ReturnFengXiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnFengXiMovement(f);
}
