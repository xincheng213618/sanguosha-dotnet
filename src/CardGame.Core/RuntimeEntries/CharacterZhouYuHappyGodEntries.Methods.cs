#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuHappyGodEntries
{
    internal bool AdvanceHappyZyChildren() => _resolve().AdvanceHappyZyChildren();
    internal void AnswerHappyZy(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHappyZy(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyZy(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHappyZy(supplied);
    internal bool CanOfferHappyZy(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferHappyZy(c, t, w);
    internal void CaptureHappyZyHp(global::CardGame.Core.ProgramSkillHpLostEvent fact) => _resolve().CaptureHappyZyHp(fact);
    internal void CaptureHappyZyPrevented(global::CardGame.Core.IdentityHpDeductionPreventedEvent fact) => _resolve().CaptureHappyZyPrevented(fact);
    internal global::CardGame.Core.PromptChoice ChooseHappyZy(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseHappyZy(p, f);
    internal bool DrainHappyZy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainHappyZy(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyZyChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyZyChoices(f);
    internal (global::CardGame.Core.NativeLogicalDiscardInvocationRecordedEvent Invocation, global::CardGame.Core.NativeLogicalDiscardMaterialCompletedEvent Material)? HappyZyDiscard(int owner) => _resolve().HappyZyDiscard(owner);
    internal bool HappyZyDying() => _resolve().HappyZyDying();
    internal bool HappyZyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HappyZyFirstChild(f, child);
    internal bool HappyZyMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HappyZyMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? HappyZyObserverRoot() => _resolve().HappyZyObserverRoot();
    internal bool HappyZyRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().HappyZyRecoveryParent(f, recovery);
    internal void IssueHappyZy(global::CardGame.Core.ProgramSkillFrame f, bool skipped, global::CardGame.Core.HappyZyStage next) => _resolve().IssueHappyZy(f, skipped, next);
    internal void PublishHappyZy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHappyZy(f);
    internal bool ResumeHappyZy(long id) => _resolve().ResumeHappyZy(id);
    internal bool RetainIssuedHappyZy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainIssuedHappyZy(f);
    internal void ReturnHappyZyDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappyZyDraw(f, outcome);
    internal bool ReturnHappyZyMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappyZyMovement(f);
}
