#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuHappyGodEntries
{
    internal bool AdvanceHappyZyChildren() => _resolve().AdvanceHappyZyChildren();
    internal void AnswerHappyZy(PromptChoice choice) => _resolve().AnswerHappyZy(choice);
    internal SkillProgramStepOutcome BeginHappyZy(ProgramSkillFrame supplied) => _resolve().BeginHappyZy(supplied);
    internal bool CanOfferHappyZy(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferHappyZy(c, t, w);
    internal void CaptureHappyZyHp(ProgramSkillHpLostEvent fact) => _resolve().CaptureHappyZyHp(fact);
    internal void CaptureHappyZyPrevented(IdentityHpDeductionPreventedEvent fact) => _resolve().CaptureHappyZyPrevented(fact);
    internal PromptChoice ChooseHappyZy(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseHappyZy(p, f);
    internal bool DrainHappyZy(ProgramSkillFrame f) => _resolve().DrainHappyZy(f);
    internal IReadOnlyList<PromptChoice> HappyZyChoices(ProgramSkillFrame f) => _resolve().HappyZyChoices(f);
    internal (NativeLogicalDiscardInvocationRecordedEvent Invocation, NativeLogicalDiscardMaterialCompletedEvent Material)? HappyZyDiscard(int owner) => _resolve().HappyZyDiscard(owner);
    internal bool HappyZyDying() => _resolve().HappyZyDying();
    internal bool HappyZyFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HappyZyFirstChild(f, child);
    internal bool HappyZyMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().HappyZyMovement(f, e, p);
    internal ProgramSkillFrame? HappyZyObserverRoot() => _resolve().HappyZyObserverRoot();
    internal bool HappyZyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().HappyZyRecoveryParent(f, recovery);
    internal void IssueHappyZy(ProgramSkillFrame f, bool skipped, HappyZyStage next) => _resolve().IssueHappyZy(f, skipped, next);
    internal void PublishHappyZy(ProgramSkillFrame f) => _resolve().PublishHappyZy(f);
    internal bool ResumeHappyZy(long id) => _resolve().ResumeHappyZy(id);
    internal bool RetainIssuedHappyZy(ProgramSkillFrame f) => _resolve().RetainIssuedHappyZy(f);
    internal void ReturnHappyZyDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnHappyZyDraw(f, outcome);
    internal bool ReturnHappyZyMovement(ProgramSkillFrame f) => _resolve().ReturnHappyZyMovement(f);
}
